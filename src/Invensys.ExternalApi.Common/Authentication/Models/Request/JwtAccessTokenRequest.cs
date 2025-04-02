using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.Common.Authentication.Models.Request;

/// <summary>
/// Represents a JWT access token request.
/// </summary>
public class JwtAccessTokenRequest(string clientId, string clientSecret, string scope) : AccessTokenRequest
{
    /// <summary>
    /// Gets the client ID.
    /// </summary>
    [JsonPropertyName("client_id")]
    public string ClientId { get; init; } = clientId;

    /// <summary>
    /// Gets the client secret.
    /// </summary>
    [JsonPropertyName("client_secret")]
    public string ClientSecret { get; init; } = clientSecret;

    /// <summary>
    /// Gets the scope.
    /// </summary>
    [JsonPropertyName("scope")]
    public string Scope { get; init; } = scope;
}
