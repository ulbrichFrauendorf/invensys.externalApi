// See https://aka.ms/new-console-template for more information
using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Payslips;

public class EmployeePayslip
{
    [JsonPropertyName("PayslipId")]
    public int? PayslipId { get; set; }

    [JsonPropertyName("Frequency")]
    public string? Frequency { get; set; }

    [JsonPropertyName("CompanyRun")]
    public string? CompanyRun { get; set; }

    [JsonPropertyName("RunDescription")]
    public string? RunDescription { get; set; }

    [JsonPropertyName("OrderNumber")]
    public int? OrderNumber { get; set; }

    [JsonPropertyName("PeriodStartDate")]
    public DateTime? PeriodStartDate { get; set; }

    [JsonPropertyName("PeriodEndDate")]
    public DateTime? PeriodEndDate { get; set; }

    [JsonPropertyName("PayDate")]
    public DateTime? PayDate { get; set; }

    [JsonPropertyName("PeriodCode")]
    public string? PeriodCode { get; set; }

    [JsonPropertyName("Paid")]
    public object? Paid { get; set; }

    [JsonPropertyName("PayslipComments")]
    public object? PayslipComments { get; set; }

    [JsonPropertyName("EmployeeNumber")]
    public string? EmployeeNumber { get; set; }

    [JsonPropertyName("HourlyRate")]
    public object? HourlyRate { get; set; }

    [JsonPropertyName("TotalValue")]
    public double? TotalValue { get; set; }

    [JsonPropertyName("NetPay")]
    public double? NetPay { get; set; }

    [JsonPropertyName("PayslipLines")]
    public List<PayslipLine> PayslipLines { get; init; } = [];

    [JsonPropertyName("LeaveBalances")]
    public List<LeaveBalance> LeaveBalances { get; init; } = [];
}
