using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Company;

public class GroupCompany
{
    [JsonPropertyName("group_id")]
    public long GroupId { get; set; }

    [JsonPropertyName("group_description")]
    public string? GroupDescription { get; set; }

    [JsonPropertyName("companies")]
    public Company[]? Companies { get; set; }
}
