using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Lookups;

public class GenLookupValue
{
   [JsonPropertyName("Value")]
   public string? Value { get; set; }

   [JsonPropertyName("Description")]
   public string? Description { get; set; }
}
