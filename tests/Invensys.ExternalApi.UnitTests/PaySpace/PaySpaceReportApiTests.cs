using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace;
using Invensys.ExternalApi.PaySpace.Core.Apis.V2_0;
using Invensys.ExternalApi.PaySpace.Core.Configuration;
using Invensys.ExternalApi.PaySpace.Core.Context;
using Invensys.ExternalApi.PaySpace.Entities.FixedInfo;
using Invensys.ExternalApi.PaySpace.Entities.Reports;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Invensys.ExternalApi.PaySpace.Interfaces.V2_0;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Invensys.ExternalApi.UnitTests.PaySpace;

[TestFixture]
public class PaySpaceReportApiTests
{
   private readonly JwtAccessTokenRequest _credentials = new("client", "secret", "scope");
   private Mock<IPaySpaceAuthenticationProvider> _authentication = null!;
   private ReportHandler _handler = null!;
   private PaySpaceReportApi _api = null!;
   private string _directory = null!;

   [SetUp]
   public void SetUp()
   {
      _authentication = new Mock<IPaySpaceAuthenticationProvider>();

      _authentication.Setup(provider => provider.GetAccessToken(_credentials, false)).ReturnsAsync("original-token");

      _authentication.Setup(provider => provider.GetAccessToken(_credentials, true)).ReturnsAsync("refreshed-token");

      _handler = new ReportHandler();

      var factory = new Mock<IHttpClientFactory>(MockBehavior.Strict);

      factory
         .Setup(value => value.CreateClient(PaySpaceHttpClient.PaySpaceRateLimitedApi))
         .Returns((string _) => new HttpClient(_handler, false) { BaseAddress = new Uri("https://api.payspace.com/") });

      factory
         .Setup(value => value.CreateClient(PaySpaceHttpClient.PaySpaceReportDownload))
         .Returns((string _) => new HttpClient(_handler, false) { BaseAddress = new Uri("https://api.payspace.com/") });

      _api = new PaySpaceReportApi(factory.Object, _authentication.Object);

      _directory = Path.Combine(Path.GetTempPath(), $"payspace-report-tests-{Guid.NewGuid():N}");

      Directory.CreateDirectory(_directory);
   }

   [TearDown]
   public void TearDown()
   {
      _handler.Dispose();

      Directory.Delete(_directory, true);
   }

   [Test]
   public async Task GetReportsReadsPlainArrayOutsideOData()
   {
      _handler.Respond = (request, _) =>
      {
         request.RequestUri!.AbsoluteUri.Should().Be("https://api.payspace.com/v2.0/42/Report");

         request.Headers.Authorization!.Parameter.Should().Be("original-token");

         request.Headers.UserAgent.ToString().Should().Contain("payspace.com");

         return Task.FromResult(Json("""[{"Id":"available-id","Name":"Payroll","Country":"ZA","Format":"csv"}]"""));
      };

      var reports = await _api.GetReportsAsync(_credentials, 42);

      reports.Should().ContainSingle().Which.Id.Should().Be("available-id");

      reports[0].Format.Should().Be("csv");
   }

   [TestCase("""{"EmployeeNumber":{"type":"string"},"TaxYearId":[1662,1663]}""")]
   [TestCase("""[{"Name":"TaxYearId","Required":true}]""")]
   public async Task ParametersPreserveUnknownMetadataAndEscapeReportId(string json)
   {
      _handler.Respond = (request, _) =>
      {
         request
            .RequestUri!.AbsoluteUri.Should()
            .Be("https://api.payspace.com/v2.0/42/Report/parameters?reportId=report%2Fone%20%26two");

         return Task.FromResult(Json(json));
      };

      var parameters = await _api.GetReportParametersAsync(_credentials, 42, "report/one &two");

      parameters.GetRawText().Should().Be(json);
   }

