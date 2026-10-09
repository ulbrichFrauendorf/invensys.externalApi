using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.FixedInfo;

public class EmployeeRecurringCostingSplit
{
   [JsonPropertyName("RecurringCostingSplitDetailId")]
   public long? RecurringCostingSplitDetailId { get; set; }

   [JsonPropertyName("OrganizationGroup")]
   public string? OrganizationGroup { get; set; }

   [JsonPropertyName("OrganizationGroupDescription")]
   public string? OrganizationGroupDescription { get; set; }

   [JsonPropertyName("EmployeeNumber")]
   public string? EmployeeNumber { get; set; }

   [JsonPropertyName("Percentage")]
   public decimal? Percentage { get; set; }

   [JsonPropertyName("EffectiveDate")]
   public DateTime? EffectiveDate { get; set; }
}
