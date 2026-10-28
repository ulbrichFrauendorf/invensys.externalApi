using System.Net;
using Ardalis.GuardClauses;
using Invensys.ExternalApi.PaySpace.Entities.Errors;

namespace Invensys.ExternalApi.PaySpace.Core.Exceptions;

/// <summary>Indicates that PaySpace rejected an onboarding record, including failures returned with HTTP 2xx.</summary>
public sealed class PaySpaceValidationException : HttpRequestException
{
   /// <summary>Creates an exception retaining PaySpace's typed response and HTTP status.</summary>
   public PaySpaceValidationException(HttpStatusCode statusCode, PaySpaceValidationResponse paySpaceValidationResponse)
      : base(GetMessage(paySpaceValidationResponse), null, statusCode)
   {
      Response = paySpaceValidationResponse;

      Details = Array.AsReadOnly(
         paySpaceValidationResponse.Details?.OfType<PaySpaceValidationDetail>().ToArray() ?? []
      );
   }

   /// <summary>The failure envelope returned by PaySpace.</summary>
   public PaySpaceValidationResponse Response { get; }

   /// <summary>All validation messages, including record-level messages whose Field is null.</summary>
   public IReadOnlyList<PaySpaceValidationDetail> Details { get; }

   private static string GetMessage(PaySpaceValidationResponse paySpaceValidationResponse)
   {
      Guard.Against.Null(paySpaceValidationResponse, nameof(paySpaceValidationResponse));

      if (!string.IsNullOrWhiteSpace(paySpaceValidationResponse.Message))
      {
         return paySpaceValidationResponse.Message;
      }

      var messages = paySpaceValidationResponse
         .Details?.OfType<PaySpaceValidationDetail>()
         .Select(detail => detail.Message)
         .Where(message => !string.IsNullOrWhiteSpace(message));

      var message = messages == null ? null : string.Join(", ", messages);

      return string.IsNullOrWhiteSpace(message) ? "PaySpace rejected the record." : message;
   }
}
