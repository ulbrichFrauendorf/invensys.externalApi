// See https://aka.ms/new-console-template for more information
using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Payslips;

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class LeaveBalance
{
    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [JsonPropertyName("Balance")]
    public double? Balance { get; set; }

    [JsonPropertyName("Accrual")]
    public double? Accrual { get; set; }

    [JsonPropertyName("LeaveType")]
    public string? LeaveType { get; set; }

    [JsonPropertyName("DoNotShowOnPayslip")]
    public bool? DoNotShowOnPayslip { get; set; }

    [JsonPropertyName("DaysDueToForfeit")]
    public double? DaysDueToForfeit { get; set; }

    [JsonPropertyName("DaysDueToCarryOver")]
    public double? DaysDueToCarryOver { get; set; }

    [JsonPropertyName("ExpiryOrCarryOverDate")]
    public DateTime? ExpiryOrCarryOverDate { get; set; }
}
