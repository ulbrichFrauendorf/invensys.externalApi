using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Lookups;

public class CompanyRun
{
   [JsonPropertyName("PeriodStartDate")]
   public DateTime? PeriodStartDate { get; set; }

   [JsonPropertyName("PeriodEndDate")]
   public DateTime? PeriodEndDate { get; set; }

   [JsonPropertyName("PayDate")]
   public DateTime? PayDate { get; set; }

   [JsonPropertyName("RunStatus")]
   public string? RunStatus { get; set; }

   [JsonPropertyName("OrderNumber")]
   public int? OrderNumber { get; set; }

   [JsonPropertyName("IsMainRun")]
   public bool? IsMainRun { get; set; }

   [JsonPropertyName("RunType")]
   public string? RunType { get; set; }

   [JsonPropertyName("Value")]
   public string? Value { get; set; }

   [JsonPropertyName("Description")]
   public string? Description { get; set; }
}
