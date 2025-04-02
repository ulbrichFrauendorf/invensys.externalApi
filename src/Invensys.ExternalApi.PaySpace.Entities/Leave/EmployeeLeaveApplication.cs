using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Leave;

public class EmployeeLeaveApplication
{
   [JsonPropertyName("LeaveAdjustmentId")]
   public int? LeaveAdjustmentId { get; set; }

   [JsonPropertyName("EmployeeNumber")]
   public string? EmployeeNumber { get; set; }

   [JsonPropertyName("FullName")]
   public string? FullName { get; set; }

   [JsonPropertyName("LeaveBucket")]
   public string? LeaveBucket { get; set; }

   [JsonPropertyName("LeaveType")]
   public string? LeaveType { get; set; }

   [JsonPropertyName("LeaveCompanyRun")]
   public string? LeaveCompanyRun { get; set; }

   [JsonPropertyName("NoOfDays")]
   public decimal? NoOfDays { get; set; }

   [JsonPropertyName("TimeOfDay")]
   public object? TimeOfDay { get; set; }

   [JsonPropertyName("Comments")]
   public string? Comments { get; set; }

   [JsonPropertyName("Reference")]
   public string? Reference { get; set; }

   [JsonPropertyName("LeaveReason")]
   public object? LeaveReason { get; set; }

   [JsonPropertyName("LeaveStartDate")]
   public DateTime? LeaveStartDate { get; set; }

   [JsonPropertyName("LeaveEndDate")]
   public DateTime? LeaveEndDate { get; set; }

   [JsonPropertyName("LeaveStatus")]
   public string? LeaveStatus { get; set; }

   [JsonPropertyName("LeaveTransactionType")]
   public string? LeaveTransactionType { get; set; }

   [JsonPropertyName("SkipValidation")]
   public bool? SkipValidation { get; set; }

   [JsonPropertyName("ThirteenCheque")]
   public object? ThirteenCheque { get; set; }

   [JsonPropertyName("CancellationId")]
   public int? CancellationId { get; set; }

   [JsonPropertyName("AttachmentUrl")]
   public string? AttachmentUrl { get; set; }

   [JsonPropertyName("TempAttachmentSignature")]
   public object? TempAttachmentSignature { get; set; }

   [JsonPropertyName("ConcessionYearEnd")]
   public string? ConcessionYearEnd { get; set; }

   [JsonPropertyName("LastModifiedDate")]
   public object? LastModifiedDate { get; set; }

   [JsonPropertyName("CustomFields")]
   public List<object> CustomFields { get; } = [];
}
