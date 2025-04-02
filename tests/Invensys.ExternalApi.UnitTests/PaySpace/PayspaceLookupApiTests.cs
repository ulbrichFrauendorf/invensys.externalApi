using FluentAssertions;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Apis;
using Invensys.ExternalApi.PaySpace.Entities.Lookups;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invensys.ExternalApi.UnitTests.PaySpace
{
    [TestFixture]
    public class PayspaceLookupApiTests
    {
        private Mock<IPaySpaceApiClient> _paySpaceApiClientMock;
        private PaySpaceLookupApi _paySpaceLookupApi;
        private JwtAccessTokenRequest _accessTokenRequest;

        [SetUp]
        public void SetUp()
        {
            _paySpaceApiClientMock = new Mock<IPaySpaceApiClient>();
            _paySpaceLookupApi = new PaySpaceLookupApi(_paySpaceApiClientMock.Object);
            _accessTokenRequest = new JwtAccessTokenRequest("clientId", "clientSecret", "scope");
        }

        [Test]
        public async Task GetCompanyPensionFundAsync_ShouldReturnCompanyPensionFunds()
        {
            var companyId = 1L;
            var expectedPensionFunds = new List<GenLookupValue> { new GenLookupValue { Value = "PF001" } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<GenLookupValue>(_accessTokenRequest, $"{companyId}/Lookup/CompanyPensionFund"))
                .ReturnsAsync(expectedPensionFunds);

            var result = await _paySpaceLookupApi.GetCompanyPensionFundAsync(_accessTokenRequest, companyId);

            result.Should().BeEquivalentTo(expectedPensionFunds);
        }

        [Test]
        public async Task GetNatureOfPersonAsync_ShouldReturnNatureOfPersons()
        {
            var companyId = 1L;
            var expectedNatureOfPersons = new List<GenLookupValue> { new GenLookupValue { Value = "NP001" } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<GenLookupValue>(_accessTokenRequest, $"{companyId}/Lookup/NatureOfPerson"))
                .ReturnsAsync(expectedNatureOfPersons);

            var result = await _paySpaceLookupApi.GetNatureOfPersonAsync(_accessTokenRequest, companyId);

            result.Should().BeEquivalentTo(expectedNatureOfPersons);
        }

        [Test]
        public async Task GetOrganizationLevelsAsync_ShouldReturnOrganizationLevels()
        {
            var companyId = 1L;
            var expectedOrganizationLevels = new List<GenLookupValue> { new GenLookupValue { Value = "OL001" } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<GenLookupValue>(_accessTokenRequest, $"{companyId}/Lookup/OrganizationLevel"))
                .ReturnsAsync(expectedOrganizationLevels);

            var result = await _paySpaceLookupApi.GetOrganizationLevelsAsync(_accessTokenRequest, companyId);

            result.Should().BeEquivalentTo(expectedOrganizationLevels);
        }

        [Test]
        public async Task GetOrganizationGroupsAsync_ShouldReturnOrganizationGroups()
        {
            var companyId = 1L;
            var expectedOrganizationGroups = new List<GenLookupValue> { new GenLookupValue { Value = "OG001" } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<GenLookupValue>(_accessTokenRequest, $"{companyId}/Lookup/OrganizationGroup"))
                .ReturnsAsync(expectedOrganizationGroups);

            var result = await _paySpaceLookupApi.GetOrganizationGroupsAsync(_accessTokenRequest, companyId);

            result.Should().BeEquivalentTo(expectedOrganizationGroups);
        }

        [Test]
        public async Task GetOrganizationCategoriesAsync_ShouldReturnOrganizationCategories()
        {
            var companyId = 1L;
            var expectedOrganizationCategories = new List<GenLookupValue> { new GenLookupValue { Value = "OC001" } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<GenLookupValue>(_accessTokenRequest, $"{companyId}/Lookup/OrganizationCategory"))
                .ReturnsAsync(expectedOrganizationCategories);

            var result = await _paySpaceLookupApi.GetOrganizationCategoriesAsync(_accessTokenRequest, companyId);

            result.Should().BeEquivalentTo(expectedOrganizationCategories);
        }
    }
}
