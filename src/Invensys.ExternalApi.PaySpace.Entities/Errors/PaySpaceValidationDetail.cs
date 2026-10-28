namespace Invensys.ExternalApi.PaySpace.Entities.Errors;

/// <summary>A validation message returned by PaySpace for a field or the whole record.</summary>
public class PaySpaceValidationDetail
{
   public string? Message { get; set; }

   /// <summary>The rejected field, or null when the message applies to the whole record.</summary>
   public string? Field { get; set; }
}
