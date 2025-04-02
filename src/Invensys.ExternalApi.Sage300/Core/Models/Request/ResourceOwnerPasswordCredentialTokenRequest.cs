using Invensys.ExternalApi.Common.Authentication.Models.Request;
using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.Sage300.Core.Models.Request;

public class ResourceOwnerPasswordCredentialTokenRequest : AccessTokenRequest
{
    [JsonPropertyName("grant_type")]
    public string? GrantType { get; init; } = "password";

    [JsonPropertyName("username")]
    public string? Username { get; init; }

    [JsonPropertyName("password")]
    public string? Password { get; init; }

    [JsonPropertyName("scope")]
    public string? Scope { get; init; }

    public required string AuthorizationUrl { get; init; }
}
