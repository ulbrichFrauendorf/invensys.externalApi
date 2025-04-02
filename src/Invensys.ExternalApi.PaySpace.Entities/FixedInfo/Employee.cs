using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.FixedInfo;

public record Employee
{
   [JsonPropertyName("EmployeeId")]
   public long EmployeeId { get; set; }

   [JsonPropertyName("EmployeeNumber")]
   public string? EmployeeNumber { get; set; }

   [JsonPropertyName("CompanyId")]
   public long? CompanyId { get; set; }

   [JsonPropertyName("CompanyFrequency")]
   public string? CompanyFrequency { get; set; }

   [JsonPropertyName("Title")]
   public string? Title { get; set; }

   [JsonPropertyName("FirstName")]
   public string? FirstName { get; set; }

   [JsonPropertyName("LastName")]
   public string? LastName { get; set; }

   [JsonPropertyName("PreferredName")]
   public string? PreferredName { get; set; }

   [JsonPropertyName("MaidenName")]
   public object? MaidenName { get; set; }

   [JsonPropertyName("MiddleName")]
   public string? MiddleName { get; set; }

   [JsonPropertyName("Initials")]
   public string? Initials { get; set; }

   [JsonPropertyName("Email")]
   public string? Email { get; set; }

   [JsonPropertyName("Birthday")]
   public DateTimeOffset Birthday { get; set; }

   [JsonPropertyName("HomeNumber")]
   public string? HomeNumber { get; set; }

   [JsonPropertyName("WorkNumber")]
   public string? WorkNumber { get; set; }

   [JsonPropertyName("CellNumber")]
   public string? CellNumber { get; set; }

   [JsonPropertyName("WorkExtension")]
   public string? WorkExtension { get; set; }

   [JsonPropertyName("Language")]
   public string? Language { get; set; }

   [JsonPropertyName("Gender")]
   public string? Gender { get; set; }

   [JsonPropertyName("MaritalStatus")]
   public string? MaritalStatus { get; set; }

   [JsonPropertyName("Race")]
   public string? Race { get; set; }

   [JsonPropertyName("Nationality")]
   public string? Nationality { get; set; }

   [JsonPropertyName("Citizenship")]
   public string? Citizenship { get; set; }

   [JsonPropertyName("DisabledType")]
   public string? DisabledType { get; set; }

   [JsonPropertyName("ForeignNational")]
   public object? ForeignNational { get; set; }

   [JsonPropertyName("DateCreated")]
   public DateTimeOffset DateCreated { get; set; }

   [JsonPropertyName("EmergencyContactName")]
   public object? EmergencyContactName { get; set; }

   [JsonPropertyName("EmergencyContactNumber")]
   public object? EmergencyContactNumber { get; set; }

   [JsonPropertyName("EmergencyContactAddress")]
   public object? EmergencyContactAddress { get; set; }

   [JsonPropertyName("IsRetired")]
   public bool? IsRetired { get; set; }

   [JsonPropertyName("UifExemption")]
   public string? UifExemption { get; set; }

   [JsonPropertyName("SdlExemption")]
   public string? SdlExemption { get; set; }

   [JsonPropertyName("EtiExempt")]
   public object? EtiExempt { get; set; }

   [JsonPropertyName("CustomFieldValue")]
   public string? CustomFieldValue { get; set; }

   [JsonPropertyName("CustomFieldValue2")]
   public object? CustomFieldValue2 { get; set; }

   [JsonPropertyName("DefaultPayslip")]
   public object? DefaultPayslip { get; set; }

   [JsonPropertyName("ImageDownloadUrl")]
   public Uri? ImageDownloadUrl { get; set; }

   [JsonPropertyName("Address")]
   public List<Address>? Address { get; set; }

   [JsonPropertyName("CustomFields")]
   public List<CustomField>? CustomFields { get; set; }

   [JsonPropertyName("InitiateWorkFlow")]
   public bool? InitiateWorkFlow { get; set; }
}
