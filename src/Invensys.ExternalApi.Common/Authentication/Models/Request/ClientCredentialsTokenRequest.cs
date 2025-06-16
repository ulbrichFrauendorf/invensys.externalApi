using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.Common.Authentication.Models.Request;

public class ClientCredentialsTokenRequest(string clientId, string clientSecret, string scope) 
   : JwtAccessTokenRequest(clientId, clientSecret, scope)
{
   [JsonPropertyName("grant_type")]
   public string? GrantType { get; init; } = "client_credentials";
}