   [TestCase(null, "")]
   [TestCase("Monthly & weekly", "?frequency=Monthly%20%26%20weekly")]
   public async Task StartSerializesVendorContractAndTypedDynamicParameters(string? frequency, string query)
   {
      _handler.Respond = async (request, _) =>
      {
         request.Method.Should().Be(HttpMethod.Post);

         request.RequestUri!.AbsoluteUri.Should().Be("https://api.payspace.com/v2.0/42/Report" + query);

         using var json = JsonDocument.Parse(await request.Content!.ReadAsStringAsync());

         json.RootElement.GetProperty("id").GetString().Should().Be("from-discovery");

         json.RootElement.GetProperty("disableBookmarks").GetBoolean().Should().BeTrue();

         var parameters = json.RootElement.GetProperty("parameters");

         parameters.GetProperty("TaxYearId").GetInt32().Should().Be(1662);

         parameters.GetProperty("EmployeeNumber").GetString().Should().Be("EMP001");

         parameters.GetProperty("IncludeTerminated").GetBoolean().Should().BeFalse();

         parameters.GetProperty("Optional").ValueKind.Should().Be(JsonValueKind.Null);

         return Json(
            """{"RequestId":"job-1","Message":"Report processing started","Link":"/v2.0/42/report/from-discovery/job-1/status"}"""
         );
      };

      var job = await _api.StartReportGenerationAsync(
         _credentials,
         42,
         new ReportGenerationRequest
         {
            Id = "from-discovery",
            DisableBookmarks = true,
            Parameters = new Dictionary<string, object?>
            {
               ["TaxYearId"] = 1662,
               ["EmployeeNumber"] = "EMP001",
               ["IncludeTerminated"] = false,
               ["Optional"] = null
            }
         },
         frequency
      );

      job.RequestId.Should().Be("job-1");
   }

   [Test]
   public async Task StartRejectsMissingRequestId()
   {
      _handler.Respond = (_, _) => Task.FromResult(Json("{}"));

      await FluentActions
         .Invoking(
            () => _api.StartReportGenerationAsync(_credentials, 42, new ReportGenerationRequest { Id = "report" })
         )
         .Should()
         .ThrowAsync<InvalidOperationException>()
         .WithMessage("*request ID*");
   }

   [Test]
   public async Task StatusEscapesIdentifiers()
   {
      _handler.Respond = (request, _) =>
      {
         request
            .RequestUri!.AbsoluteUri.Should()
            .Be("https://api.payspace.com/v2.0/42/Report/report%2Fid/job%3Fid/Status");

         return Task.FromResult(Json("""{"IsReady":false,"Reason":"Processing"}"""));
      };

      var status = await _api.GetReportGenerationStatusAsync(_credentials, 42, "report/id", "job?id");

      status.IsReady.Should().BeFalse();

      status.Reason.Should().Be("Processing");
   }

   [TestCase(false)]
   [TestCase(true)]
   public async Task UnauthorizedRefreshesOnceIncludingPostBody(bool remainsUnauthorized)
   {
      var attempts = 0;

      _handler.Respond = async (request, _) =>
      {
         request.Headers.Authorization!.Parameter.Should().Be(attempts == 0 ? "original-token" : "refreshed-token");

         (await request.Content!.ReadAsStringAsync()).Should().Contain("\"id\":\"report\"");

         return attempts++ == 0 || remainsUnauthorized
            ? new HttpResponseMessage(HttpStatusCode.Unauthorized)
            : Json("""{"RequestId":"job"}""");
      };

      var action = () =>
         _api.StartReportGenerationAsync(_credentials, 42, new ReportGenerationRequest { Id = "report" });

      if (remainsUnauthorized)
      {
         await action.Should().ThrowAsync<HttpRequestException>();
      }
      else
      {
         (await action()).RequestId.Should().Be("job");
      }

      attempts.Should().Be(2);

      _authentication.Verify(provider => provider.GetAccessToken(_credentials, true), Times.Once);
   }

   [Test]
   public async Task WaitPollsExistingJobUntilReady()
   {
      var attempts = 0;

      _handler.Respond = (_, _) =>
         Task.FromResult(
            Json(
               ++attempts < 3
                  ? """{"IsReady":false,"Reason":"Processing"}"""
                  : """{"IsReady":true,"Reason":"Completed","ReportUrl":"https://storage.example/report.csv"}"""
            )
         );

      var status = await _api.WaitForReportAsync(_credentials, 42, "report", "job", FastPolling());

      status.ReportUrl.Should().Be("https://storage.example/report.csv");

      attempts.Should().Be(3);
   }

   [Test]
   public async Task WaitTimesOutWithLastReason()
   {
      _handler.Respond = (_, _) => Task.FromResult(Json("""{"IsReady":false,"Reason":"Still queued"}"""));

      await FluentActions
         .Invoking(
            () =>
               _api.WaitForReportAsync(_credentials, 42, "report", "job", FastPolling(TimeSpan.FromMilliseconds(100)))
         )
         .Should()
         .ThrowAsync<TimeoutException>()
         .WithMessage("*job*Still queued*");
   }

