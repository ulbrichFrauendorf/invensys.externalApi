using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.Odata
{
   public class ListResponse<T>
   {
      [JsonPropertyName("@odata.context")]
      public Uri? OdataContext { get; set; }

      [JsonPropertyName("@odata.nextLink")]
      public Uri? OdataNextLink { get; set; }

      [JsonPropertyName("value")]
      public required List<T> Value { get; set; }
   }
}
