using System.Net;
using Invensys.ExternalApi.Common.Http.Handlers;
using Invensys.ExternalApi.Iserve.Core;
using Invensys.ExternalApi.Iserve.Core.Authentication;
using Invensys.ExternalApi.Iserve.Core.Configuration;
using Invensys.ExternalApi.Iserve.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Invensys.ExternalApi.Iserve;

public static class DependencyInjection
{
   public static IServiceCollection AddIserveApiServices(this IServiceCollection services)
   {
      services
         .AddHttpClient(IserveHttpClient.IserveRateLimitedApi)
         .AddPolicyHandler(RateLimitPolicy)
         .AddHttpMessageHandler(() => new RateLimitingHandler(10, 100));

      //Core
      services.AddTransient<IIserveAuthenticationProvider, IserveAuthenticationProvider>();
      services.AddTransient<IIserveApiClient, IserveApiClient>();

      return services;
   }

   private static IAsyncPolicy<HttpResponseMessage> RateLimitPolicy
   {
      get
      {
         var jitterer = new Random();

         // Retry policy for transient errors and rate-limiting
         var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests) // Handle 429 Too Many Requests
            .WaitAndRetryAsync(
               retryCount: 5,
               sleepDurationProvider: retryAttempt =>
                  TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 1000))
            );

         return retryPolicy;
      }
   }
}
