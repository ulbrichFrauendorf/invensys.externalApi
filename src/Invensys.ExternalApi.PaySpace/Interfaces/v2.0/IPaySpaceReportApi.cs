using System.Text.Json;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Configuration;
using Invensys.ExternalApi.PaySpace.Entities.Reports;

namespace Invensys.ExternalApi.PaySpace.Interfaces.V2_0;

public interface IPaySpaceReportApi
{
   /// <summary>Lists the reports available to the company (a plain array, not an OData envelope).</summary>
   Task<List<ReportDefinition>> GetReportsAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      CancellationToken cancellationToken = default
   );

   /// <summary>Returns report-specific metadata unchanged; the vendor does not publish a fixed parameter schema.</summary>
   Task<JsonElement> GetReportParametersAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      string reportId,
      CancellationToken cancellationToken = default
   );

   /// <summary>Starts a job. Persist its RequestId to resume polling without generating another report.</summary>
   Task<ReportGenerationResponse> StartReportGenerationAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      ReportGenerationRequest reportGenerationRequest,
      string? frequency = null,
      CancellationToken cancellationToken = default
   );

   Task<ReportGenerationStatus> GetReportGenerationStatusAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      string reportId,
      string requestId,
      CancellationToken cancellationToken = default
   );

   /// <summary>Polls an existing job with bounded exponential backoff. Throws TimeoutException on expiry.</summary>
   Task<ReportGenerationStatus> WaitForReportAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      string reportId,
      string requestId,
      ReportPollingOptions? reportPollingOptions = null,
      CancellationToken cancellationToken = default
   );

   /// <summary>
   /// Streams an HTTPS report URL to a caller-selected path and returns the full path.
   /// Sends bearer credentials only to the reporting API origin, including across redirects.
   /// Uses a temporary file so failed downloads leave an existing destination intact.
   /// </summary>
   Task<string> DownloadReportAsync(
      JwtAccessTokenRequest accessTokenRequest,
      string reportUrl,
      string destinationPath,
      bool overwrite = false,
      CancellationToken cancellationToken = default
   );

   /// <summary>
   /// Generates, waits for, and saves a report. The timeout covers polling; cancellation covers all stages.
   /// Durable automation should use the separate start/status/download operations and persist the RequestId.
   /// </summary>
   Task<string> GenerateAndSaveReportAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      ReportGenerationRequest reportGenerationRequest,
      string destinationPath,
      string? frequency = null,
      ReportPollingOptions? reportPollingOptions = null,
      bool overwrite = false,
      CancellationToken cancellationToken = default
   );
}
