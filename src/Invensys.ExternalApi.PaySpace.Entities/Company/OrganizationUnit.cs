using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Company;

public class OrganizationUnit
{
    [JsonPropertyName("OrganizationUnitId")]
    public int OrganizationUnitId { get; set; }

    [JsonPropertyName("ParentOrganizationUnitId")]
    public int? ParentOrganizationUnitId { get; set; }

    [JsonPropertyName("UploadCode")]
    public string? UploadCode { get; set; }

    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [JsonPropertyName("CostCentre")]
    public bool? CostCentre { get; set; }

    [JsonPropertyName("OrganizationLevel")]
    public string? OrganizationLevel { get; set; }

    [JsonPropertyName("GroupGlKey")]
    public string? GroupGlKey { get; set; }

    [JsonPropertyName("Budget")]
    public object? Budget { get; set; }

    [JsonPropertyName("Reference")]
    public object? Reference { get; set; }

    [JsonPropertyName("ManagerEmployeeNumber")]
    public object? ManagerEmployeeNumber { get; set; }

    [JsonPropertyName("InactiveDate")]
    public object? InactiveDate { get; set; }

    [JsonPropertyName("Address")]
    public object? Address { get; set; }
}
