using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.FixedInfo;

public class CustomField
{
   [JsonPropertyName("Code")]
   public string? Code { get; set; }

   [JsonPropertyName("Label")]
   public string? Label { get; set; }

   [JsonPropertyName("Value")]
   public string? Value { get; set; }

   [JsonPropertyName("OptionCode")]
   public string? OptionCode { get; set; }
}
