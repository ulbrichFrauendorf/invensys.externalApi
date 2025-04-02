using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Loans;

public class EmployeeLoan
{
    [JsonPropertyName("EmployeeLoanId")]
    public int? EmployeeLoanId { get; set; }

    [JsonPropertyName("Customfield1")]
    public object? Customfield1 { get; set; }

    [JsonPropertyName("LoanAmount")]
    public double? LoanAmount { get; set; }

    [JsonPropertyName("BalanceBroughtForward")]
    public double? BalanceBroughtForward { get; set; }

    [JsonPropertyName("PreviousBalance")]
    public double? PreviousBalance { get; set; }

    [JsonPropertyName("InterestRate")]
    public double? InterestRate { get; set; }

    [JsonPropertyName("InstallmentType")]
    public string? InstallmentType { get; set; }

    [JsonPropertyName("LoanCompanyComponent")]
    public object? LoanCompanyComponent { get; set; }

    [JsonPropertyName("Installment")]
    public double? Installment { get; set; }

    [JsonPropertyName("IncreaseDecrease")]
    public double? IncreaseDecrease { get; set; }

    [JsonPropertyName("NonFringeBenefitLoan")]
    public bool? NonFringeBenefitLoan { get; set; }

    [JsonPropertyName("DebitAccNo")]
    public object? DebitAccNo { get; set; }

    [JsonPropertyName("CreditAccNo")]
    public object? CreditAccNo { get; set; }

    [JsonPropertyName("PreviousInterestAmount")]
    public double? PreviousInterestAmount { get; set; }

    [JsonPropertyName("OverrideOutstandingBalance")]
    public object? OverrideOutstandingBalance { get; set; }

    [JsonPropertyName("EmployeeNumber")]
    public string? EmployeeNumber { get; set; }

    [JsonPropertyName("FullName")]
    public string? FullName { get; set; }

    [JsonPropertyName("ComponentCompany")]
    public string? ComponentCompany { get; set; }

    [JsonPropertyName("ComponentCompanyId")]
    public int? ComponentCompanyId { get; set; }

    [JsonPropertyName("ComponentIndicatorLine")]
    public object? ComponentIndicatorLine { get; set; }

    [JsonPropertyName("InPackage")]
    public bool? InPackage { get; set; }

    [JsonPropertyName("BcoePercentage")]
    public object? BcoePercentage { get; set; }

    [JsonPropertyName("StartDate")]
    public object? StartDate { get; set; }

    [JsonPropertyName("EndDate")]
    public object? EndDate { get; set; }

    [JsonPropertyName("Comments")]
    public string? Comments { get; set; }

    [JsonPropertyName("Values")]
    public List<object> Values { get; } = new List<object>();
}

