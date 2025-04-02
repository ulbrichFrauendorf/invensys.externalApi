using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Authentication.Models;

namespace Invensys.ExternalApi.PaySpace.Core.Authentication;

public interface IPaySpaceApiAuthenticationService
{
    Task<PaySpaceAuthorizationResponse> Authenticate(JwtAccessTokenRequest accessTokenRequest);
}
