using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Entities.Company;

namespace Invensys.ExternalApi.PaySpace.Core.Authentication.Models;

public class PaySpaceAuthorizationResponse
{
   public string AccessToken { get; set; } = null!;
   public JwtAccessTokenRequest AccessTokenRequest { get; set; } = null!;
   public List<Company> Companies { get; set; } = [];
}
