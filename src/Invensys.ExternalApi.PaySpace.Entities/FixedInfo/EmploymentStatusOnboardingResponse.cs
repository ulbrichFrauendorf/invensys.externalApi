using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.FixedInfo;

public class EmploymentStatusOnboardingResponse
{
   [JsonPropertyName("GroupJoinDate")]
   public DateOnly? GroupJoinDate { get; set; }

   [JsonPropertyName("EmploymentDate")]
   public DateOnly? EmploymentDate { get; set; }

   [JsonPropertyName("AdditionalDate")]
   public DateOnly? AdditionalDate { get; set; }

   [JsonPropertyName("AdditionalDate1")]
   public DateOnly? AdditionalDate1 { get; set; }

   [JsonPropertyName("PermitIssued")]
   public DateOnly? PermitIssued { get; set; }

   [JsonPropertyName("PermitExpiry")]
   public DateOnly? PermitExpiry { get; set; }

   [JsonPropertyName("PassportIssued")]
   public DateOnly? PassportIssued { get; set; }

   [JsonPropertyName("PassportExpiry")]
   public DateOnly? PassportExpiry { get; set; }

   [JsonPropertyName("TerminationDate")]
   public DateOnly? TerminationDate { get; set; }

   [JsonPropertyName("EmploymentCaptureDate")]
   public DateOnly? EmploymentCaptureDate { get; set; }

   [JsonPropertyName("TerminationCaptureDate")]
   public DateOnly? TerminationCaptureDate { get; set; }

   [JsonPropertyName("EmploymentStatusId")]
   public long? EmploymentStatusId { get; set; }

   [JsonPropertyName("EmployeeNumber")]
   public string? EmployeeNumber { get; set; }

   [JsonPropertyName("FullName")]
   public string? FullName { get; set; }

   [JsonPropertyName("TerminationReason")]
   public string? TerminationReason { get; set; }

   [JsonPropertyName("TaxStatus")]
   public string? TaxStatus { get; set; }

   [JsonPropertyName("TaxReferenceNumber")]
   public string? TaxReferenceNumber { get; set; }

   [JsonPropertyName("NatureOfPerson")]
   public string? NatureOfPerson { get; set; }

   [JsonPropertyName("TaxDirectiveNumber")]
   public string? TaxDirectiveNumber { get; set; }

   [JsonPropertyName("EmploymentAction")]
   public string? EmploymentAction { get; set; }

   [JsonPropertyName("TerminationCompanyRun")]
   public string? TerminationCompanyRun { get; set; }

   [JsonPropertyName("IdentityType")]
   public string? IdentityType { get; set; }

   [JsonPropertyName("IdNumber")]
   public string? IdNumber { get; set; }

   [JsonPropertyName("PassportNumber")]
   public string? PassportNumber { get; set; }

   [JsonPropertyName("PercentageAmount")]
   public string? PercentageAmount { get; set; }

   [JsonPropertyName("Amount")]
   public decimal? Amount { get; set; }

   [JsonPropertyName("Percentage")]
   public decimal? Percentage { get; set; }

   [JsonPropertyName("DeemnthlyRemuneration")]
   public decimal? DeemnthlyRemuneration { get; set; }

   [JsonPropertyName("Deemed75Indicator")]
   public bool? Deemed75Indicator { get; set; }

   [JsonPropertyName("DeemedRecoveryMonthly")]
   public bool? DeemedRecoveryMonthly { get; set; }

   [JsonPropertyName("EncashLeave")]
   public bool? EncashLeave { get; set; }

   [JsonPropertyName("Irp30")]
   public bool? Irp30 { get; set; }

   [JsonPropertyName("FinalizeIssueTaxCert")]
   public bool? FinalizeIssueTaxCert { get; set; }

   [JsonPropertyName("PassportCountry")]
   public string? PassportCountry { get; set; }

   [JsonPropertyName("TempWorker")]
   public bool? TempWorker { get; set; }

   [JsonPropertyName("NotReEmployable")]
   public bool? NotReEmployable { get; set; }

   [JsonPropertyName("ReferenceNumber")]
   public string? ReferenceNumber { get; set; }

   [JsonPropertyName("OldEmployeeId")]
   public long? OldEmployeeId { get; set; }

   [JsonPropertyName("TaxOffice")]
   public int? TaxOffice { get; set; }

   [JsonPropertyName("IT3AReason")]
   public int? IT3AReason { get; set; }

   [JsonPropertyName("CustomFields")]
   public List<CustomField>? CustomFields { get; set; }
}
