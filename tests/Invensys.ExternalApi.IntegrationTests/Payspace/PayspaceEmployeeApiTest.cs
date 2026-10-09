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
      list.Should().HaveCountGreaterThanOrEqualTo(3);
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

   [Test]
   public async Task ShouldReturnEmployeeRecurringCostingListWithEmployeeFiltering()
   {
      var tokenResponse = await GetPaySpaceAuthTokenResponse();
      var companyId = tokenResponse.CompanyIds[0];
      var employeeApi = IPaySpaceEmployeeApi();
      var list = await employeeApi.EmployeeRecurringCostingAsync(tokenResponse.Token, companyId, TestDate);

      list.Should().NotBeNull();
      list.Should().NotBeEmpty("the test company must have recurring costing records at the effective date");
      list.Should().AllSatisfy(costing =>
      {
         costing.RecurringCostingSplitHeaderId.Should().BeGreaterThan(0);
         costing.EmployeeNumber.Should().NotBeNullOrWhiteSpace();
         costing.RecurringCostingSplitDetails.Should().NotBeNull();
      });

      var employeeNumbers = list.Select(costing => costing.EmployeeNumber!).Distinct().Take(2).ToArray();
      var filteredList = await employeeApi.EmployeeRecurringCostingAsync(
         tokenResponse.Token, companyId, TestDate, employeeNumbers);

      filteredList.Should().NotBeEmpty();
      filteredList.Should().OnlyContain(costing => employeeNumbers.Contains(costing.EmployeeNumber!));
      filteredList.Should().BeEquivalentTo(
         list.Where(costing => employeeNumbers.Contains(costing.EmployeeNumber!)));
   }

   [Test]
   public async Task ShouldReturnEmployeeRecurringCostingSplitListWithEmployeeFiltering()
   {
      var tokenResponse = await GetPaySpaceAuthTokenResponse();
      var companyId = tokenResponse.CompanyIds[0];
      var employeeApi = IPaySpaceEmployeeApi();
      var list = await employeeApi.EmployeeRecurringCostingSplitAsync(tokenResponse.Token, companyId, TestDate);

      list.Should().NotBeNull();
      list.Should().NotBeEmpty("the test company must have recurring costing splits at the effective date");
      list.Should().AllSatisfy(split =>
      {
         split.RecurringCostingSplitDetailId.Should().BeGreaterThan(0);
         split.EmployeeNumber.Should().NotBeNullOrWhiteSpace();
      });

      var employeeNumbers = list.Select(split => split.EmployeeNumber!).Distinct().Take(2).ToArray();
      var filteredList = await employeeApi.EmployeeRecurringCostingSplitAsync(
         tokenResponse.Token, companyId, TestDate, employeeNumbers);

      filteredList.Should().NotBeEmpty();
      filteredList.Should().OnlyContain(split => employeeNumbers.Contains(split.EmployeeNumber!));
      filteredList.Should().BeEquivalentTo(
         list.Where(split => employeeNumbers.Contains(split.EmployeeNumber!)));
   }
}
