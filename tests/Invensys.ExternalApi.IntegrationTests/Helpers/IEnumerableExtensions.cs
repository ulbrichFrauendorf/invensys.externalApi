using Ardalis.GuardClauses;

namespace Invensys.ExternalApi.IntegrationTests.Helpers;

public static class IEnumerableExtensions
{
   public static IEnumerable<string> SelectString<T>(
      this IEnumerable<T> source,
      Func<T, string?> selector,
      string defaultValue = ""
   )
   {
      Guard.Against.Null(source, nameof(source));
      Guard.Against.Null(selector, nameof(selector));

      foreach (var item in source)
      {
         yield return selector(item) ?? defaultValue;
      }
   }
}
