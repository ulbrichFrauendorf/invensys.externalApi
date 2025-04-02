using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Components;

public class PensionFundComponentCompany
{
    [JsonPropertyName("ComponentId")]
    public int? ComponentId { get; set; }

    [JsonPropertyName("Action")]
    public string? Action { get; set; }

    [JsonPropertyName("ComponentType")]
    public string? ComponentType { get; set; }

    [JsonPropertyName("ComponentCode")]
    public string? ComponentCode { get; set; }

    [JsonPropertyName("TaxCode")]
    public string? TaxCode { get; set; }

    [JsonPropertyName("TableBuilderCategory")]
    public object? TableBuilderCategory { get; set; }

    [JsonPropertyName("FormulaTableType")]
    public string? FormulaTableType { get; set; }

    [JsonPropertyName("ComponentLines")]
    public List<object>? ComponentLines { get; set; }

    [JsonPropertyName("ComponentSubCodes")]
    public List<object>? ComponentSubCodes { get; set; }

    [JsonPropertyName("InPackage")]
    public bool? InPackage { get; set; }

    [JsonPropertyName("EnforcePackageRule")]
    public bool? EnforcePackageRule { get; set; }

    [JsonPropertyName("FieldCode")]
    public string? FieldCode { get; set; }

    [JsonPropertyName("EnableBcoe")]
    public bool? EnableBcoe { get; set; }

    [JsonPropertyName("MedicalCategory")]
    public string? MedicalCategory { get; set; }

    [JsonPropertyName("ComponentCategory")]
    public string? ComponentCategory { get; set; }

    [JsonPropertyName("IsMonthlyByWeeks")]
    public bool? IsMonthlyByWeeks { get; set; }

    [JsonPropertyName("SpecialComponentCode")]
    public string? SpecialComponentCode { get; set; }

    [JsonPropertyName("IsOnceOffComponent")]
    public bool? IsOnceOffComponent { get; set; }

    [JsonPropertyName("IsStatutoryComponent")]
    public bool? IsStatutoryComponent { get; set; }

    [JsonPropertyName("Value")]
    public string? Value { get; set; }

    [JsonPropertyName("Description")]
    public string? Description { get; set; }
}
