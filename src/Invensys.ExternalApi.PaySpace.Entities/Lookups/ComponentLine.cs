using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Lookups;

public class ComponentLine
{
    [JsonPropertyName("CalculationElement")]
    public string? CalculationElement { get; set; }

    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [JsonPropertyName("ValueType")]
    public string? ValueType { get; set; }

    [JsonPropertyName("Indicators")]
    public List<Indicator>? Indicators { get; set; }

    [JsonPropertyName("ComponentValue")]
    public object? ComponentValue { get; set; }
}
