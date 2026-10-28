using System.Text.Json.Serialization;

namespace Invensys.ExternalApi.PaySpace.Entities.FixedInfo;

/// <summary>An address supplied when creating an employee; the first address must be Physical.</summary>
public class CreateEmployeeAddressRequest
{
   [JsonPropertyName("EmployeeNumber")]
   public string? EmployeeNumber { get; set; }

   [JsonPropertyName("AddressType")]
   public string? AddressType { get; set; }

   [JsonPropertyName("AddressLine1")]
   public string? AddressLine1 { get; set; }

   [JsonPropertyName("AddressLine2")]
   public string? AddressLine2 { get; set; }

   [JsonPropertyName("AddressLine3")]
   public string? AddressLine3 { get; set; }

   [JsonPropertyName("AddressCode")]
   public string? AddressCode { get; set; }

   [JsonPropertyName("Province")]
   public string? Province { get; set; }

   [JsonPropertyName("AddressCountry")]
   public string? AddressCountry { get; set; }

   [JsonPropertyName("UnitNumber")]
   public string? UnitNumber { get; set; }

   [JsonPropertyName("Complex")]
   public string? Complex { get; set; }

   [JsonPropertyName("StreetNumber")]
   public string? StreetNumber { get; set; }

   [JsonPropertyName("SameAsPhysical")]
   public bool? SameAsPhysical { get; set; }

   [JsonPropertyName("IsCareofAddress")]
   public bool? IsCareofAddress { get; set; }

   [JsonPropertyName("CareOfIntermediary")]
   public string? CareOfIntermediary { get; set; }

   [JsonPropertyName("SpecialServices")]
   public string? SpecialServices { get; set; }

   [JsonPropertyName("Municipality")]
   public string? Municipality { get; set; }

   [JsonPropertyName("AddressStreetType")]
   public string? AddressStreetType { get; set; }
}
