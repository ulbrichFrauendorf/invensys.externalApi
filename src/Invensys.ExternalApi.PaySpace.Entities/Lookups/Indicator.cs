using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Lookups;

public class Indicator
{
    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [JsonPropertyName("Variable")]
    public double? Variable { get; set; }
}
