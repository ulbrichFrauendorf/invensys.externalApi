using System.Net;
using System.Reflection;
using Invensys.ExternalApi.Sage300.Core;
using Invensys.ExternalApi.Sage300.Core.Authentication;
using Invensys.ExternalApi.Sage300.Core.Configuration;
using Invensys.ExternalApi.Sage300.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Invensys.ExternalApi.Sage300;

public static class DependencyInjection
{
   public static IServiceCollection AddSage300ApiServices(this IServiceCollection services)
   {
      services
         .AddHttpClient(Sage300HttpClient.Sage300RateLimitedApi)
         .AddPolicyHandler(RateLimitPolicy)
         .AddHttpMessageHandler(() => new RateLimitingHandler(10, 100));

      services.AddAutoMapper(Assembly.GetExecutingAssembly());

      //Core
      services.AddTransient<ISage300AuthenticationProvider, Sage300AuthenticationProvider>(); //Implements IAuthenticationProvider<GroupCompany[]> From Api common
      services.AddTransient<ISage300ApiClient, Sage300ApiClient>();

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
