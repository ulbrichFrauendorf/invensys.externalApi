using System.Net;
using System.Text.Json;
using FluentAssertions;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core;
using Invensys.ExternalApi.PaySpace.Core.Apis.V2_0;
using Invensys.ExternalApi.PaySpace.Core.Configuration;
using Invensys.ExternalApi.PaySpace.Core.Exceptions;
using Invensys.ExternalApi.PaySpace.Entities.FixedInfo;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Moq;
using NUnit.Framework;

namespace Invensys.ExternalApi.UnitTests.PaySpace;

public class PaySpaceEmployeeWriteApiTests
{
   [Test]
   public async Task TypedEmployeeRequestSendsContactNumberAndPhysicalAddressAndReadsReturnedAddresses()
   {
      var credentials = new JwtAccessTokenRequest("id", "secret", "scope");

      var provider = new Mock<IPaySpaceAuthenticationProvider>();

      provider.Setup(p => p.GetAccessToken(credentials, false)).ReturnsAsync("token");

      var handler = new Handler(async request =>
      {
         using var json = JsonDocument.Parse(await request.Content!.ReadAsStringAsync());

         json.RootElement.GetProperty("WorkNumber").GetString().Should().Be("0115550100");

         var addresses = json.RootElement.GetProperty("Address");

         addresses.GetArrayLength().Should().Be(1);

         addresses[0].GetProperty("AddressType").GetString().Should().Be("Physical");

         addresses[0].GetProperty("AddressCountry").GetString().Should().Be("ZA");

         addresses[0].GetProperty("Province").GetString().Should().Be("Gauteng");

         addresses[0].TryGetProperty("AddressId", out _).Should().BeFalse();

         return new HttpResponseMessage(HttpStatusCode.Created)
         {
            Content = new StringContent(
               """{"EmployeeId":42,"WorkNumber":"0115550100","Address":[{"AddressId":12,"AddressType":"Physical","AddressLine1":"Integration Test Street","Province":"Gauteng","AddressCountry":"ZA"}]}"""
            )
         };
      });

      var factory = new Mock<IHttpClientFactory>(MockBehavior.Strict);

      factory
         .Setup(f => f.CreateClient(PaySpaceHttpClient.PaySpaceRateLimitedApi))
         .Returns(() => new HttpClient(handler, false) { BaseAddress = new Uri("https://apistaging.payspace.com/") });

      var api = new PaySpaceEmployeeApi(new PaySpaceApiClient(factory.Object, provider.Object));

      var employee = await api.CreateEmployeeAsync(
         credentials,
         123,
         new CreateEmployeeRequest
         {
            WorkNumber = "0115550100",
            Address =
            [
               new CreateEmployeeAddressRequest
               {
                  AddressType = "Physical",
                  StreetNumber = "123",
                  AddressLine1 = "Integration Test Street",
                  AddressLine2 = "Sandton",
                  AddressLine3 = "Johannesburg",
                  AddressCode = "2196",
                  Province = "Gauteng",
                  AddressCountry = "ZA"
               }
            ]
         }
      );

      employee.Address.Should().ContainSingle().Which.AddressId.Should().Be(12);

      employee.WorkNumber.Should().Be("0115550100");

      factory.Verify(f => f.CreateClient(PaySpaceHttpClient.PaySpaceRateLimitedApi), Times.Once);

      factory.VerifyNoOtherCalls();
   }

   private const string ValidationResponse = """
      {"@odata.context":"https://apistaging.payspace.com/odata/v2.0/9923/$metadata#Employee/$entity","Message":"One contact number required, Physical address required","Details":[{"Message":"One contact number required","Field":null},{"Message":"Physical address required","Field":"Address"}],"Success":false}
      """;

   [TestCase(HttpStatusCode.BadRequest, false)]
   [TestCase(HttpStatusCode.UnprocessableEntity, true)]
   [TestCase(HttpStatusCode.OK, false)]
   [TestCase(HttpStatusCode.OK, true)]
   public async Task ValidationFailureExposesSummaryFieldAndRecordErrorsWithoutRetry(HttpStatusCode status, bool typed)
   {
      var attempts = 0;

      var credentials = new JwtAccessTokenRequest("id", "secret", "scope");

      var api = ErrorResponseApi(() =>
      {
         attempts++;

         return new HttpResponseMessage(status) { Content = new StringContent(ValidationResponse) };
      });

      Func<Task> action = () =>
         typed
            ? api.CreateEmployeeAsync(credentials, 9923, new CreateEmployeeRequest { FirstName = "Test" })
            : api.CreateEmployeeAsync(credentials, 9923, JsonSerializer.SerializeToElement(new { FirstName = "Test" }));

      var exception = (await action.Should().ThrowAsync<PaySpaceValidationException>()).Which;

      exception.StatusCode.Should().Be(status);

      exception.Message.Should().Be("One contact number required, Physical address required");

      exception.Response.Success.Should().BeFalse();

      exception.Response.OdataContext.Should().EndWith("#Employee/$entity");

      exception.Details.Should().HaveCount(2);

      exception.Details[0].Field.Should().BeNull();

      exception.Details[0].Message.Should().Be("One contact number required");

      exception.Details[1].Field.Should().Be("Address");

      exception.Details[1].Message.Should().Be("Physical address required");

      attempts.Should().Be(1);
   }

