using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Incidents;

public class EmployeeIncident
{
    [JsonPropertyName("EmployeeIncidentId")]
    public int? EmployeeIncidentId { get; set; }

    [JsonPropertyName("EmployeeNumber")]
    public string? EmployeeNumber { get; set; }

    [JsonPropertyName("FullName")]
    public string? FullName { get; set; }

    [JsonPropertyName("LinkedIncidentId")]
    public object? LinkedIncidentId { get; set; }

    [JsonPropertyName("IncidentType")]
    public string? IncidentType { get; set; }

    [JsonPropertyName("CompanyOffence")]
    public string? CompanyOffence { get; set; }

    [JsonPropertyName("OffenceOutcome")]
    public string? OffenceOutcome { get; set; }

    [JsonPropertyName("OffenceCategory")]
    public object? OffenceCategory { get; set; }

    [JsonPropertyName("AppealReason")]
    public object? AppealReason { get; set; }

    [JsonPropertyName("CompanyLegalBody")]
    public object? CompanyLegalBody { get; set; }

    [JsonPropertyName("CompanyIncidentType")]
    public object? CompanyIncidentType { get; set; }

    [JsonPropertyName("AppealOutcome")]
    public object? AppealOutcome { get; set; }

    [JsonPropertyName("AwardFavour")]
    public object? AwardFavour { get; set; }

    [JsonPropertyName("OtherOutcome")]
    public object? OtherOutcome { get; set; }

    [JsonPropertyName("SettlementReinstate")]
    public object? SettlementReinstate { get; set; }

    [JsonPropertyName("IncidentDate")]
    public DateTime? IncidentDate { get; set; }

    [JsonPropertyName("IncidentNotes")]
    public string? IncidentNotes { get; set; }

    [JsonPropertyName("EmployeeRepresentative")]
    public string? EmployeeRepresentative { get; set; }

    [JsonPropertyName("CompanyRepresentative")]
    public string? CompanyRepresentative { get; set; }

    [JsonPropertyName("OutcomeDetails")]
    public object? OutcomeDetails { get; set; }

    [JsonPropertyName("DateCharged")]
    public DateTime? DateCharged { get; set; }

    [JsonPropertyName("HearingDate")]
    public DateTime? HearingDate { get; set; }

    [JsonPropertyName("Chairperson")]
    public string? Chairperson { get; set; }

    [JsonPropertyName("CompanyWitnesses")]
    public string? CompanyWitnesses { get; set; }

    [JsonPropertyName("EmployeeWitnesses")]
    public string? EmployeeWitnesses { get; set; }

    [JsonPropertyName("ValidityDate")]
    public DateTime? ValidityDate { get; set; }

    [JsonPropertyName("CaseRefNumber")]
    public object? CaseRefNumber { get; set; }

    [JsonPropertyName("SRDescription")]
    public object? SRDescription { get; set; }

    [JsonPropertyName("SRMonetaryValue")]
    public object? SRMonetaryValue { get; set; }

    [JsonPropertyName("SRReinstatementDate")]
    public object? SRReinstatementDate { get; set; }

    [JsonPropertyName("SRConditions")]
    public object? SRConditions { get; set; }

    [JsonPropertyName("CommissionerName")]
    public object? CommissionerName { get; set; }

    [JsonPropertyName("LegalRepresentative")]
    public object? LegalRepresentative { get; set; }

    [JsonPropertyName("SettlementDetails")]
    public object? SettlementDetails { get; set; }

    [JsonPropertyName("Accused")]
    public object? Accused { get; set; }

    [JsonPropertyName("Facilitator")]
    public object? Facilitator { get; set; }

    [JsonPropertyName("NatureOfGrievance")]
    public object? NatureOfGrievance { get; set; }

    [JsonPropertyName("GrievanceRecommendation")]
    public object? GrievanceRecommendation { get; set; }

    [JsonPropertyName("DateOfOutcome")]
    public object? DateOfOutcome { get; set; }

    [JsonPropertyName("ReferralDate")]
    public object? ReferralDate { get; set; }

    [JsonPropertyName("OutcomeNotes")]
    public object? OutcomeNotes { get; set; }

    [JsonPropertyName("IsGuilty")]
    public bool? IsGuilty { get; set; }

    [JsonPropertyName("ReportedBy")]
    public object? ReportedBy { get; set; }

    [JsonPropertyName("OutcomeDate")]
    public object? OutcomeDate { get; set; }

    [JsonPropertyName("AttachmentUrls")]
    public List<string> AttachmentUrls { get; } = [];

    [JsonPropertyName("CustomFields")]
    public List<object> CustomFields { get; } = [];
}
