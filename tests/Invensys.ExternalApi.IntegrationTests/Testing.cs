using Ardalis.GuardClauses;
using Invensys.ExternalApi.IntegrationTests.Payspace;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.Common.Authentication.Models.Result;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Invensys.ExternalApi.Sage300.Core.Models.Request;
using Invensys.ExternalApi.Sage300.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Invensys.ExternalApi.IntegrationTests;

[SetUpFixture]
public partial class Testing
{
   private static CustomWebApplicationFactory s_factory = null!;
   private static IServiceScopeFactory s_scopeFactory = null!;

   [OneTimeSetUp]
   public async Task RunBeforeAnyTests()
   {
      s_factory = new CustomWebApplicationFactory();

      s_scopeFactory = s_factory.Services.GetRequiredService<IServiceScopeFactory>();

      await Task.CompletedTask.ConfigureAwait(false);
   }

   public static PaySpaceTestClientsConfig GetPaySpaceTestClientConfig()
   {
      var scope = s_factory.Services.CreateScope();

      var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

      var paySpaceTestClientConfig = config
         .GetSection(nameof(PaySpaceTestClientsConfig))
         .Get<PaySpaceTestClientsConfig>();

      Guard.Against.Null(paySpaceTestClientConfig, nameof(paySpaceTestClientConfig));

      return paySpaceTestClientConfig;
   }

   public static JwtAccessTokenRequest GetPaySpaceJwtAccessTokenRequest(PaySpaceTestClient paySpaceTestClient)
   {
      return new JwtAccessTokenRequest(
         paySpaceTestClient.ClientId!,
         paySpaceTestClient.ClientSecret!,
         paySpaceTestClient.Scope!
      );
   }

   public static async Task<(JwtAccessTokenRequest Token, long[] CompanyIds)> GetPaySpaceAuthTokenResponse()
   {
      using var scope = s_scopeFactory.CreateScope();

      var payspaceClient = scope.ServiceProvider.GetRequiredService<IPaySpaceAuthenticationProvider>();

      var paySpaceTestClient = GetPaySpaceTestClientConfig()
         ?.PaySpaceTestClients
         ?.First()!;

      var accessTokenRequest = GetPaySpaceJwtAccessTokenRequest(paySpaceTestClient);

      var authenticationResult = await payspaceClient.GetAuthenticationResult(accessTokenRequest);

      var accessToken = await payspaceClient.GetAccessToken(accessTokenRequest);

      return (
         accessTokenRequest,
         authenticationResult
            .AuthenticationResultData!.Select(s => s.Companies)
            .First()!
            .Select(s => s.CompanyId)
            .ToArray()
      );
   }

   public static async Task<AuthenticationResult> GetSage300AuthTokenResponse(ResourceOwnerPasswordCredentialTokenRequest accessTokenRequest)
   {
      using var scope = s_scopeFactory.CreateScope(); //why is s_scopeFactory null here?

      var sage300Client = scope.ServiceProvider.GetRequiredService<ISage300AuthenticationProvider>();

      var authenticationResult = await sage300Client.GetAuthenticationResult(accessTokenRequest);

      var accessToken = await sage300Client.GetAccessToken(accessTokenRequest);

      return authenticationResult;
   }

   public static ISage300ApiClient ISage300ApiClient()
   {
      using var scope = s_scopeFactory.CreateScope();

      return scope.ServiceProvider.GetRequiredService<ISage300ApiClient>();
   }

   public static IPaySpaceEmployeeApi IPaySpaceEmployeeApi()
   {
      using var scope = s_scopeFactory.CreateScope();

      return scope.ServiceProvider.GetRequiredService<IPaySpaceEmployeeApi>();
   }

   public static IPaySpacePayslipApi IPaySpacePayslipApi()
   {
      using var scope = s_scopeFactory.CreateScope();

      return scope.ServiceProvider.GetRequiredService<IPaySpacePayslipApi>();
   }

   public static IPaySpacePayrollProcessingApi IPaySpacePayrollProcessingApi()
   {
      using var scope = s_scopeFactory.CreateScope();

      return scope.ServiceProvider.GetRequiredService<IPaySpacePayrollProcessingApi>();
   }

   public static IPaySpaceLookupApi IPaySpaceLookupApi()
   {
      using var scope = s_scopeFactory.CreateScope();

      return scope.ServiceProvider.GetRequiredService<IPaySpaceLookupApi>();
   }

   public static IPaySpaceCompanyApi IPaySpaceCompanyApi()
   {
      using var scope = s_scopeFactory.CreateScope();

      return scope.ServiceProvider.GetRequiredService<IPaySpaceCompanyApi>();
   }

   public static async Task ResetState()
   {
      await Task.CompletedTask.ConfigureAwait(false);
   }

   [OneTimeTearDown]
   public async Task RunAfterAnyTests()
   {
      await s_factory.DisposeAsync();
   }
}
