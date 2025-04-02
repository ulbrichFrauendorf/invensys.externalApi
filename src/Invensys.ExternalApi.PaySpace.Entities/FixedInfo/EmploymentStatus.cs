using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.FixedInfo;

public class EmploymentStatus
{
   [JsonPropertyName("EmploymentStatusId")]
   public int? EmploymentStatusId { get; set; }

   [JsonPropertyName("EmployeeNumber")]
   public string? EmployeeNumber { get; set; }

   [JsonPropertyName("FullName")]
   public string? FullName { get; set; }

   [JsonPropertyName("GroupJoinDate")]
   public DateTime? GroupJoinDate { get; set; }

   [JsonPropertyName("EmploymentDate")]
   public DateTime? EmploymentDate { get; set; }

   [JsonPropertyName("TerminationDate")]
   public DateTime? TerminationDate { get; set; }

   [JsonPropertyName("TerminationReason")]
   public object? TerminationReason { get; set; }

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
   public object? TerminationCompanyRun { get; set; }

   [JsonPropertyName("IdentityType")]
   public string? IdentityType { get; set; }

   [JsonPropertyName("IdNumber")]
   public string? IdNumber { get; set; }

   [JsonPropertyName("PassportNumber")]
   public string? PassportNumber { get; set; }

   [JsonPropertyName("PercentageAmount")]
   public string? PercentageAmount { get; set; }

   [JsonPropertyName("Amount")]
   public string? Amount { get; set; }

   [JsonPropertyName("Percentage")]
   public string? Percentage { get; set; }

   [JsonPropertyName("DeemnthlyRemuneration")]
   public string? DeemnthlyRemuneration { get; set; }

   [JsonPropertyName("Deemed75Indicator")]
   public object? Deemed75Indicator { get; set; }

   [JsonPropertyName("DeemedRecoveryMonthly")]
   public object? DeemedRecoveryMonthly { get; set; }

   [JsonPropertyName("EncashLeave")]
   public bool? EncashLeave { get; set; }

   [JsonPropertyName("Irp30")]
   public object? Irp30 { get; set; }

   [JsonPropertyName("FinalizeIssueTaxCert")]
   public object? FinalizeIssueTaxCert { get; set; }

   [JsonPropertyName("PassportCountry")]
   public string? PassportCountry { get; set; }

   [JsonPropertyName("PassportIssued")]
   public object? PassportIssued { get; set; }

   [JsonPropertyName("PassportExpiry")]
   public object? PassportExpiry { get; set; }

   [JsonPropertyName("PermitIssued")]
   public object? PermitIssued { get; set; }

   [JsonPropertyName("PermitExpiry")]
   public object? PermitExpiry { get; set; }

   [JsonPropertyName("AdditionalDate")]
   public object? AdditionalDate { get; set; }

   [JsonPropertyName("EmploymentCaptureDate")]
   public DateTime? EmploymentCaptureDate { get; set; }

   [JsonPropertyName("TerminationCaptureDate")]
   public DateTime? TerminationCaptureDate { get; set; }

   [JsonPropertyName("TempWorker")]
   public object? TempWorker { get; set; }

   [JsonPropertyName("AdditionalDate1")]
   public object? AdditionalDate1 { get; set; }

   [JsonPropertyName("NotReEmployable")]
   public object? NotReEmployable { get; set; }

   [JsonPropertyName("ReferenceNumber")]
   public string? ReferenceNumber { get; set; }

   [JsonPropertyName("OldEmployeeId")]
   public object? OldEmployeeId { get; set; }

   [JsonPropertyName("TaxOffice")]
   public int? TaxOffice { get; set; }

   [JsonPropertyName("IT3AReason")]
   public object? IT3AReason { get; set; }

   [JsonPropertyName("CustomFields")]
   public List<CustomField> CustomFields { get; } = [];
}
