using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.Odata
{
    public class ErrorResponse
    {
        [JsonPropertyName("@odata.context")]
        public string? OdataContext { get; set; }

        [JsonPropertyName("Message")]
        public string? Message { get; set; }

        [JsonPropertyName("Details")]
        public List<Detail> Details { get; } = [];

        [JsonPropertyName("Success")]
        public bool? Success { get; set; }
    }

    public class Detail
    {
        [JsonPropertyName("Message")]
        public string? Message { get; set; }
    }
}