   [TestCase("EmploymentStatus")]
   [TestCase("Position")]
   [TestCase("BankDetail")]
   public async Task OtherOnboardingOperationsAlsoRejectValidationEnvelopes(string operation)
   {
      var credentials = new JwtAccessTokenRequest("id", "secret", "scope");

      var api = ErrorResponseApi(
         () => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(ValidationResponse) }
      );

      Func<Task> action = () =>
         operation switch
         {
            "EmploymentStatus"
               => api.CreateEmploymentStatusAsync(credentials, 9923, new CreateEmploymentStatusRequest()),
            "Position" => api.CreatePositionAsync(credentials, 9923, new CreatePositionRequest()),
            _ => api.CreateBankDetailAsync(credentials, 9923, new CreateBankDetailRequest())
         };

      await action.Should().ThrowAsync<PaySpaceValidationException>();
   }

   [TestCase("{\"Success\":false}", "PaySpace rejected the record.", 0)]
   [TestCase("{\"Success\":false,\"Details\":null}", "PaySpace rejected the record.", 0)]
   [TestCase(
      "{\"Success\":false,\"Details\":[{\"Message\":\"Required\",\"Field\":\"Address\"},{\"Message\":\"Invalid\",\"Field\":\"Address\"}]}",
      "Required, Invalid",
      2
   )]
   public async Task MissingSummaryFallsBackToDetailsWithoutLosingRepeatedFieldErrors(
      string json,
      string message,
      int count
   )
   {
      var api = ErrorResponseApi(
         () => new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(json) }
      );

      var exception = (
         await FluentActions
            .Invoking(
               () =>
                  api.CreateEmployeeAsync(
                     new JwtAccessTokenRequest("id", "secret", "scope"),
                     9923,
                     new CreateEmployeeRequest()
                  )
            )
            .Should()
            .ThrowAsync<PaySpaceValidationException>()
      ).Which;

      exception.Message.Should().Be(message);

      exception.Details.Should().HaveCount(count);
   }

   [TestCase("<html>Gateway unavailable</html>")]
   [TestCase("")]
   [TestCase("{\"Message\":\"Server unavailable\"}")]
   public async Task NonValidationHttpFailuresPreserveTransportExceptionAndStatus(string content)
   {
      var api = ErrorResponseApi(
         () => new HttpResponseMessage(HttpStatusCode.BadGateway) { Content = new StringContent(content) }
      );

      var exception = (
         await FluentActions
            .Invoking(
               () =>
                  api.CreateEmployeeAsync(
                     new JwtAccessTokenRequest("id", "secret", "scope"),
                     9923,
                     new CreateEmployeeRequest()
                  )
            )
            .Should()
            .ThrowAsync<HttpRequestException>()
      ).Which;

      exception.Should().NotBeOfType<PaySpaceValidationException>();

      exception.StatusCode.Should().Be(HttpStatusCode.BadGateway);
   }

   [Test]
   public async Task SuccessfulResponseWithMessageIsNotAValidationFailure()
   {
      var api = ErrorResponseApi(
         () =>
            new HttpResponseMessage(HttpStatusCode.Created)
            {
               Content = new StringContent("{\"Success\":true,\"Message\":\"Created\",\"EmployeeId\":42}")
            }
      );

      var result = await api.CreateEmployeeAsync(
         new JwtAccessTokenRequest("id", "secret", "scope"),
         9923,
         new CreateEmployeeRequest()
      );

      result.EmployeeId.Should().Be(42);
   }

   private static PaySpaceEmployeeApi ErrorResponseApi(Func<HttpResponseMessage> respond)
   {
      var provider = new Mock<IPaySpaceAuthenticationProvider>();

      provider.Setup(p => p.GetAccessToken(It.IsAny<JwtAccessTokenRequest>(), It.IsAny<bool>())).ReturnsAsync("token");

      var handler = new Handler(_ => Task.FromResult(respond()));

      var factory = new Mock<IHttpClientFactory>();

      factory
         .Setup(f => f.CreateClient(It.IsAny<string>()))
         .Returns(() => new HttpClient(handler, false) { BaseAddress = new Uri("https://apistaging.payspace.com/") });

      return new PaySpaceEmployeeApi(new PaySpaceApiClient(factory.Object, provider.Object));
   }

   [Test]
   public async Task TypedEmployeeRequestUsesMetadataNamesDatesAndOmitNullsAndReturnsTypedResponse()
   {
      var credentials = new JwtAccessTokenRequest("id", "secret", "scope");

      var provider = new Mock<IPaySpaceAuthenticationProvider>();

      provider.Setup(p => p.GetAccessToken(credentials, false)).ReturnsAsync("token");

      var handler = new Handler(async request =>
      {
         using var body = JsonDocument.Parse(await request.Content!.ReadAsStringAsync());

         body.RootElement.GetProperty("Birthday").GetString().Should().Be("2000-01-02");

         body.RootElement.GetProperty("FirstName").GetString().Should().Be("Test");

         body.RootElement.TryGetProperty("LastName", out _).Should().BeFalse();

         return new HttpResponseMessage(HttpStatusCode.Created)
         {
            Content = new StringContent(
               "{\"EmployeeId\":42,\"FirstName\":\"Test\",\"Birthday\":\"2000-01-02T09:28:15.342Z\"}"
            )
         };
      });

      var factory = new Mock<IHttpClientFactory>();

      factory
         .Setup(f => f.CreateClient(It.IsAny<string>()))
         .Returns(() => new HttpClient(handler, false) { BaseAddress = new Uri("https://apistaging.payspace.com/") });

      var api = new PaySpaceEmployeeApi(new PaySpaceApiClient(factory.Object, provider.Object));

      var result = await api.CreateEmployeeAsync(
         credentials,
         123,
         new CreateEmployeeRequest { FirstName = "Test", Birthday = new DateOnly(2000, 1, 2) }
      );

      result.EmployeeId.Should().Be(42);

      result.Birthday.Should().Be(new DateOnly(2000, 1, 2));
   }

   [TestCase("Employee")]
   [TestCase("EmployeeEmploymentStatus")]
   [TestCase("EmployeePosition")]
   [TestCase("EmployeeBankDetail")]
   public async Task CreatesRecordUsingDocumentedPathAndPreservesFields(string entity)
   {
      var credentials = new JwtAccessTokenRequest("id", "secret", "scope");

      var provider = new Mock<IPaySpaceAuthenticationProvider>();

      provider.Setup(p => p.GetAccessToken(credentials, false)).ReturnsAsync("token");

      var handler = new Handler(async request =>
      {
         request.Method.Should().Be(HttpMethod.Post);

         request.RequestUri!.AbsoluteUri.Should().Be($"https://apistaging.payspace.com/odata/v2.0/123/{entity}");

         request.Headers.Authorization!.Parameter.Should().Be("token");

         using var body = JsonDocument.Parse(await request.Content!.ReadAsStringAsync());

         body.RootElement.GetProperty("EmployeeNumber").GetString().Should().Be("TEST001");

         return new HttpResponseMessage(HttpStatusCode.Created)
         {
            Content = new StringContent("{\"EmployeeNumber\":\"TEST001\"}")
         };
      });

      var factory = new Mock<IHttpClientFactory>();

      factory
         .Setup(f => f.CreateClient(It.IsAny<string>()))
         .Returns(() => new HttpClient(handler, false) { BaseAddress = new Uri("https://apistaging.payspace.com/") });

      var api = new PaySpaceEmployeeApi(new PaySpaceApiClient(factory.Object, provider.Object));

      var payload = JsonSerializer.SerializeToElement(new { EmployeeNumber = "TEST001" });

      var response = entity switch
      {
         "Employee" => await api.CreateEmployeeAsync(credentials, 123, payload),
         "EmployeeEmploymentStatus" => await api.CreateEmploymentStatusAsync(credentials, 123, payload),
         "EmployeePosition" => await api.CreatePositionAsync(credentials, 123, payload),
         _ => await api.CreateBankDetailAsync(credentials, 123, payload)
      };

      response.GetProperty("EmployeeNumber").GetString().Should().Be("TEST001");
   }

   [Test]
   public async Task DoesNotRetryFailedCreation()
   {
      var credentials = new JwtAccessTokenRequest("id", "secret", "scope");

      var provider = new Mock<IPaySpaceAuthenticationProvider>();

      provider.Setup(p => p.GetAccessToken(credentials, false)).ReturnsAsync("token");

      var attempts = 0;

      var handler = new Handler(_ =>
      {
         attempts++;
         return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
      });

      var factory = new Mock<IHttpClientFactory>();

      factory
         .Setup(f => f.CreateClient(It.IsAny<string>()))
         .Returns(() => new HttpClient(handler, false) { BaseAddress = new Uri("https://apistaging.payspace.com/") });

      var api = new PaySpaceEmployeeApi(new PaySpaceApiClient(factory.Object, provider.Object));

      await FluentActions
         .Invoking(() => api.CreateEmployeeAsync(credentials, 123, JsonSerializer.SerializeToElement(new { })))
         .Should()
         .ThrowAsync<HttpRequestException>();

      attempts.Should().Be(1);
   }

   private sealed class Handler(Func<HttpRequestMessage, Task<HttpResponseMessage>> send) : HttpMessageHandler
   {
      protected override Task<HttpResponseMessage> SendAsync(
         HttpRequestMessage request,
         CancellationToken cancellationToken
      ) => send(request);
   }
}
