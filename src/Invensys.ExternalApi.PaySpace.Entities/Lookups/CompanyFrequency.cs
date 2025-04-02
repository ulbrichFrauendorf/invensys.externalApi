using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Lookups;

public class CompanyFrequency
{
    [JsonPropertyName("Frequency")]
    public string? Frequency { get; set; }

    [JsonPropertyName("HoursPerDay")]
    public double? HoursPerDay { get; set; }

    [JsonPropertyName("DaysPerPeriod")]
    public double? DaysPerPeriod { get; set; }

    [JsonPropertyName("Value")]
    public string? Value { get; set; }

    [JsonPropertyName("Description")]
    public string? Description { get; set; }
}
