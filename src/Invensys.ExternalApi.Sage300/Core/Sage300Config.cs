namespace Invensys.ExternalApi.Sage300.Core;

public class Sage300Config
{
   public required string AuthorizationUrl { get; set; }
   public required string ApiBaseUrl { get; set; }
}

public static class Sage300HttpClient
{
   public const string Sage300RateLimitedApi = "Sage300RateLimitedApi";
}
