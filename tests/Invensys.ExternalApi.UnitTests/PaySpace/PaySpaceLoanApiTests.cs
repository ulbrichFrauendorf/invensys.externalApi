using FluentAssertions;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Apis;
using Invensys.ExternalApi.PaySpace.Entities.Loans;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invensys.ExternalApi.UnitTests.PaySpace
{
    [TestFixture]
    public class PaySpaceLoanApiTests
    {
        private Mock<IPaySpaceApiClient> _paySpaceApiClientMock;
        private PaySpaceLoanApi _paySpaceLoanApi;
        private JwtAccessTokenRequest _accessTokenRequest;

        [SetUp]
        public void SetUp()
        {
            _paySpaceApiClientMock = new Mock<IPaySpaceApiClient>();
            _paySpaceLoanApi = new PaySpaceLoanApi(_paySpaceApiClientMock.Object);
            _accessTokenRequest = new JwtAccessTokenRequest("clientId", "clientSecret", "scope");
        }

        [Test]
        public async Task GetEmployeeLoanAsync_ShouldReturnEmployeeLoans()
        {
            var companyId = 1L;
            var frequency = "Monthly";
            var period = "2023-01";
            var employeeNumbers = new List<string> { "E001" };
            var expectedLoans = new List<EmployeeLoan> { new EmployeeLoan { EmployeeLoanId = 1 } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsyncWithListFilter<EmployeeLoan>(_accessTokenRequest, $"{companyId}/EmployeeLoan?frequency={frequency}&period={period}", "EmployeeNumber", employeeNumbers))
                .ReturnsAsync(expectedLoans);

            var result = await _paySpaceLoanApi.GetEmployeeLoanAsync(_accessTokenRequest, companyId, frequency, period, employeeNumbers);

            result.Should().BeEquivalentTo(expectedLoans);
        }
    }
}
