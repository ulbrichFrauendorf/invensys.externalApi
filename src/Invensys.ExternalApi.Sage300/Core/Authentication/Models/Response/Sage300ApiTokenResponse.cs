using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.Sage300.Core.Authentication.Models.Response;

public class Sage300ApiTokenResponse
{
   [JsonPropertyName("expires_in")]
   public int ExpirySeconds { get; set; }
}
