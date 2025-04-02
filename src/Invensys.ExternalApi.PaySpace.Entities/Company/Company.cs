using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Company;

public class Company
{
    [JsonPropertyName("company_id")]
    public long CompanyId { get; set; }

    [JsonPropertyName("company_name")]
    public string? CompanyName { get; set; }

    [JsonPropertyName("company_code")]
    public string? CompanyCode { get; set; }
}
