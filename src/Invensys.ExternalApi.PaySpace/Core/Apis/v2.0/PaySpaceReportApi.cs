using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Ardalis.GuardClauses;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Attributes;
using Invensys.ExternalApi.PaySpace.Core.Configuration;
using Invensys.ExternalApi.PaySpace.Entities.Reports;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Invensys.ExternalApi.PaySpace.Interfaces.V2_0;

namespace Invensys.ExternalApi.PaySpace.Core.Apis.V2_0;

[PaySpaceApi("v2.0")]
public class PaySpaceReportApi(
   IHttpClientFactory httpClientFactory,
   IPaySpaceAuthenticationProvider paySpaceAuthenticationProvider
) : IPaySpaceReportApi
{
   public Task<List<ReportDefinition>> GetReportsAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      CancellationToken cancellationToken = default
   ) =>
      SendJsonAsync<List<ReportDefinition>>(
         accessTokenRequest,
         HttpMethod.Get,
         ReportPath(companyId),
         null,
         cancellationToken
      );

   public Task<JsonElement> GetReportParametersAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      string reportId,
      CancellationToken cancellationToken = default
   ) =>
      SendJsonAsync<JsonElement>(
         accessTokenRequest,
         HttpMethod.Get,
         $"{ReportPath(companyId)}/parameters?reportId={EscapeIdentifier(reportId)}",
         null,
         cancellationToken
      );

   public async Task<ReportGenerationResponse> StartReportGenerationAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      ReportGenerationRequest reportGenerationRequest,
      string? frequency = null,
      CancellationToken cancellationToken = default
   )
   {
      Guard.Against.Null(reportGenerationRequest, nameof(reportGenerationRequest));

      Guard.Against.NullOrWhiteSpace(reportGenerationRequest.Id, nameof(reportGenerationRequest.Id));

      var path = ReportPath(companyId);

      if (!string.IsNullOrWhiteSpace(frequency))
      {
         path += $"?frequency={Uri.EscapeDataString(frequency)}";
      }

      var response = await SendJsonAsync<ReportGenerationResponse>(
         accessTokenRequest,
         HttpMethod.Post,
         path,
         reportGenerationRequest,
         cancellationToken
      );

      if (string.IsNullOrWhiteSpace(response.RequestId))
      {
         throw new InvalidOperationException("PaySpace did not return a report request ID.");
      }

      return response;
   }

   public Task<ReportGenerationStatus> GetReportGenerationStatusAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      string reportId,
      string requestId,
      CancellationToken cancellationToken = default
   ) =>
      SendJsonAsync<ReportGenerationStatus>(
         accessTokenRequest,
         HttpMethod.Get,
         $"{ReportPath(companyId)}/{EscapeIdentifier(reportId)}/{EscapeIdentifier(requestId)}/Status",
         null,
         cancellationToken
      );

   public async Task<ReportGenerationStatus> WaitForReportAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      string reportId,
      string requestId,
      ReportPollingOptions? reportPollingOptions = null,
      CancellationToken cancellationToken = default
   )
   {
      var options = reportPollingOptions ?? new ReportPollingOptions();

      ValidatePollingOptions(options);

      using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

      timeout.CancelAfter(options.Timeout);

      var interval = options.InitialInterval;

      string? lastReason = null;

      try
      {
         while (true)
         {
            var status = await GetReportGenerationStatusAsync(
               accessTokenRequest,
               companyId,
               reportId,
               requestId,
               timeout.Token
            );

            lastReason = status.Reason;

            if (status.IsReady)
            {
               if (string.IsNullOrWhiteSpace(status.ReportUrl))
               {
                  throw new InvalidOperationException(
                     $"PaySpace marked report request {requestId} ready without a download URL. Reason: {status.Reason}"
                  );
               }

               return status;
            }

            await Task.Delay(interval, timeout.Token);

            interval = TimeSpan.FromMilliseconds(
               Math.Min(interval.TotalMilliseconds * 2, options.MaximumInterval.TotalMilliseconds)
            );
         }
      }
      catch (OperationCanceledException exception)
         when (!cancellationToken.IsCancellationRequested && timeout.IsCancellationRequested)
      {
         throw new TimeoutException(
            $"Timed out waiting for PaySpace report request {requestId}. Last reason: {lastReason}",
            exception
         );
      }
   }

   public async Task<string> DownloadReportAsync(
      JwtAccessTokenRequest accessTokenRequest,
      string reportUrl,
      string destinationPath,
      bool overwrite = false,
      CancellationToken cancellationToken = default
   )
   {
      var uri = ValidateDownloadUri(reportUrl);

      var fullPath = ValidateDestinationPath(destinationPath, overwrite);

      using var client = httpClientFactory.CreateClient(PaySpaceHttpClient.PaySpaceReportDownload);

      using var apiClient = httpClientFactory.CreateClient(PaySpaceHttpClient.PaySpaceRateLimitedApi);

      using var response = await SendDownloadAsync(client, apiClient, accessTokenRequest, uri, cancellationToken);

      response.EnsureSuccessStatusCode();

      Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

      var temporaryPath = Path.Combine(Path.GetDirectoryName(fullPath)!, $".{Guid.NewGuid():N}.part");

      try
      {
         await using (
            var file = new FileStream(
               temporaryPath,
               FileMode.CreateNew,
               FileAccess.Write,
               FileShare.None,
               81920,
               FileOptions.Asynchronous
            )
         )
         {
            await response.Content.CopyToAsync(file, cancellationToken);
         }

         cancellationToken.ThrowIfCancellationRequested();

         File.Move(temporaryPath, fullPath, overwrite);

         return fullPath;
      }
      finally
      {
         File.Delete(temporaryPath);
      }
   }

   public async Task<string> GenerateAndSaveReportAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      ReportGenerationRequest reportGenerationRequest,
      string destinationPath,
      string? frequency = null,
      ReportPollingOptions? reportPollingOptions = null,
      bool overwrite = false,
      CancellationToken cancellationToken = default
   )
   {
      var fullPath = ValidateDestinationPath(destinationPath, overwrite);

      ValidatePollingOptions(reportPollingOptions ?? new ReportPollingOptions());

      var job = await StartReportGenerationAsync(
         accessTokenRequest,
         companyId,
         reportGenerationRequest,
         frequency,
         cancellationToken
      );

      var status = await WaitForReportAsync(
         accessTokenRequest,
         companyId,
         reportGenerationRequest.Id,
         job.RequestId,
         reportPollingOptions,
         cancellationToken
      );

      return await DownloadReportAsync(accessTokenRequest, status.ReportUrl!, fullPath, overwrite, cancellationToken);
   }

   private async Task<T> SendJsonAsync<T>(
      JwtAccessTokenRequest accessTokenRequest,
      HttpMethod method,
      string path,
      ReportGenerationRequest? reportGenerationRequest,
      CancellationToken cancellationToken
   )
   {
      using var client = httpClientFactory.CreateClient(PaySpaceHttpClient.PaySpaceRateLimitedApi);

      using var response = await SendAuthenticatedAsync(
         client,
         accessTokenRequest,
         method,
         new Uri(client.BaseAddress!, PaySpaceApiAttribute.BuildPath(GetType(), path)),
         reportGenerationRequest,
         cancellationToken
      );

      response.EnsureSuccessStatusCode();

      return await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken)
         ?? throw new InvalidOperationException("PaySpace returned an empty report response.");
   }

   private async Task<HttpResponseMessage> SendAuthenticatedAsync(
      HttpClient client,
      JwtAccessTokenRequest accessTokenRequest,
      HttpMethod method,
      Uri uri,
      ReportGenerationRequest? reportGenerationRequest,
      CancellationToken cancellationToken
   )
   {
      for (var attempt = 0; ; attempt++)
      {
         cancellationToken.ThrowIfCancellationRequested();

         var token = await paySpaceAuthenticationProvider
            .GetAccessToken(accessTokenRequest, forceRefresh: attempt > 0)
            .WaitAsync(cancellationToken);

         using var request = new HttpRequestMessage(method, uri);

         request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

         request.Headers.UserAgent.ParseAdd("payspace.com");

         if (reportGenerationRequest != null)
         {
            request.Content = JsonContent.Create(reportGenerationRequest);
         }

         var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

         if (response.StatusCode != HttpStatusCode.Unauthorized || attempt > 0)
         {
            return response;
         }

         response.Dispose();
      }
   }

   private async Task<HttpResponseMessage> SendDownloadAsync(
      HttpClient client,
      HttpClient apiClient,
      JwtAccessTokenRequest accessTokenRequest,
      Uri uri,
      CancellationToken cancellationToken
   )
   {
      for (var redirects = 0; ; redirects++)
      {
         HttpResponseMessage response;

         if (
            uri.GetLeftPart(UriPartial.Authority)
               .Equals(apiClient.BaseAddress!.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase)
         )
         {
            response = await SendAuthenticatedAsync(
               apiClient,
               accessTokenRequest,
               HttpMethod.Get,
               uri,
               null,
               cancellationToken
            );
         }
         else
         {
            using var request = new HttpRequestMessage(HttpMethod.Get, uri);

            response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
         }

         if (
            response.StatusCode
            is not (
               HttpStatusCode.MovedPermanently
               or HttpStatusCode.Found
               or HttpStatusCode.SeeOther
               or HttpStatusCode.TemporaryRedirect
               or HttpStatusCode.PermanentRedirect
            )
         )
         {
            return response;
         }

         using (response)
         {
            if (redirects >= 5 || response.Headers.Location == null)
            {
               throw new HttpRequestException("PaySpace report download returned an invalid or excessive redirect.");
            }

            uri = ValidateDownloadUri(new Uri(uri, response.Headers.Location).AbsoluteUri);
         }
      }
   }

   private static string ReportPath(long companyId)
   {
      Guard.Against.OutOfRange(companyId, nameof(companyId), 1L, long.MaxValue);

      return $"{companyId}/Report";
   }

   private static string EscapeIdentifier(string value)
   {
      Guard.Against.NullOrWhiteSpace(value, nameof(value));

      Guard.Against.InvalidInput(
         value,
         nameof(value),
         identifier => identifier is not ("." or ".."),
         "A report identifier cannot be a relative path segment."
      );

      return Uri.EscapeDataString(value);
   }

   private static Uri ValidateDownloadUri(string reportUrl)
   {
      Guard.Against.InvalidInput(
         reportUrl,
         nameof(reportUrl),
         url =>
            Uri.TryCreate(url, UriKind.Absolute, out var uri)
            && uri.Scheme == Uri.UriSchemeHttps
            && string.IsNullOrEmpty(uri.UserInfo),
         "The report download URL must be an absolute HTTPS URL without user information."
      );

      return new Uri(reportUrl);
   }

   private static string ValidateDestinationPath(string destinationPath, bool overwrite)
   {
      Guard.Against.NullOrWhiteSpace(destinationPath, nameof(destinationPath));

      var fullPath = Path.GetFullPath(destinationPath);

      Guard.Against.InvalidInput(
         fullPath,
         nameof(destinationPath),
         path => !Path.EndsInDirectorySeparator(path) && !Directory.Exists(path),
         "Select a report file path, including its filename."
      );

      if (!overwrite && File.Exists(fullPath))
      {
         throw new IOException("The destination report file already exists.");
      }

      return fullPath;
   }

   private static void ValidatePollingOptions(ReportPollingOptions reportPollingOptions)
   {
      const string message =
         "Use positive, finite polling durations with maximum interval at least the initial interval.";

      Guard.Against.OutOfRange(
         reportPollingOptions.Timeout.TotalMilliseconds,
         nameof(reportPollingOptions),
         double.Epsilon,
         uint.MaxValue - 1d,
         message
      );

      Guard.Against.OutOfRange(
         reportPollingOptions.InitialInterval.TotalMilliseconds,
         nameof(reportPollingOptions),
         double.Epsilon,
         uint.MaxValue - 1d,
         message
      );

      Guard.Against.OutOfRange(
         reportPollingOptions.MaximumInterval.TotalMilliseconds,
         nameof(reportPollingOptions),
         reportPollingOptions.InitialInterval.TotalMilliseconds,
         uint.MaxValue - 1d,
         message
      );
   }
}
