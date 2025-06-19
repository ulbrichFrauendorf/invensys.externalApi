namespace Invensys.ExternalApi.IntegrationTests;

using static Invensys.ExternalApi.IntegrationTests.Testing;

[TestFixture]
public abstract class BaseTestFixture
{
   [SetUp]
   public async Task TestSetUp()
   {
      await ResetState();
   }
}
