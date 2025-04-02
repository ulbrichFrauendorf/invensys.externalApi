using System.Text.Json.Serialization;
using Invensys.ExternalApi.PaySpace.Entities.Company;

namespace Invensys.ExternalApi.PaySpace.Core.Authentication.Models;

public class PaySpaceApiTokenResponse
{
   [JsonPropertyName("access_token")]
   public string? AccessToken { get; set; }

   [JsonPropertyName("expires_in")]
   public int ExpiresIn { get; set; }

   [JsonPropertyName("token_type")]
   public string? TokenType { get; set; }

   [JsonPropertyName("scope")]
   public string? Scope { get; set; }

   [JsonPropertyName("group_companies")]
   public GroupCompany[]? GroupCompanies { get; set; }
}
