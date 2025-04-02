using Ardalis.GuardClauses;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Authentication.Models;
using Invensys.ExternalApi.PaySpace.Interfaces;

namespace Invensys.ExternalApi.PaySpace.Core.Authentication;

public class PaySpaceApiAuthenticationService(
   IPaySpaceAuthenticationProvider payspaceAuthenticationProvider,
   IPaySpaceLookupApi payspaceLookupApi
) : IPaySpaceApiAuthenticationService
{
    public async Task<PaySpaceAuthorizationResponse> Authenticate(JwtAccessTokenRequest accessTokenRequest)
    {
        Guard.Against.Null(accessTokenRequest, nameof(accessTokenRequest));

        var authenticationResult = await payspaceAuthenticationProvider.GetAuthenticationResult(accessTokenRequest);

        var accessToken = await payspaceAuthenticationProvider.GetAccessToken(accessTokenRequest);

        var groups = authenticationResult.AuthenticationResultData;

        Guard.Against.Null(groups, nameof(groups));

        var companyId = groups.First().Companies?.First().CompanyId;

        Guard.Against.Null(companyId, nameof(companyId));

        var orgUnitSelections = await payspaceLookupApi.GetOrganizationLevelsAsync(accessTokenRequest, companyId.Value);

        var companies = groups.Where(gc => gc.Companies != null).SelectMany(gc => gc.Companies!).ToList();

        return new PaySpaceAuthorizationResponse { AccessToken = accessToken, Companies = companies, AccessTokenRequest = accessTokenRequest };
    }
}
