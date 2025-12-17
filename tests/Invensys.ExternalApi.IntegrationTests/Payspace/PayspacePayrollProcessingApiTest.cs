using Invensys.ExternalApi.IntegrationTests.Helpers;
using static Invensys.ExternalApi.IntegrationTests.Testing;

namespace Invensys.ExternalApi.IntegrationTests.PaySpace;

internal class PaySpacePayrollProcessingApiTest : BaseTestFixture
{
   [Test]
   public async Task ShouldReturnEmployeeComponentsList()
   {
      var tokenResponse = await GetPaySpaceAuthTokenResponse();
      var listEmps = (
         await IPaySpaceEmployeeApi()
            .EmployeeListAsync(tokenResponse.Token, tokenResponse.CompanyIds[0], DateTime.Parse("2025-01-01"))
      )
         .SelectString(e => e.EmployeeNumber)
         .ToList();

      var paySpaceTestClient = GetPaySpaceTestClientConfig()
         ?.PaySpaceTestClients
         ?.First()!;

      var list = await IPaySpacePayrollProcessingApi()
         .GetEmployeeComponentsAsync(
            tokenResponse.Token,
            int.Parse(paySpaceTestClient.CompanyId!),
            paySpaceTestClient.FrequencyValue!,
            paySpaceTestClient.PayrunValue!,
            listEmps
         );

      list.Should().NotBeNull();
      list.Should().NotBeEmpty();
      list.Should().HaveCountGreaterThanOrEqualTo(10);
   }

   [Test]
   public async Task ShouldReturnEmployeePayRateList()
   {
      var tokenResponse = await GetPaySpaceAuthTokenResponse();
      var list = await IPaySpacePayrollProcessingApi()
         .EmployeePayRateAsync(tokenResponse.Token, tokenResponse.CompanyIds[0], DateTime.Parse("2025-03-01"));

      list.Should().NotBeNull();
      list.Should().NotBeEmpty();
      list.Should().HaveCountGreaterThanOrEqualTo(1);
   }

   [Test]
   public async Task ShouldReturnEmployeePayRateList_with_filter()
   {
      var tokenResponse = await GetPaySpaceAuthTokenResponse();
      var list = await IPaySpacePayrollProcessingApi()
         .EmployeePayRateAsync(tokenResponse.Token, tokenResponse.CompanyIds[0], DateTime.Parse("2025-03-01"));

      list.Should().NotBeNull();
      list.Should().NotBeEmpty();
      list.Should().HaveCountGreaterThanOrEqualTo(1);
   }

   [Test]
   public async Task ShouldReturnEmployeePensionFundList()
   {
      var tokenResponse = await GetPaySpaceAuthTokenResponse();
      var listEmps = (
         await IPaySpaceEmployeeApi()
            .EmployeeListAsync(tokenResponse.Token, tokenResponse.CompanyIds[0], TestDate)
      ).SelectString(e => e.EmployeeNumber);

      var list = await IPaySpacePayrollProcessingApi()
         .GetEmployeePensionFundAsync(
            tokenResponse.Token,
            tokenResponse.CompanyIds[0],
            "006_Salaries",
            "20243",
            listEmps
         );

      list.Should().NotBeNull();
      list.Should().NotBeEmpty();
      list.Should().HaveCountGreaterThanOrEqualTo(10);
   }
}
