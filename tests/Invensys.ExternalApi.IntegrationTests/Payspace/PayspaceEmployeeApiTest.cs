using static Invensys.ExternalApi.IntegrationTests.Testing;

namespace Invensys.ExternalApi.IntegrationTests.PaySpace;

internal class PaySpaceEmployeeApiTest : BaseTestFixture
{
   [Test]
   public async Task ShouldReturnEmployeeList()
   {
      var tokenResponse = await GetPaySpaceAuthTokenResponse();
      var list = await IPaySpaceEmployeeApi()
         .EmployeeListAsync(tokenResponse.Token, tokenResponse.CompanyIds[0], TestDate);

      list.Should().NotBeNull();
      list.Should().NotBeEmpty();
      list.Should().HaveCountGreaterThanOrEqualTo(10);
   }

   [Test]
   public async Task ShouldReturnEmployeeStatusList()
   {
      var tokenResponse = await GetPaySpaceAuthTokenResponse();
      var list = await IPaySpaceEmployeeApi()
         .EmployeeEmploymentStatusAsync(tokenResponse.Token, tokenResponse.CompanyIds[0], TestDate);

      list.Should().NotBeNull();
      list.Should().NotBeEmpty();
      list.Should().HaveCountGreaterThanOrEqualTo(10);
   }

   [Test]
   public async Task ShouldReturnEmployeeStatusAllList()
   {
      var tokenResponse = await GetPaySpaceAuthTokenResponse();
      // Run for each company to ensure data is returned
      foreach (var companyId in tokenResponse.CompanyIds)
      {
         var list = await IPaySpaceEmployeeApi()
            .EmployeeEmploymentStatusAllAsync(tokenResponse.Token, companyId);
         list.Should().NotBeNull();
         list.Should().NotBeEmpty();
         list.Should().HaveCountGreaterThanOrEqualTo(10);
      }
   }

   [Test]
   public async Task ShouldReturnEmployeeBankDetailsList()
   {
      var tokenResponse = await GetPaySpaceAuthTokenResponse();
      var list = await IPaySpaceEmployeeApi().EmployeeBankDetailAsync(tokenResponse.Token, tokenResponse.CompanyIds[0]);

      list.Should().NotBeNull();
      list.Should().NotBeEmpty();
      list.Should().HaveCountGreaterThanOrEqualTo(10);
   }

   [Test]
   public async Task ShouldReturnEmployeePositionList()
   {
      var tokenResponse = await GetPaySpaceAuthTokenResponse();
      var list = await IPaySpaceEmployeeApi()
         .EmployeePositionsAsync(tokenResponse.Token, tokenResponse.CompanyIds[0], TestDate);

      list.Should().NotBeNull();
      list.Should().NotBeEmpty();
      list.Should().HaveCountGreaterThanOrEqualTo(10);
   }
}
