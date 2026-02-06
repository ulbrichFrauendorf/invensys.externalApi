using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.Iserve.Core.Authentication.Models.Response;

public class IserveAuthTokenResponse
{
   [JsonPropertyName("access_token")]
   public string? AccessToken { get; set; }

   [JsonPropertyName("expires_in")]
   public int ExpiresIn { get; set; }

   [JsonPropertyName("token_type")]
   public string? TokenType { get; set; }

   [JsonPropertyName("scope")]
   public string? Scope { get; set; }
}
