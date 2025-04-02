using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.Components;

public class EmployeePensionFund
{
   [JsonPropertyName("EmployeePensionFundId")]
   public int? EmployeePensionFundId { get; set; }

   [JsonPropertyName("CompanyPensionFund")]
   public string? CompanyPensionFund { get; set; }

   [JsonPropertyName("ComponentCompany")]
   public string? ComponentCompany { get; set; }

   [JsonPropertyName("ComponentCompanyId")]
   public int? ComponentCompanyId { get; set; }

   [JsonPropertyName("ComponentIndicatorLine")]
   public string? ComponentIndicatorLine { get; set; }

   [JsonPropertyName("CompanyPensionFundLink")]
   public string? CompanyPensionFundLink { get; set; }

   [JsonPropertyName("EffectiveDate")]
   public DateTime? EffectiveDate { get; set; }

   [JsonPropertyName("EmployeeFixedAmount")]
   public double? EmployeeFixedAmount { get; set; }

   [JsonPropertyName("EmployerFixedAmount")]
   public double? EmployerFixedAmount { get; set; }

   [JsonPropertyName("EmployeeNumber")]
   public string? EmployeeNumber { get; set; }

   [JsonPropertyName("FullName")]
   public string? FullName { get; set; }

   [JsonPropertyName("OverrideAmount")]
   public double? OverrideAmount { get; set; }

   [JsonPropertyName("ReferenceNumber")]
   public string? ReferenceNumber { get; set; }

   [JsonPropertyName("InPackage")]
   public bool? InPackage { get; set; }

   [JsonPropertyName("BcoePercentage")]
   public string? BcoePercentage { get; set; }

   [JsonPropertyName("StartDate")]
   public string? StartDate { get; set; }

   [JsonPropertyName("EndDate")]
   public string? EndDate { get; set; }

   [JsonPropertyName("Comments")]
   public string? Comments { get; set; }

   [JsonPropertyName("Values")]
   public List<string?> Values { get; } = [];
}
