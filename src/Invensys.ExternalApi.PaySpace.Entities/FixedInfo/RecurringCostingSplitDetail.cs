using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.FixedInfo;

public class RecurringCostingSplitDetail
{
   [JsonPropertyName("RecurringCostingSplitDetailId")]
   public long? RecurringCostingSplitDetailId { get; set; }

   [JsonPropertyName("OrganizationGroup")]
   public string? OrganizationGroup { get; set; }

   [JsonPropertyName("OrganizationGroupDescription")]
   public string? OrganizationGroupDescription { get; set; }

   [JsonPropertyName("ProjectCode")]
   public string? ProjectCode { get; set; }

   [JsonPropertyName("ProjectActivityCode")]
   public string? ProjectActivityCode { get; set; }

   [JsonPropertyName("Percentage")]
   public decimal? Percentage { get; set; }
}
