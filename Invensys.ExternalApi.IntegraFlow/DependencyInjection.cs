using System.Net;
using Invensys.ExternalApi.Common.Http.Handlers;
using Invensys.ExternalApi.IntegraFlow.Core;
using Invensys.ExternalApi.IntegraFlow.Core.Authentication;
using Invensys.ExternalApi.IntegraFlow.Core.Configuration;
using Invensys.ExternalApi.IntegraFlow.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Invensys.ExternalApi.IntegraFlow;

public static class DependencyInjection
{
   public static IServiceCollection AddIntegraFlowApiServices(this IServiceCollection services)
   {
      services
         .AddHttpClient(IntegraFlowHttpClient.IntegraFlowRateLimitedApi)
         .AddPolicyHandler(RateLimitPolicy)
         .AddHttpMessageHandler(() => new RateLimitingHandler(10, 100));

      //Core
      services.AddTransient<IIntegraFlowAuthenticationProvider, IntegraFlowAuthenticationProvider>();
      services.AddTransient<IIntegraFlowApiClient, IntegraFlowApiClient>();

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
