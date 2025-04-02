// See https://aka.ms/new-console-template for more information
using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Payslips;

public class PayslipLine
{
    [JsonPropertyName("ComponentCode")]
    public string? ComponentCode { get; set; }

    [JsonPropertyName("AliasDescription")]
    public string? AliasDescription { get; set; }

    [JsonPropertyName("AlternateComponentName")]
    public string? AlternateComponentName { get; set; }

    [JsonPropertyName("PayslipAction")]
    public string? PayslipAction { get; set; }

    [JsonPropertyName("TaxCode")]
    public string? TaxCode { get; set; }

    [JsonPropertyName("ValidTaxCode")]
    public bool? ValidTaxCode { get; set; }

    [JsonPropertyName("Comments")]
    public string? Comments { get; set; }

    [JsonPropertyName("PayslipLineValue")]
    public decimal? PayslipLineValue { get; set; }

    [JsonPropertyName("YtdAmount")]
    public double? YtdAmount { get; set; }

    [JsonPropertyName("DoNotShowOnPayslip")]
    public bool? DoNotShowOnPayslip { get; set; }

    [JsonPropertyName("InPackage")]
    public bool? InPackage { get; set; }

    [JsonPropertyName("PayslipMessage")]
    public object? PayslipMessage { get; set; }

    [JsonPropertyName("Quantity")]
    public double? Quantity { get; set; }

    [JsonPropertyName("SpecialComponentCode")]
    public string? SpecialComponentCode { get; set; }
}
