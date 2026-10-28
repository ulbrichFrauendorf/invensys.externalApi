using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Errors;

/// <summary>The failure envelope returned when PaySpace rejects an operation.</summary>
public class PaySpaceValidationResponse
{
   [JsonPropertyName("@odata.context")]
   public string? OdataContext { get; set; }

   public string? Message { get; set; }
   public List<PaySpaceValidationDetail>? Details { get; set; }
   public bool? Success { get; set; }
}
