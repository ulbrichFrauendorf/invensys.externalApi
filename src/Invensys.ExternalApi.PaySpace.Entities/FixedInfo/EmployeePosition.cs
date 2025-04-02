using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.FixedInfo;

public class EmployeePosition
{
   [JsonPropertyName("EmployeePositionId")]
   public int? EmployeePositionId { get; set; }

   [JsonPropertyName("EmployeeNumber")]
   public string? EmployeeNumber { get; set; }

   [JsonPropertyName("FullName")]
   public string? FullName { get; set; }

   [JsonPropertyName("EffectiveDate")]
   public DateTime? EffectiveDate { get; set; }

   [JsonPropertyName("OrganizationPosition")]
   public string? OrganizationPosition { get; set; }

   [JsonPropertyName("OrganizationPositionId")]
   public int? OrganizationPositionId { get; set; }

   [JsonPropertyName("OrganizationPositionWithCode")]
   public string? OrganizationPositionWithCode { get; set; }

   [JsonPropertyName("PositionType")]
   public string? PositionType { get; set; }

   [JsonPropertyName("Grade")]
   public string? Grade { get; set; }

   [JsonPropertyName("OccupationalLevel")]
   public string? OccupationalLevel { get; set; }

   [JsonPropertyName("DirectlyReportsPositionOverride")]
   public object? DirectlyReportsPositionOverride { get; set; }

   [JsonPropertyName("DirectlyReportsPosition")]
   public object? DirectlyReportsPosition { get; set; }

   [JsonPropertyName("OrganizationGroup")]
   public string? OrganizationGroup { get; set; }

   [JsonPropertyName("OrganizationGroupDescription")]
   public string? OrganizationGroupDescription { get; set; }

   [JsonPropertyName("OrganizationGroups")]
   public List<object>? OrganizationGroups { get; set; }

   [JsonPropertyName("OrganizationRegion")]
   public string? OrganizationRegion { get; set; }

   [JsonPropertyName("PayPoint")]
   public string? PayPoint { get; set; }

   [JsonPropertyName("DirectlyReportsEmployee")]
   public string? DirectlyReportsEmployee { get; set; }

   [JsonPropertyName("DirectlyReportsEmployeeNumber")]
   public string? DirectlyReportsEmployeeNumber { get; set; }

   [JsonPropertyName("EmploymentCategory")]
   public object? EmploymentCategory { get; set; }

   [JsonPropertyName("EmploymentSubCategory")]
   public object? EmploymentSubCategory { get; set; }

   [JsonPropertyName("Administrator")]
   public object? Administrator { get; set; }

   [JsonPropertyName("AdministratorEmployeeNumber")]
   public object? AdministratorEmployeeNumber { get; set; }

   [JsonPropertyName("WorkflowRole")]
   public string? WorkflowRole { get; set; }

   [JsonPropertyName("GeneralLedger")]
   public string? GeneralLedger { get; set; }

   [JsonPropertyName("TradeUnion")]
   public object? TradeUnion { get; set; }

   [JsonPropertyName("IsPromotion")]
   public bool? IsPromotion { get; set; }

   [JsonPropertyName("Roster")]
   public object? Roster { get; set; }

   [JsonPropertyName("Job")]
   public object? Job { get; set; }

   [JsonPropertyName("Comments")]
   public object? Comments { get; set; }

   [JsonPropertyName("AltPositionName")]
   public object? AltPositionName { get; set; }

   [JsonPropertyName("PositionEffectiveDate")]
   public DateTime? PositionEffectiveDate { get; set; }

   [JsonPropertyName("CustomTradeUnion")]
   public object? CustomTradeUnion { get; set; }

   [JsonPropertyName("CustomFields")]
   public List<object>? CustomFields { get; set; }
}
