using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Reports;

public class ReportGenerationRequest
{
   [JsonPropertyName("id")]
   public required string Id { get; set; }

   [JsonPropertyName("disableBookmarks")]
   public bool DisableBookmarks { get; set; }

   [JsonPropertyName("parameters")]
   public Dictionary<string, object?> Parameters { get; set; } = [];
}
