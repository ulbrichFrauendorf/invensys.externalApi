using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Payslips;

public class EmployeePayslipLine
{
    [JsonPropertyName("PayslipLineId")]
    public int? PayslipLineId { get; set; }

    [JsonPropertyName("EmployeeNumber")]
    public string? EmployeeNumber { get; set; }

    [JsonPropertyName("RunDescription")]
    public string? RunDescription { get; set; }

    [JsonPropertyName("PeriodCode")]
    public string? PeriodCode { get; set; }

    [JsonPropertyName("Comments")]
    public string? Comments { get; set; }

    [JsonPropertyName("AliasDescription")]
    public string? AliasDescription { get; set; }

    [JsonPropertyName("AlternateComponentName")]
    public object? AlternateComponentName { get; set; }

    [JsonPropertyName("ComponentCode")]
    public string? ComponentCode { get; set; }

    [JsonPropertyName("PayslipAction")]
    public string? PayslipAction { get; set; }

    [JsonPropertyName("TaxCode")]
    public string? TaxCode { get; set; }

    [JsonPropertyName("ValidTaxCode")]
    public bool? ValidTaxCode { get; set; }

    [JsonPropertyName("PayslipLineValue")]
    public double? PayslipLineValue { get; set; }

    [JsonPropertyName("YtdAmount")]
    public double? YtdAmount { get; set; }

    [JsonPropertyName("DoNotShowOnPayslip")]
    public bool? DoNotShowOnPayslip { get; set; }

    [JsonPropertyName("DoNotConvertCurrency")]
    public bool? DoNotConvertCurrency { get; set; }

    [JsonPropertyName("InPackage")]
    public bool? InPackage { get; set; }

    [JsonPropertyName("Quantity")]
    public double? Quantity { get; set; }

    [JsonPropertyName("Balance")]
    public double? Balance { get; set; }

    [JsonPropertyName("SpecialComponentCode")]
    public string? SpecialComponentCode { get; set; }
}
