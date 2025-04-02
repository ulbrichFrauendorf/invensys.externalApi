using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.FixedInfo;

public class EmployeeBankDetail
{
    [JsonPropertyName("BankDetailId")]
    public int? BankDetailId { get; set; }

    [JsonPropertyName("EmployeeNumber")]
    public string? EmployeeNumber { get; set; }

    [JsonPropertyName("FullName")]
    public string? FullName { get; set; }

    [JsonPropertyName("PaymentMethod")]
    public string? PaymentMethod { get; set; }

    [JsonPropertyName("SplitType")]
    public string? SplitType { get; set; }

    [JsonPropertyName("BankAccountOwner")]
    public string? BankAccountOwner { get; set; }

    [JsonPropertyName("BankAccountOwnerName")]
    public string? BankAccountOwnerName { get; set; }

    [JsonPropertyName("AccountType")]
    public string? AccountType { get; set; }

    [JsonPropertyName("BankName")]
    public string? BankName { get; set; }

    [JsonPropertyName("BankBranchNo")]
    public string? BankBranchNo { get; set; }

    [JsonPropertyName("BankAccountNo")]
    public string? BankAccountNo { get; set; }

    [JsonPropertyName("Reference")]
    public string? Reference { get; set; }

    [JsonPropertyName("Amount")]
    public double? Amount { get; set; }

    [JsonPropertyName("Comments")]
    public string? Comments { get; set; }

    [JsonPropertyName("SwiftCode")]
    public object? SwiftCode { get; set; }

    [JsonPropertyName("RoutingCode")]
    public object? RoutingCode { get; set; }

    [JsonPropertyName("CompanyComponent")]
    public object? CompanyComponent { get; set; }

    [JsonPropertyName("Currency")]
    public object? Currency { get; set; }

    [JsonPropertyName("CompanyEdbIndicator")]
    public object? CompanyEdbIndicator { get; set; }

    [JsonPropertyName("SkipValidation")]
    public bool? SkipValidation { get; set; }
}
