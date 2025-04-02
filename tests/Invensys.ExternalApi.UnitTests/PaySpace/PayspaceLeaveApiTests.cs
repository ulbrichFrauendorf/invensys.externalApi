using FluentAssertions;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Apis;
using Invensys.ExternalApi.PaySpace.Entities.Leave;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invensys.ExternalApi.UnitTests.PaySpace
{
    [TestFixture]
    public class PayspaceLeaveApiTests
    {
        private Mock<IPaySpaceApiClient> _paySpaceApiClientMock;
        private PaySpaceLeaveApi _paySpaceLeaveApi;
        private JwtAccessTokenRequest _accessTokenRequest;

        [SetUp]
        public void SetUp()
        {
            _paySpaceApiClientMock = new Mock<IPaySpaceApiClient>();
            _paySpaceLeaveApi = new PaySpaceLeaveApi(_paySpaceApiClientMock.Object);
            _accessTokenRequest = new JwtAccessTokenRequest("clientId", "clientSecret", "scope");
        }

        [Test]
        public async Task EmployeeLeaveApplicationListAsync_WithYearAndMonth_ShouldReturnLeaveApplications()
        {
            var companyId = 1L;
            var year = 2023;
            var month = 1;
            var expectedApplications = new List<EmployeeLeaveApplication> { new EmployeeLeaveApplication { LeaveAdjustmentId = 1 } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<EmployeeLeaveApplication>(_accessTokenRequest, $"{companyId}/EmployeeLeaveApplication/{year}/{month}"))
                .ReturnsAsync(expectedApplications);

            var result = await _paySpaceLeaveApi.EmployeeLeaveApplicationListAsync(_accessTokenRequest, companyId, year, month);

            result.Should().BeEquivalentTo(expectedApplications);
        }
    }
}
