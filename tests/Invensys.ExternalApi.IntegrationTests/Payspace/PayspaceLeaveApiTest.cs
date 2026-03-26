using static Invensys.ExternalApi.IntegrationTests.Testing;

namespace Invensys.ExternalApi.IntegrationTests.PaySpace;

internal class PaySpaceLeaveApiTest : BaseTestFixture
{
   [Test]
   public async Task ShouldReturnEmployeeLeaveApplicationList()
   {
      var tokenResponse = await GetPaySpaceAuthTokenResponse();
      var list = await IPaySpaceLeaveApi()
         .EmployeeLeaveApplicationListAsync(tokenResponse.Token, tokenResponse.CompanyIds[0], 2026, 2);

      list.Should().NotBeNull();
      list.Should().NotBeEmpty();
   }

   [Test]
   [Obsolete("Use the overload with year and month parameters.")]
   public async Task ShouldReturnEmployeeLeaveApplicationListWithoutYearMonth()
   {
      var tokenResponse = await GetPaySpaceAuthTokenResponse();
      var list = await IPaySpaceLeaveApi()
         .EmployeeLeaveApplicationListAsync(tokenResponse.Token, tokenResponse.CompanyIds[0]);

      list.Should().NotBeNull();
      list.Should().NotBeEmpty();
   }
}
