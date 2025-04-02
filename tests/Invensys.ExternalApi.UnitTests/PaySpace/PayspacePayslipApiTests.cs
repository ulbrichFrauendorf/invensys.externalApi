using FluentAssertions;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Apis;
using Invensys.ExternalApi.PaySpace.Entities.Payslips;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invensys.ExternalApi.UnitTests.PaySpace
{
    [TestFixture]
    public class PayspacePayslipApiTests
    {
        private Mock<IPaySpaceApiClient> _paySpaceApiClientMock;
        private PaySpacePayslipApi _paySpacePayslipApi;
        private JwtAccessTokenRequest _accessTokenRequest;

        [SetUp]
        public void SetUp()
        {
            _paySpaceApiClientMock = new Mock<IPaySpaceApiClient>();
            _paySpacePayslipApi = new PaySpacePayslipApi(_paySpaceApiClientMock.Object);
            _accessTokenRequest = new JwtAccessTokenRequest("clientId", "clientSecret", "scope");
        }

        [Test]
        public async Task GetConsolidatedEmployeePayslipsAsync_ShouldReturnConsolidatedPayslips()
        {
            var companyId = 1L;
            var year = 2023;
            var month = 1;
            var expectedPayslips = new List<EmployeePayslip> { new EmployeePayslip { PayslipId = 1 } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<EmployeePayslip>(_accessTokenRequest, $"{companyId}/EmployeePayslip/{year}/{month}/consolidated"))
                .ReturnsAsync(expectedPayslips);

            var result = await _paySpacePayslipApi.GetConsolidatedEmployeePayslipsAsync(_accessTokenRequest, companyId, year, month);

            result.Should().BeEquivalentTo(expectedPayslips);
        }

        [Test]
        public async Task GetConsolidatedEmployeePayslipsAsync_WithComponentCodes_ShouldReturnConsolidatedPayslips()
        {
            var companyId = 1L;
            var year = 2023;
            var month = 1;
            var componentCodes = new List<string> { "C001" };
            var expectedPayslips = new List<EmployeePayslip> { new EmployeePayslip { PayslipId = 1 } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<EmployeePayslip>(_accessTokenRequest, $"{companyId}/EmployeePayslip/{year}/{month}/consolidated?componentCodes={string.Join(',', componentCodes)}"))
                .ReturnsAsync(expectedPayslips);

            var result = await _paySpacePayslipApi.GetConsolidatedEmployeePayslipsAsync(_accessTokenRequest, companyId, year, month, componentCodes);

            result.Should().BeEquivalentTo(expectedPayslips);
        }

        [Test]
        public async Task GetEmployeePayslipLinesAsync_ShouldReturnPayslipLines()
        {
            var companyId = 1L;
            var year = 2023;
            var month = 1;
            var expectedPayslipLines = new List<EmployeePayslipLine> { new EmployeePayslipLine { PayslipLineId = 1 } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<EmployeePayslipLine>(_accessTokenRequest, $"{companyId}/EmployeePayslipLine/{year}/{month}"))
                .ReturnsAsync(expectedPayslipLines);

            var result = await _paySpacePayslipApi.GetEmployeePayslipLinesAsync(_accessTokenRequest, companyId, year, month);

            result.Should().BeEquivalentTo(expectedPayslipLines);
        }

        [Test]
        public async Task GetEmployeePayslipsAsync_ShouldReturnPayslips()
        {
            var companyId = 1L;
            var year = 2023;
            var month = 1;
            var expectedPayslips = new List<EmployeePayslip> { new EmployeePayslip { PayslipId = 1 } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<EmployeePayslip>(_accessTokenRequest, $"{companyId}/EmployeePayslip/{year}/{month}"))
                .ReturnsAsync(expectedPayslips);

            var result = await _paySpacePayslipApi.GetEmployeePayslipsAsync(_accessTokenRequest, companyId, year, month);

            result.Should().BeEquivalentTo(expectedPayslips);
        }

        [Test]
        public async Task GetEmployeePayslipsAsync_WithComponentCodes_ShouldReturnPayslips()
        {
            var companyId = 1L;
            var year = 2023;
            var month = 1;
            var componentCodes = new List<string> { "C001" };
            var expectedPayslips = new List<EmployeePayslip> { new EmployeePayslip { PayslipId = 1 } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<EmployeePayslip>(_accessTokenRequest, $"{companyId}/EmployeePayslip/{year}/{month}?componentCodes={string.Join(',', componentCodes)}"))
                .ReturnsAsync(expectedPayslips);

            var result = await _paySpacePayslipApi.GetEmployeePayslipsAsync(_accessTokenRequest, companyId, year, month, componentCodes);

            result.Should().BeEquivalentTo(expectedPayslips);
        }
    }
}
