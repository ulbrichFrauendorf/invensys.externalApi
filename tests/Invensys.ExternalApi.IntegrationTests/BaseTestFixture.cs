namespace Invensys.ExternalApi.IntegrationTests;

using static Invensys.ExternalApi.IntegrationTests.Testing;

[TestFixture]
public abstract class BaseTestFixture
{
   public DateTime TestDate { get; set; }
   [SetUp]
   public async Task TestSetUp()
   {
      TestDate = DateTime.UtcNow.FirstDayOfMonth();
      await ResetState();
   }
}