   [TestCase(false)]
   [TestCase(true)]
   public async Task WaitCancelsAnInFlightStatusRequest(bool callerCancels)
   {
      _handler.Respond = async (_, token) =>
      {
         await Task.Delay(Timeout.InfiniteTimeSpan, token);

         return Json("{}");
      };

      using var cancellation = new CancellationTokenSource();

      if (callerCancels)
      {
         cancellation.CancelAfter(100);
      }

      var action = () =>
         _api.WaitForReportAsync(
            _credentials,
            42,
            "report",
            "job",
            FastPolling(TimeSpan.FromMilliseconds(callerCancels ? 5000 : 100)),
            cancellation.Token
         );

      if (callerCancels)
      {
         await action.Should().ThrowAsync<OperationCanceledException>();
      }
      else
      {
         await action.Should().ThrowAsync<TimeoutException>();
      }
   }

   [Test]
   public async Task ReadyWithoutUrlFailsClearly()
   {
      _handler.Respond = (_, _) => Task.FromResult(Json("""{"IsReady":true,"Reason":"Completed"}"""));

      await FluentActions
         .Invoking(() => _api.WaitForReportAsync(_credentials, 42, "report", "job"))
         .Should()
         .ThrowAsync<InvalidOperationException>()
         .WithMessage("*without a download URL*");
   }

