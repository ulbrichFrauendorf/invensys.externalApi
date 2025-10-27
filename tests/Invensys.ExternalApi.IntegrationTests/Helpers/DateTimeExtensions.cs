namespace Invensys.ExternalApi.IntegrationTests;

public static class DateTimeExtensions
{
 /// <summary>
 /// Returns a DateTime representing the first day of the month for the provided date.
 /// The returned DateTime preserves the Kind of the source date and sets the time to midnight.
 /// </summary>
 public static DateTime FirstDayOfMonth(this DateTime date)
 {
 return new DateTime(date.Year, date.Month,1,0,0,0, date.Kind);
 }
}
