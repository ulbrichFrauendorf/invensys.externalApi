using System.Globalization;
using System.Text.Json;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.IntegrationTests.Payspace;
using Invensys.ExternalApi.PaySpace.Core.Configuration;
using Invensys.ExternalApi.PaySpace.Entities.Reports;
using Invensys.ExternalApi.PaySpace.Interfaces.V2_0;
using static Invensys.ExternalApi.IntegrationTests.Testing;

namespace Invensys.ExternalApi.IntegrationTests.PaySpace;

[Category("PaySpaceReportsLive")]
[Explicit("Calls live PaySpace reporting APIs. Configure credentials and select this fixture explicitly.")]
internal class PaySpaceReportApiTest : BaseTestFixture
{
   [Test]
   public async Task ShouldListAvailableReports()
   {
      var (credentials, companyId) = GetReportClient();

      using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(1));

      var reports = await IPaySpaceReportApi().GetReportsAsync(credentials, companyId, cancellation.Token);

      reports.Should().NotBeNull();

      reports.Should().OnlyContain(report => !string.IsNullOrWhiteSpace(report.Id));

      foreach (var report in reports)
      {
         TestContext.Progress.WriteLine($"{report.Id}: {report.Name} ({report.Format}, {report.Country})");
      }
   }

   [Test]
   public async Task ShouldRetrieveReportParameters()
   {
      var (credentials, companyId) = GetReportClient();

      var configuration = GetPaySpaceReportTestConfig();

      using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(1));

      var report = await GetSelectedReport(credentials, companyId, configuration.ReportId, cancellation.Token);

      var parameters = await IPaySpaceReportApi()
         .GetReportParametersAsync(credentials, companyId, report.Id, cancellation.Token);

      parameters.ValueKind.Should().NotBe(JsonValueKind.Undefined);

      parameters.ValueKind.Should().NotBe(JsonValueKind.Null);

      var metadataPath = Path.Combine(GetOutputDirectory(configuration), $"report-parameters-{Guid.NewGuid():N}.json");

      await File.WriteAllTextAsync(metadataPath, parameters.GetRawText(), cancellation.Token);

      TestContext.Progress.WriteLine($"Parameter metadata saved locally to {metadataPath}");
   }

   [Test]
   public async Task ShouldGeneratePollAndSaveReport()
   {
      var (credentials, companyId) = GetReportClient();

      var configuration = GetPaySpaceReportTestConfig();

      if (string.IsNullOrWhiteSpace(configuration.ReportId))
      {
         Assert.Ignore(
            "Set PaySpaceReportTestConfig:ReportId to an ID returned by ShouldListAvailableReports before generating a report."
         );
      }

      configuration.TimeoutSeconds.Should().BeInRange(1, 86400);

      using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(configuration.TimeoutSeconds + 120));

      var api = IPaySpaceReportApi();

      var report = await GetSelectedReport(credentials, companyId, configuration.ReportId, cancellation.Token);

      var parameters =
         JsonSerializer.Deserialize<Dictionary<string, object?>>(configuration.ParametersJson)
         ?? throw new InvalidOperationException("Configure ParametersJson as a JSON object.");

      var job = await api.StartReportGenerationAsync(
         credentials,
         companyId,
         new ReportGenerationRequest
         {
            Id = report.Id,
            DisableBookmarks = configuration.DisableBookmarks,
            Parameters = parameters
         },
         configuration.Frequency,
         cancellation.Token
      );

      job.RequestId.Should().NotBeNullOrWhiteSpace();

      TestContext.Progress.WriteLine(
         $"Started report {report.Id}, request {job.RequestId}. Use this request ID to resume status checks if the test is interrupted."
      );

      var initialStatus = await api.GetReportGenerationStatusAsync(
         credentials,
         companyId,
         report.Id,
         job.RequestId,
         cancellation.Token
      );

      var status = initialStatus.IsReady
         ? initialStatus
         : await api.WaitForReportAsync(
            credentials,
            companyId,
            report.Id,
            job.RequestId,
            new ReportPollingOptions { Timeout = TimeSpan.FromSeconds(configuration.TimeoutSeconds) },
            cancellation.Token
         );

      status.IsReady.Should().BeTrue();

      status.ReportUrl.Should().NotBeNullOrWhiteSpace();

      var extension = report.Format?.ToLowerInvariant() switch
      {
         "pdf" => ".pdf",
         "csv" => ".csv",
         "xlsx" => ".xlsx",
         "txt" => ".txt",
         "zip" => ".zip",
         _ => ".bin"
      };

      var destination = Path.Combine(
         GetOutputDirectory(configuration),
         $"payspace-report-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}{extension}"
      );

      var path = await api.DownloadReportAsync(
         credentials,
         status.ReportUrl!,
         destination,
         cancellationToken: cancellation.Token
      );

      File.Exists(path).Should().BeTrue();

      new FileInfo(path).Length.Should().BeGreaterThan(0);

      TestContext.Progress.WriteLine($"Report saved locally to {path}");
   }

   private static (JwtAccessTokenRequest accessTokenRequest, long CompanyId) GetReportClient()
   {
      var client = GetPaySpaceTestClientConfig().PaySpaceTestClients?.FirstOrDefault();

      if (
         client == null
         || string.IsNullOrWhiteSpace(client.ClientId)
         || string.IsNullOrWhiteSpace(client.ClientSecret)
         || string.IsNullOrWhiteSpace(client.Scope)
      )
      {
         Assert.Ignore(
            "Configure the first PaySpaceTestClientsConfig:PaySpaceTestClients entry with ClientId, ClientSecret, Scope and CompanyId using environment variables or local configuration."
         );
      }

      if (!long.TryParse(client!.CompanyId, CultureInfo.InvariantCulture, out var companyId) || companyId <= 0)
      {
         Assert.Ignore("Configure a positive CompanyId on the selected PaySpace test client.");
      }

      return (GetPaySpaceJwtAccessTokenRequest(client), companyId);
   }

   private static async Task<ReportDefinition> GetSelectedReport(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      string? reportId,
      CancellationToken cancellationToken
   )
   {
      var reports = await IPaySpaceReportApi().GetReportsAsync(accessTokenRequest, companyId, cancellationToken);

      if (reports.Count == 0)
      {
         Assert.Ignore("No reports are available for the configured company and API permissions.");
      }

      if (string.IsNullOrWhiteSpace(reportId))
      {
         return reports[0];
      }

      return reports.SingleOrDefault(report => report.Id == reportId)
         ?? throw new InvalidOperationException(
            "The configured ReportId was not returned by PaySpace for the selected company."
         );
   }

   private static string GetOutputDirectory(PaySpaceReportTestConfig paySpaceReportTestConfig)
   {
      var directory = Path.GetFullPath(
         paySpaceReportTestConfig.OutputDirectory ?? Path.Combine(Path.GetTempPath(), "Invensys.PaySpaceReports")
      );

      Directory.CreateDirectory(directory);

      return directory;
   }
}
