using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.FixedInfo;

public class CreatePositionRequest
{
   [JsonPropertyName("EffectiveDate")]
   public DateOnly? EffectiveDate { get; set; }

   [JsonPropertyName("PositionEffectiveDate")]
   public DateOnly? PositionEffectiveDate { get; set; }

   [JsonPropertyName("EmployeeNumber")]
   public string? EmployeeNumber { get; set; }

   [JsonPropertyName("OrganizationPositionId")]
   public long? OrganizationPositionId { get; set; }

   [JsonPropertyName("OrganizationPositionWithCode")]
   public string? OrganizationPositionWithCode { get; set; }

   [JsonPropertyName("PositionType")]
   public string? PositionType { get; set; }

   [JsonPropertyName("Grade")]
   public string? Grade { get; set; }

   [JsonPropertyName("DirectlyReportsPositionOverride")]
   public string? DirectlyReportsPositionOverride { get; set; }

   [JsonPropertyName("OrganizationGroup")]
   public string? OrganizationGroup { get; set; }

   [JsonPropertyName("OrganizationRegion")]
   public string? OrganizationRegion { get; set; }

   [JsonPropertyName("PayPoint")]
   public string? PayPoint { get; set; }

   [JsonPropertyName("DirectlyReportsEmployeeNumber")]
   public string? DirectlyReportsEmployeeNumber { get; set; }

   [JsonPropertyName("EmploymentCategory")]
   public string? EmploymentCategory { get; set; }

   [JsonPropertyName("EmploymentSubCategory")]
   public string? EmploymentSubCategory { get; set; }

   [JsonPropertyName("AdministratorEmployeeNumber")]
   public string? AdministratorEmployeeNumber { get; set; }

   [JsonPropertyName("WorkflowRole")]
   public string? WorkflowRole { get; set; }

   [JsonPropertyName("GeneralLedger")]
   public string? GeneralLedger { get; set; }

   [JsonPropertyName("TradeUnion")]
   public string? TradeUnion { get; set; }

   [JsonPropertyName("IsPromotion")]
   public bool? IsPromotion { get; set; }

   [JsonPropertyName("KeepJob")]
   public bool? KeepJob { get; set; }

   [JsonPropertyName("Roster")]
   public string? Roster { get; set; }

   [JsonPropertyName("Job")]
   public string? Job { get; set; }

   [JsonPropertyName("Comments")]
   public string? Comments { get; set; }

   [JsonPropertyName("AltPositionName")]
   public string? AltPositionName { get; set; }

   [JsonPropertyName("CustomTradeUnion")]
   public string? CustomTradeUnion { get; set; }

   [JsonPropertyName("CustomFields")]
   public List<CustomField>? CustomFields { get; set; }
}
