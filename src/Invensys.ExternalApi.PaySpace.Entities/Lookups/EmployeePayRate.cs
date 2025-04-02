using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Lookups;

public class EmployeePayRate
{
    [JsonPropertyName("PayRateId")]
    public int? PayRateId { get; set; }

    [JsonPropertyName("EmployeeNumber")]
    public string? EmployeeNumber { get; set; }

    [JsonPropertyName("FullName")]
    public string? FullName { get; set; }

    [JsonPropertyName("EffectiveDate")]
    public DateTime? EffectiveDate { get; set; }

    [JsonPropertyName("AutomaticPayInd")]
    public bool? AutomaticPayInd { get; set; }

    [JsonPropertyName("CompanyFrequency")]
    public string? CompanyFrequency { get; set; }

    [JsonPropertyName("PayFrequency")]
    public string? PayFrequency { get; set; }

    [JsonPropertyName("HoursPerDay")]
    public double? HoursPerDay { get; set; }

    [JsonPropertyName("HoursPerMonth")]
    public double? HoursPerMonth { get; set; }

    [JsonPropertyName("HoursPerWeek")]
    public double? HoursPerWeek { get; set; }

    [JsonPropertyName("DaysPerPeriod")]
    public double? DaysPerPeriod { get; set; }

    [JsonPropertyName("WeeksPerMonth")]
    public int? WeeksPerMonth { get; set; }

    [JsonPropertyName("HourlyRate")]
    public double? HourlyRate { get; set; }

    [JsonPropertyName("DailyRate")]
    public double? DailyRate { get; set; }

    [JsonPropertyName("WeeklyRate")]
    public double? WeeklyRate { get; set; }

    [JsonPropertyName("MonthlyRate")]
    public double? MonthlyRate { get; set; }

    [JsonPropertyName("PercentageOfPreviousPackage")]
    public object? PercentageOfPreviousPackage { get; set; }

    [JsonPropertyName("Package")]
    public double? Package { get; set; }

    [JsonPropertyName("HoursPerFortnight")]
    public double? HoursPerFortnight { get; set; }

    [JsonPropertyName("FortnightlyRate")]
    public double? FortnightlyRate { get; set; }

    [JsonPropertyName("AnnualPackage")]
    public double? AnnualPackage { get; set; }

    [JsonPropertyName("ThirteenCheque")]
    public bool? ThirteenCheque { get; set; }

    [JsonPropertyName("IsAnnual")]
    public bool? IsAnnual { get; set; }

    [JsonPropertyName("Package2")]
    public object? Package2 { get; set; }

    [JsonPropertyName("Reason")]
    public object? Reason { get; set; }

    [JsonPropertyName("Currency")]
    public object? Currency { get; set; }

    [JsonPropertyName("OrganizationCategory")]
    public object? OrganizationCategory { get; set; }

    [JsonPropertyName("WorkingDayMonday")]
    public bool? WorkingDayMonday { get; set; }

    [JsonPropertyName("WorkingDayTuesday")]
    public bool? WorkingDayTuesday { get; set; }

    [JsonPropertyName("WorkingDayWednesday")]
    public bool? WorkingDayWednesday { get; set; }

    [JsonPropertyName("WorkingDayThursday")]
    public bool? WorkingDayThursday { get; set; }

    [JsonPropertyName("WorkingDayFriday")]
    public bool? WorkingDayFriday { get; set; }

    [JsonPropertyName("WorkingDaySaturday")]
    public bool? WorkingDaySaturday { get; set; }

    [JsonPropertyName("WorkingDaySunday")]
    public bool? WorkingDaySunday { get; set; }

    [JsonPropertyName("DateAdded")]
    public object? DateAdded { get; set; }

    [JsonPropertyName("Comments")]
    public string? Comments { get; set; }

    [JsonPropertyName("DateCreated")]
    public DateTime? DateCreated { get; set; }

    [JsonPropertyName("OneDollarInCurrency")]
    public double? OneDollarInCurrency { get; set; }

    [JsonPropertyName("CustomFields")]
    public List<object?> CustomFields { get; } = [];
}
