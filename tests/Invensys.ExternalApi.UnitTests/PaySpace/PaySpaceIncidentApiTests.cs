using FluentAssertions;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Apis;
using Invensys.ExternalApi.PaySpace.Entities.Incidents;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invensys.ExternalApi.UnitTests.PaySpace
{
    [TestFixture]
    public class PaySpaceIncidentApiTests
    {
        private Mock<IPaySpaceApiClient> _paySpaceApiClientMock;
        private PaySpaceIncidentApi _paySpaceIncidentApi;
        private JwtAccessTokenRequest _accessTokenRequest;

        [SetUp]
        public void SetUp()
        {
            _paySpaceApiClientMock = new Mock<IPaySpaceApiClient>();
            _paySpaceIncidentApi = new PaySpaceIncidentApi(_paySpaceApiClientMock.Object);
            _accessTokenRequest = new JwtAccessTokenRequest("clientId", "clientSecret", "scope");
        }

        [Test]
        public async Task EmployeeIncidentListAsync_ShouldReturnEmployeeIncidents()
        {
            var companyId = 1L;
            var expectedIncidents = new List<EmployeeIncident> { new EmployeeIncident { EmployeeIncidentId = 1 } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<EmployeeIncident>(_accessTokenRequest, $"{companyId}/EmployeeIncident"))
                .ReturnsAsync(expectedIncidents);

            var result = await _paySpaceIncidentApi.EmployeeIncidentListAsync(_accessTokenRequest, companyId);

            result.Should().BeEquivalentTo(expectedIncidents);
        }
    }
}
