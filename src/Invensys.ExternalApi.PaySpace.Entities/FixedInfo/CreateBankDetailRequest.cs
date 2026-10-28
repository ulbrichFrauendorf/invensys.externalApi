using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.FixedInfo;

public class CreateBankDetailRequest
{
   [JsonPropertyName("EmployeeNumber")]
   public string? EmployeeNumber { get; set; }

   [JsonPropertyName("PaymentMethod")]
   public string? PaymentMethod { get; set; }

   [JsonPropertyName("SplitType")]
   public string? SplitType { get; set; }

   [JsonPropertyName("BankAccountOwner")]
   public string? BankAccountOwner { get; set; }

   [JsonPropertyName("BankAccountOwnerName")]
   public string? BankAccountOwnerName { get; set; }

   [JsonPropertyName("AccountType")]
   public string? AccountType { get; set; }

   [JsonPropertyName("BankName")]
   public string? BankName { get; set; }

   [JsonPropertyName("BankBranchNo")]
   public string? BankBranchNo { get; set; }

   [JsonPropertyName("BankAccountNo")]
   public string? BankAccountNo { get; set; }

   [JsonPropertyName("Reference")]
   public string? Reference { get; set; }

   [JsonPropertyName("Amount")]
   public decimal? Amount { get; set; }

   [JsonPropertyName("Comments")]
   public string? Comments { get; set; }

   [JsonPropertyName("SwiftCode")]
   public string? SwiftCode { get; set; }

   [JsonPropertyName("RoutingCode")]
   public string? RoutingCode { get; set; }

   [JsonPropertyName("CompanyComponent")]
   public string? CompanyComponent { get; set; }

   [JsonPropertyName("Currency")]
   public string? Currency { get; set; }

   [JsonPropertyName("CompanyEdbIndicator")]
   public string? CompanyEdbIndicator { get; set; }

   [JsonPropertyName("SkipValidation")]
   public bool? SkipValidation { get; set; }
}
