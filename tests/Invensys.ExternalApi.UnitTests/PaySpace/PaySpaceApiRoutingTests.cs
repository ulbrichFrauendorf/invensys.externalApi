using System.Net;
using FluentAssertions;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core;
using Invensys.ExternalApi.PaySpace.Core.Apis.V2_0;
using Invensys.ExternalApi.PaySpace.Core.Attributes;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Moq;
using NUnit.Framework;

namespace Invensys.ExternalApi.UnitTests.PaySpace;

public class PaySpaceApiRoutingTests
{
   [TestCase(typeof(PaySpaceEmployeeApi), "odata/v2.0")]
   [TestCase(typeof(PaySpaceCompanyApi), "odata/v2.0")]
   [TestCase(typeof(PaySpaceLookupApi), "odata/v2.0")]
   [TestCase(typeof(PaySpaceLeaveApi), "odata/v2.0")]
   [TestCase(typeof(PaySpaceLoanApi), "odata/v2.0")]
   [TestCase(typeof(PaySpaceIncidentApi), "odata/v2.0")]
   [TestCase(typeof(PaySpacePayslipApi), "odata/v2.0")]
   [TestCase(typeof(PaySpacePayrollProcessingApi), "odata/v2.0")]
   [TestCase(typeof(PaySpaceReportApi), "v2.0")]
   [TestCase(typeof(AlternateApi), "custom/v3.1")]
   public void CollectionAttributesDetermineVersionAndSubdirectory(Type type, string expectedPrefix)
   {
      PaySpaceApiAttribute
         .BuildPath(type, "42/resource?frequency=Monthly%20Extra")
         .Should()
         .Be($"{expectedPrefix}/42/resource?frequency=Monthly%20Extra");
   }

   [Test]
   public async Task CollectionPrefixesInitialRequestAndFollowsAbsoluteNextPageWithoutDuplicatingPrefix()
   {
      using var routingMessageHandler = new RoutingMessageHandler();

      using var httpClient = new HttpClient(routingMessageHandler)
      {
         BaseAddress = new Uri("https://apistaging.payspace.com/")
      };

      var httpClientFactory = new Mock<IHttpClientFactory>();

      httpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

      var paySpaceAuthenticationProvider = new Mock<IPaySpaceAuthenticationProvider>();

      paySpaceAuthenticationProvider
         .Setup(provider => provider.GetAccessToken(It.IsAny<AccessTokenRequest>(), It.IsAny<bool>()))
         .ReturnsAsync("token");

      var paySpaceApiClient = new PaySpaceApiClient(httpClientFactory.Object, paySpaceAuthenticationProvider.Object);

      var paySpaceEmployeeApi = new PaySpaceEmployeeApi(paySpaceApiClient);

      var employees = await paySpaceEmployeeApi.EmployeeListAsync(
         new JwtAccessTokenRequest("client", "secret", "scope"),
         42
      );

      employees.Select(employee => employee.EmployeeId).Should().Equal(1L, 2L);

      routingMessageHandler.RequestCount.Should().Be(2);
   }

   [PaySpaceApi("v3.1", "custom")]
   private sealed class AlternateApi { }

   private sealed class RoutingMessageHandler : HttpMessageHandler
   {
      public int RequestCount { get; private set; }

      protected override Task<HttpResponseMessage> SendAsync(
         HttpRequestMessage httpRequestMessage,
         CancellationToken cancellationToken
      )
      {
         RequestCount++;

         httpRequestMessage
            .RequestUri!.AbsoluteUri.Should()
            .Be(
               RequestCount == 1
                  ? "https://apistaging.payspace.com/odata/v2.0/42/Employee"
                  : "https://apistaging.payspace.com/odata/v2.0/42/Employee?$skip=1"
            );

         return Task.FromResult(
            new HttpResponseMessage(HttpStatusCode.OK)
            {
               Content = new StringContent(
                  RequestCount == 1
                     ? """{"value":[{"EmployeeId":1}],"@odata.nextLink":"https://apistaging.payspace.com/odata/v2.0/42/Employee?$skip=1"}"""
                     : """{"value":[{"EmployeeId":2}]}"""
               )
            }
         );
      }
   }
}
