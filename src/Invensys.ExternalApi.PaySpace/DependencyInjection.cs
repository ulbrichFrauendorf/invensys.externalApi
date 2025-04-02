using System.Net;
using System.Reflection;
using Ardalis.GuardClauses;
using AutoMapper;
using Invensys.ExternalApi.PaySpace.Core;
using Invensys.ExternalApi.PaySpace.Core.Apis;
using Invensys.ExternalApi.PaySpace.Core.Authentication;
using Invensys.ExternalApi.PaySpace.Core.Configuration;
using Invensys.ExternalApi.PaySpace.Core.Context;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Invensys.ExternalApi.PaySpace;

public static class DependencyInjection
{
    public static IServiceCollection AddPaySpaceApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        var paySpaceConfig = configuration.GetSection(nameof(PaySpaceConfig)).Get<PaySpaceConfig>();

        Guard.Against.Null(paySpaceConfig, nameof(paySpaceConfig));

        services.AddHttpClient(PaySpaceHttpClient.PaySpaceRateLimitedApi, client => client.BaseAddress = new Uri(paySpaceConfig.ApiBaseUrl))
           .AddPolicyHandler(RateLimitPolicy)
           .AddHttpMessageHandler(() => new RateLimitingHandler(50, 30));

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        //Core
        services.AddTransient<IPaySpaceAuthenticationProvider, PaySpaceAuthenticationProvider>(); //Implements IAuthenticationProvider<GroupCompany[]> From Api common
        services.AddTransient<IPaySpaceApiClient, PaySpaceApiClient>();
        services.AddTransient<IPaySpaceCompanyApi, PaySpaceCompanyApi>();
        services.AddTransient<IPaySpaceEmployeeApi, PaySpaceEmployeeApi>();
        services.AddTransient<IPaySpaceLookupApi, PaySpaceLookupApi>();
        services.AddTransient<IPaySpacePayslipApi, PaySpacePayslipApi>();
        services.AddTransient<IPaySpaceLeaveApi, PaySpaceLeaveApi>();
        services.AddTransient<IPaySpaceIncidentApi, PaySpaceIncidentApi>();
        services.AddTransient<IPaySpaceLoanApi, PaySpaceLoanApi>();
        services.AddTransient<IPaySpacePayrollProcessingApi, PaySpacePayrollProcessingApi>();
        services.AddTransient<IPaySpaceApiContext, PaySpaceApiContext>();
        services.AddTransient<IPaySpaceApiAuthenticationService, PaySpaceApiAuthenticationService>();

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
                .OrResult(msg => msg.StatusCode == HttpStatusCode.BadRequest)
                .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests) // Handle 429 Too Many Requests
                .WaitAndRetryAsync(
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)));

            return retryPolicy;
        }
    }
}