   [Test]
   public async Task ExternalSignedDownloadStreamsBytesWithoutBearer()
   {
      byte[] bytes = [0, 255, 10, 13, 42];

      _handler.Respond = (request, _) =>
      {
         request
            .RequestUri!.AbsoluteUri.Should()
            .Be("https://storage.example/report.pdf?signature=a%2Bb%2Fz&expires=123");

         request.Headers.Authorization.Should().BeNull();

         return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(bytes) });
      };

      var path = await _api.DownloadReportAsync(
         _credentials,
         "https://storage.example/report.pdf?signature=a%2Bb%2Fz&expires=123",
         Path.Combine(_directory, "nested", "report.pdf")
      );

      (await File.ReadAllBytesAsync(path)).Should().Equal(bytes);

      _authentication.Verify(
         provider => provider.GetAccessToken(It.IsAny<AccessTokenRequest>(), It.IsAny<bool>()),
         Times.Never
      );
   }

   [Test]
   public async Task DownloadRedirectsReevaluateOriginAndRefreshSameOriginToken()
   {
      var attempts = 0;

      _handler.Respond = (request, _) =>
      {
         var step = attempts++;

         if (step == 0)
         {
            request.Headers.Authorization!.Parameter.Should().Be("original-token");

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized));
         }

         if (step == 1)
         {
            request.Headers.Authorization!.Parameter.Should().Be("refreshed-token");

            var redirect = new HttpResponseMessage(HttpStatusCode.Found);

            redirect.Headers.Location = new Uri("https://storage.example/report.csv");

            return Task.FromResult(redirect);
         }

         request.Headers.Authorization.Should().BeNull();

         return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("csv") });
      };

      var path = await _api.DownloadReportAsync(
         _credentials,
         "https://api.payspace.com/reports/file",
         Path.Combine(_directory, "report.csv")
      );

      (await File.ReadAllTextAsync(path)).Should().Be("csv");

      attempts.Should().Be(3);
   }

   [TestCase("http://storage.example/report")]
   [TestCase("file:///C:/report.csv")]
   [TestCase("https://user:password@storage.example/report")]
   public async Task RejectsUnsafeDownloadUrls(string url)
   {
      await FluentActions
         .Invoking(() => _api.DownloadReportAsync(_credentials, url, Path.Combine(_directory, "report.csv")))
         .Should()
         .ThrowAsync<ArgumentException>();
   }

   [TestCase(false)]
   [TestCase(true)]
   public async Task RejectsDowngradeOrExcessiveRedirects(bool downgrade)
   {
      _handler.Respond = (_, _) =>
      {
         var response = new HttpResponseMessage(HttpStatusCode.Found);

         response.Headers.Location = new Uri(
            downgrade ? "http://storage.example/report" : "https://storage.example/report"
         );

         return Task.FromResult(response);
      };

      await FluentActions
         .Invoking(
            () =>
               _api.DownloadReportAsync(
                  _credentials,
                  "https://storage.example/report",
                  Path.Combine(_directory, "report.csv")
               )
         )
         .Should()
         .ThrowAsync<Exception>();

      Directory.GetFiles(_directory).Should().BeEmpty();
   }

   [Test]
   public async Task ExpiredExternalDownloadDoesNotSendBearerOrOverwriteExistingFile()
   {
      var path = Path.Combine(_directory, "existing.csv");

      await File.WriteAllTextAsync(path, "original");

      _handler.Respond = (request, _) =>
      {
         request.Headers.Authorization.Should().BeNull();

         return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Forbidden));
      };

      await FluentActions
         .Invoking(() => _api.DownloadReportAsync(_credentials, "https://storage.example/expired", path, true))
         .Should()
         .ThrowAsync<HttpRequestException>();

      (await File.ReadAllTextAsync(path)).Should().Be("original");
   }

   [TestCase(false)]
   [TestCase(true)]
   public async Task InterruptedDownloadCleansPartialFileAndPreservesDestination(bool cancel)
   {
      var path = Path.Combine(_directory, "existing.csv");

      await File.WriteAllTextAsync(path, "original");

      using var cancellation = new CancellationTokenSource();

      _handler.Respond = (_, _) =>
         Task.FromResult(
            new HttpResponseMessage(HttpStatusCode.OK)
            {
               Content = new InterruptedContent(cancel ? cancellation : null)
            }
         );

      await FluentActions
         .Invoking(
            () =>
               _api.DownloadReportAsync(_credentials, "https://storage.example/report", path, true, cancellation.Token)
         )
         .Should()
         .ThrowAsync<Exception>();

      (await File.ReadAllTextAsync(path)).Should().Be("original");

      Directory.GetFiles(_directory).Should().Equal(path);
   }

   [Test]
   public async Task GenerateAndSaveUsesDiscoveryIdAndCompletesWorkflow()
   {
      var methods = new List<HttpMethod>();

      _handler.Respond = (request, _) =>
      {
         methods.Add(request.Method);

         if (request.Method == HttpMethod.Post)
         {
            return Task.FromResult(Json("""{"RequestId":"job"}"""));
         }

         if (request.RequestUri!.Host == "storage.example")
         {
            return Task.FromResult(
               new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("report bytes") }
            );
         }

         request.RequestUri.AbsolutePath.Should().EndWith("/Report/discovered/job/Status");

         return Task.FromResult(Json("""{"IsReady":true,"ReportUrl":"https://storage.example/report"}"""));
      };

      var path = await _api.GenerateAndSaveReportAsync(
         _credentials,
         42,
         new ReportGenerationRequest { Id = "discovered" },
         Path.Combine(_directory, "report.csv")
      );

      (await File.ReadAllTextAsync(path)).Should().Be("report bytes");

      methods.Should().Equal(HttpMethod.Post, HttpMethod.Get, HttpMethod.Get);
   }

   [Test]
   public async Task ExistingDestinationAndInvalidPollingFailBeforeStartingJob()
   {
      var path = Path.Combine(_directory, "existing.csv");

      await File.WriteAllTextAsync(path, "original");

      await FluentActions
         .Invoking(
            () => _api.GenerateAndSaveReportAsync(_credentials, 42, new ReportGenerationRequest { Id = "report" }, path)
         )
         .Should()
         .ThrowAsync<IOException>();

      await FluentActions
         .Invoking(
            () =>
               _api.GenerateAndSaveReportAsync(
                  _credentials,
                  42,
                  new ReportGenerationRequest { Id = "report" },
                  path + ".new",
                  reportPollingOptions: new ReportPollingOptions { InitialInterval = TimeSpan.Zero }
               )
         )
         .Should()
         .ThrowAsync<ArgumentOutOfRangeException>();
   }

   [TestCase("https://api.payspace.com")]
   [TestCase("https://apistaging.payspace.com/")]
   public async Task RegistrationSharesApiPipelineForReadsWritesAndReportsWithoutRetryingPosts(string apiBaseUrl)
   {
      var configuration = new ConfigurationBuilder()
         .AddInMemoryCollection(
            new Dictionary<string, string?>
            {
               ["PaySpaceConfig:ApiBaseUrl"] = apiBaseUrl,
               ["PaySpaceConfig:AuthorizationUrl"] = "https://identity.example/token"
            }
         )
         .Build();

      var services = new ServiceCollection();

      services.AddSingleton<IConfiguration>(configuration);

      services.AddPaySpaceApiServices(configuration);

      services.AddTransient(_ => _authentication.Object);

      services
         .AddHttpClient(PaySpaceHttpClient.PaySpaceRateLimitedApi)
         .ConfigurePrimaryHttpMessageHandler(() => _handler);

      services
         .AddHttpClient(PaySpaceHttpClient.PaySpaceReportDownload)
         .ConfigurePrimaryHttpMessageHandler(() => new ReportHandler());

      using var provider = services.BuildServiceProvider();

      var factory = provider.GetRequiredService<IHttpClientFactory>();

      using var odataClient = factory.CreateClient(PaySpaceHttpClient.PaySpaceRateLimitedApi);

      odataClient.BaseAddress.Should().Be(new Uri(apiBaseUrl));

      using var downloadClient = factory.CreateClient(PaySpaceHttpClient.PaySpaceReportDownload);

      downloadClient.BaseAddress.Should().Be(odataClient.BaseAddress);

      provider.GetRequiredService<IPaySpaceApiContext>().PaySpaceReportApi.Should().NotBeNull();

      var attempts = 0;

      _handler.Respond = (_, _) =>
      {
         attempts++;

         return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
      };

      await FluentActions
         .Invoking(
            () =>
               provider
                  .GetRequiredService<IPaySpaceReportApi>()
                  .StartReportGenerationAsync(_credentials, 42, new ReportGenerationRequest { Id = "report" })
         )
         .Should()
         .ThrowAsync<HttpRequestException>();

      attempts.Should().Be(1);

      await FluentActions
         .Invoking(
            () =>
               provider
                  .GetRequiredService<IPaySpaceEmployeeApi>()
                  .CreateEmployeeAsync(_credentials, 42, new CreateEmployeeRequest())
         )
         .Should()
         .ThrowAsync<HttpRequestException>();

      attempts.Should().Be(2);

      _handler.Respond = (request, _) =>
      {
         request.Method.Should().Be(HttpMethod.Get);

         request.Headers.Authorization!.Parameter.Should().Be("original-token");

         return Task.FromResult(
            request.RequestUri!.AbsolutePath.EndsWith("/Employee")
               ? Json("""{"value":[{"EmployeeId":42}]}""")
               : Json("[]")
         );
      };

      var employees = await provider.GetRequiredService<IPaySpaceEmployeeApi>().EmployeeListAsync(_credentials, 42);

      employees.Should().ContainSingle().Which.EmployeeId.Should().Be(42);

      var reports = await provider.GetRequiredService<IPaySpaceReportApi>().GetReportsAsync(_credentials, 42);

      reports.Should().BeEmpty();

      var path = Path.Combine(_directory, "shared-pipeline.csv");

      await provider
         .GetRequiredService<IPaySpaceReportApi>()
         .DownloadReportAsync(_credentials, new Uri(odataClient.BaseAddress!, "report.csv").AbsoluteUri, path);

      File.ReadAllText(path).Should().Be("[]");
   }

   private static HttpResponseMessage Json(string json) =>
      new(HttpStatusCode.OK) { Content = new StringContent(json, Encoding.UTF8, "application/json") };

   private static ReportPollingOptions FastPolling(TimeSpan? timeout = null) =>
      new()
      {
         InitialInterval = TimeSpan.FromMilliseconds(1),
         MaximumInterval = TimeSpan.FromMilliseconds(5),
         Timeout = timeout ?? TimeSpan.FromSeconds(2)
      };

   private sealed class ReportHandler : HttpMessageHandler
   {
      public Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> Respond { get; set; } =
         (_, _) => throw new AssertionException("Unexpected HTTP request.");

      protected override Task<HttpResponseMessage> SendAsync(
         HttpRequestMessage request,
         CancellationToken cancellationToken
      ) => Respond(request, cancellationToken);
   }

   private sealed class InterruptedContent(CancellationTokenSource? cancellation) : HttpContent
   {
      protected override async Task SerializeToStreamAsync(Stream stream, TransportContext? context)
      {
         await stream.WriteAsync(new byte[] { 1, 2, 3 });

         if (cancellation != null)
         {
            cancellation.Cancel();

            cancellation.Token.ThrowIfCancellationRequested();
         }

         throw new IOException("Connection interrupted.");
      }

      protected override bool TryComputeLength(out long length)
      {
         length = 0;

         return false;
      }
   }
}
