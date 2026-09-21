using FluentAssertions;
using Invensys.ExternalApi.PaySpace.Core;
using NUnit.Framework;

namespace Invensys.ExternalApi.UnitTests.PaySpace;

public class PaySpaceDateHelperTests
{
   [TestCase(DateTimeKind.Utc)]
   [TestCase(DateTimeKind.Local)]
   public void ConvertsInstantsAcrossTheSouthAfricanDateBoundary(DateTimeKind kind)
   {
      var utc = new DateTime(2024, 12, 31, 22, 30, 0, DateTimeKind.Utc);
      var input = kind == DateTimeKind.Local ? utc.ToLocalTime() : utc;

      PaySpaceDateHelper.FormatEffectiveDate(input).Should().Be("2025-01-01");
      PaySpaceDateHelper.FormatODataDate(input).Should().Be("2025-01-01T00%3A00%3A00%2B02%3A00");
   }

   [Test]
   public void TreatsUnspecifiedDatesAsSouthAfricanWallClockTime()
   {
      var input = new DateTime(2024, 12, 31, 23, 30, 0, DateTimeKind.Unspecified);

      PaySpaceDateHelper.FormatEffectiveDate(input).Should().Be("2024-12-31");
      PaySpaceDateHelper.FormatODataDate(input).Should().Be("2024-12-31T00%3A00%3A00%2B02%3A00");
   }

   [Test]
   [SetCulture("th-TH")]
   public void UsesInvariantGregorianDates()
   {
      var input = new DateTime(2025, 1, 1);

      PaySpaceDateHelper.FormatEffectiveDate(input).Should().Be("2025-01-01");
      PaySpaceDateHelper.FormatODataDate(input).Should().Be("2025-01-01T00%3A00%3A00%2B02%3A00");
   }
}
