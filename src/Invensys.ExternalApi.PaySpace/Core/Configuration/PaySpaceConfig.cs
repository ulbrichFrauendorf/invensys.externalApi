namespace Invensys.ExternalApi.PaySpace.Core.Configuration;

public class PaySpaceConfig
{
   public required string AuthorizationUrl { get; set; }

   /// <summary>The API domain only, for example https://apistaging.payspace.com.</summary>
   public required string ApiBaseUrl { get; set; }
}
