using System.Globalization;

namespace Invensys.ExternalApi.PaySpace.Core;

/// <summary>
/// Formats PaySpace calendar dates in South African time. UTC and local values are
/// converted before selecting the date; unspecified values are South African wall-clock time.
/// </summary>
public static class PaySpaceDateHelper
{
   private static readonly TimeZoneInfo SouthAfricaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Africa/Johannesburg");

   private static DateTime ToSouthAfricaDate(DateTime date)
   {
      var localDate = date.Kind == DateTimeKind.Unspecified
         ? date
         : TimeZoneInfo.ConvertTime(date, SouthAfricaTimeZone);
      return DateTime.SpecifyKind(localDate.Date, DateTimeKind.Unspecified);
   }

   /// <summary>Returns an invariant yyyy-MM-dd date for effective-date URL paths.</summary>
   public static string FormatEffectiveDate(DateTime date)
      => ToSouthAfricaDate(date).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

   /// <summary>Returns a URL-escaped OData date at South African midnight with its UTC offset.</summary>
   public static string FormatODataDate(DateTime date)
   {
      var localDate = ToSouthAfricaDate(date);
      var dateWithOffset = new DateTimeOffset(localDate, SouthAfricaTimeZone.GetUtcOffset(localDate));
      return Uri.EscapeDataString(dateWithOffset.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture));
   }
}
