using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.FixedInfo;

public class EmployeeRecurringCosting
{
   [JsonPropertyName("RecurringCostingSplitHeaderId")]
   public long? RecurringCostingSplitHeaderId { get; set; }

   [JsonPropertyName("EmployeeNumber")]
   public string? EmployeeNumber { get; set; }

   [JsonPropertyName("FullName")]
   public string? FullName { get; set; }

   [JsonPropertyName("EffectiveDate")]
   public DateTime? EffectiveDate { get; set; }

   [JsonPropertyName("BasedOnSplitOption")]
   public string? BasedOnSplitOption { get; set; }

   [JsonPropertyName("RecurringCostingSplitDetails")]
   public List<RecurringCostingSplitDetail>? RecurringCostingSplitDetails { get; set; }
}
