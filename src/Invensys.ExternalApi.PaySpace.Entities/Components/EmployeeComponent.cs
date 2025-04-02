using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Components;

public class EmployeeComponent
{
    [JsonPropertyName("ComponentEmployeeId")]
    public int? ComponentEmployeeId { get; set; }

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
    public object? Comments { get; set; }

    [JsonPropertyName("Values")]
    public List<object?> Values { get; } = [];
}
