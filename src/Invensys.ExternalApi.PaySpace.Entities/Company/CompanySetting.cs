using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Company;

public class CompanySetting
{
   [JsonPropertyName("Code")]
   public string? Code { get; set; }

   [JsonPropertyName("Enabled")]
   public bool? Enabled { get; set; }

   [JsonPropertyName("Value")]
   public string? Value { get; set; }

   [JsonPropertyName("Description")]
   public string? Description { get; set; }

   [JsonPropertyName("Screen")]
   public string? Screen { get; set; }
}
