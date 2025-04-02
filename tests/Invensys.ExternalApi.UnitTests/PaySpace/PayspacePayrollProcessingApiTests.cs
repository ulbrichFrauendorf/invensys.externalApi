using FluentAssertions;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Apis;
using Invensys.ExternalApi.PaySpace.Entities.Components;
using Invensys.ExternalApi.PaySpace.Entities.Lookups;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Moq;
using NUnit.Framework;

namespace Invensys.ExternalApi.UnitTests.PaySpace
{
    [TestFixture]
    public class PayspacePayrollProcessingApiTests
    {
        private Mock<IPaySpaceApiClient> _paySpaceApiClientMock;
        private PaySpacePayrollProcessingApi _paySpacePayrollProcessingApi;
        private JwtAccessTokenRequest _accessTokenRequest;

        [SetUp]
        public void SetUp()
        {
            _paySpaceApiClientMock = new Mock<IPaySpaceApiClient>();
            _paySpacePayrollProcessingApi = new PaySpacePayrollProcessingApi(_paySpaceApiClientMock.Object);
            _accessTokenRequest = new JwtAccessTokenRequest("clientId", "clientSecret", "scope");
        }

        [Test]
        public async Task EmployeePayRateAsync_ShouldReturnEmployeePayRates()
        {
            var companyId = 1L;
            var effectiveDate = new DateTime(2023, 1, 1);
            var expectedPayRates = new List<EmployeePayRate> { new EmployeePayRate { PayRateId = 1 } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<EmployeePayRate>(_accessTokenRequest, $"{companyId}/EmployeePayRate/effective/{effectiveDate:yyyy-MM-dd}"))
                .ReturnsAsync(expectedPayRates);

            var result = await _paySpacePayrollProcessingApi.EmployeePayRateAsync(_accessTokenRequest, companyId, effectiveDate);

            result.Should().BeEquivalentTo(expectedPayRates);
        }

        [Test]
        public async Task EmployeePayRateAsync_WithEmployeeNumbers_ShouldReturnEmployeePayRates()
        {
            var companyId = 1L;
            var effectiveDate = new DateTime(2023, 1, 1);
            var employeeNumbers = new List<string> { "E001" };
            var expectedPayRates = new List<EmployeePayRate> { new EmployeePayRate { PayRateId = 1 } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsyncWithListFilter<EmployeePayRate>(_accessTokenRequest, $"{companyId}/EmployeePayRate/effective/{effectiveDate:yyyy-MM-dd}", "EmployeeNumber", employeeNumbers))
                .ReturnsAsync(expectedPayRates);

            var result = await _paySpacePayrollProcessingApi.EmployeePayRateAsync(_accessTokenRequest, companyId, effectiveDate, employeeNumbers);

            result.Should().BeEquivalentTo(expectedPayRates);
        }

        [Test]
        public async Task GetEmployeePensionFundAsync_ShouldReturnEmployeePensionFunds()
        {
            var companyId = 1L;
            var frequency = "Monthly";
            var period = "2023-01";
            var employeeNumbers = new List<string> { "E001" };
            var expectedPensionFunds = new List<EmployeePensionFund> { new EmployeePensionFund { EmployeePensionFundId = 1 } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsyncWithListFilter<EmployeePensionFund>(_accessTokenRequest, $"{companyId}/EmployeePensionFund?frequency={frequency}&period={period}", "EmployeeNumber", employeeNumbers))
                .ReturnsAsync(expectedPensionFunds);

            var result = await _paySpacePayrollProcessingApi.GetEmployeePensionFundAsync(_accessTokenRequest, companyId, frequency, period, employeeNumbers);

            result.Should().BeEquivalentTo(expectedPensionFunds);
        }

        [Test]
        public async Task GetEmployeeComponentsAsync_ShouldReturnEmployeeComponents()
        {
            var companyId = 1L;
            var frequency = "Monthly";
            var period = "2023-01";
            var employeeNumbers = new List<string> { "E001" };
            var expectedComponents = new List<EmployeeComponent> { new EmployeeComponent { ComponentEmployeeId = 1 } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsyncWithListFilter<EmployeeComponent>(_accessTokenRequest, $"{companyId}/EmployeeComponent?frequency={frequency}&period={period}", "EmployeeNumber", employeeNumbers))
                .ReturnsAsync(expectedComponents);

            var result = await _paySpacePayrollProcessingApi.GetEmployeeComponentsAsync(_accessTokenRequest, companyId, frequency, period, employeeNumbers);

            result.Should().BeEquivalentTo(expectedComponents);
        }
    }
}
