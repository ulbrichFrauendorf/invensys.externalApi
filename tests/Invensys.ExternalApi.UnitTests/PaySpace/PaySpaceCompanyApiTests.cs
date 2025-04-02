using FluentAssertions;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Apis;
using Invensys.ExternalApi.PaySpace.Entities.Company;
using Invensys.ExternalApi.PaySpace.Entities.Components;
using Invensys.ExternalApi.PaySpace.Entities.Lookups;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Moq;
using NUnit.Framework;

namespace Invensys.ExternalApi.UnitTests.PaySpace
{
    [TestFixture]
    public class PaySpaceCompanyApiTests
    {
        private Mock<IPaySpaceApiClient> _paySpaceApiClientMock;
        private PaySpaceCompanyApi _paySpaceCompanyApi;
        private JwtAccessTokenRequest _accessTokenRequest;

        [SetUp]
        public void SetUp()
        {
            _paySpaceApiClientMock = new Mock<IPaySpaceApiClient>();
            _paySpaceCompanyApi = new PaySpaceCompanyApi(_paySpaceApiClientMock.Object);
            _accessTokenRequest = new JwtAccessTokenRequest("clientId", "clientSecret", "scope");
        }

        [Test]
        public async Task GetCompanyFrequenciesAsync_ShouldReturnCompanyFrequencies()
        {
            var companyId = 1L;
            var expectedFrequencies = new List<CompanyFrequency> { new() { Frequency = "Monthly" } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<CompanyFrequency>(_accessTokenRequest, $"{companyId}/Lookup/CompanyFrequency"))
                .ReturnsAsync(expectedFrequencies);

            var result = await _paySpaceCompanyApi.GetCompanyFrequenciesAsync(_accessTokenRequest, companyId);

            result.Should().BeEquivalentTo(expectedFrequencies);
        }

        [Test]
        public async Task GetCompanyFrequenciesAsync_ShouldReturnEmptyList_WhenNoDataAvailable()
        {
            var companyId = 1L;

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<CompanyFrequency>(_accessTokenRequest, $"{companyId}/Lookup/CompanyFrequency"))
                .ReturnsAsync(new List<CompanyFrequency>());

            var result = await _paySpaceCompanyApi.GetCompanyFrequenciesAsync(_accessTokenRequest, companyId);

            result.Should().BeEmpty();
        }

        [Test]
        public async Task GetCompanyFrequenciesAsync_ShouldThrowException_OnApiFailure()
        {
            var companyId = 1L;

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<CompanyFrequency>(_accessTokenRequest, $"{companyId}/Lookup/CompanyFrequency"))
                .ThrowsAsync(new Exception("API Error"));

            await FluentActions.Invoking(() => _paySpaceCompanyApi.GetCompanyFrequenciesAsync(_accessTokenRequest, companyId))
                .Should().ThrowAsync<Exception>()
                .WithMessage("API Error");
        }

        [Test]
        public async Task GetPensionFundComponentCompanyAsync_ShouldReturnData()
        {
            var companyId = 1L;
            var frequency = "Monthly";
            var period = "2023-01";
            var expectedData = new List<PensionFundComponentCompany> { new() { ComponentCode = "PF001" } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<PensionFundComponentCompany>(_accessTokenRequest, $"{companyId}/Lookup/PensionFundComponentCompany?frequency={frequency}&period={period}"))
                .ReturnsAsync(expectedData);

            var result = await _paySpaceCompanyApi.GetPensionFundComponentCompanyAsync(_accessTokenRequest, companyId, frequency, period);

            result.Should().BeEquivalentTo(expectedData);
        }

        [Test]
        public async Task GetPensionFundComponentCompanyAsync_ShouldReturnEmptyList_WhenNoDataAvailable()
        {
            var companyId = 1L;
            var frequency = "Monthly";
            var period = "2023-01";

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<PensionFundComponentCompany>(_accessTokenRequest, $"{companyId}/Lookup/PensionFundComponentCompany?frequency={frequency}&period={period}"))
                .ReturnsAsync(new List<PensionFundComponentCompany>());

            var result = await _paySpaceCompanyApi.GetPensionFundComponentCompanyAsync(_accessTokenRequest, companyId, frequency, period);

            result.Should().BeEmpty();
        }

        [Test]
        public async Task GetPensionFundComponentCompanyAsync_ShouldThrowException_OnApiFailure()
        {
            var companyId = 1L;
            var frequency = "Monthly";
            var period = "2023-01";

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<PensionFundComponentCompany>(_accessTokenRequest, $"{companyId}/Lookup/PensionFundComponentCompany?frequency={frequency}&period={period}"))
                .ThrowsAsync(new Exception("API Error"));

            await FluentActions.Invoking(() => _paySpaceCompanyApi.GetPensionFundComponentCompanyAsync(_accessTokenRequest, companyId, frequency, period))
                .Should().ThrowAsync<Exception>()
                .WithMessage("API Error");
        }

        [Test]
        public async Task GetOrganizationUnitsAsync_ShouldReturnData()
        {
            var companyId = 1L;
            var expectedUnits = new List<OrganizationUnit> { new() { OrganizationUnitId = 1 } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<OrganizationUnit>(_accessTokenRequest, $"{companyId}/OrganizationUnit"))
                .ReturnsAsync(expectedUnits);

            var result = await _paySpaceCompanyApi.GetOrganizationUnitsAsync(_accessTokenRequest, companyId);

            result.Should().BeEquivalentTo(expectedUnits);
        }

        [Test]
        public async Task GetOrganizationUnitsAsync_ShouldReturnEmptyList_WhenNoDataAvailable()
        {
            var companyId = 1L;

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<OrganizationUnit>(_accessTokenRequest, $"{companyId}/OrganizationUnit"))
                .ReturnsAsync(new List<OrganizationUnit>());

            var result = await _paySpaceCompanyApi.GetOrganizationUnitsAsync(_accessTokenRequest, companyId);

            result.Should().BeEmpty();
        }

        [Test]
        public async Task GetOrganizationUnitsAsync_ShouldThrowException_OnApiFailure()
        {
            var companyId = 1L;

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<OrganizationUnit>(_accessTokenRequest, $"{companyId}/OrganizationUnit"))
                .ThrowsAsync(new Exception("API Error"));

            await FluentActions.Invoking(() => _paySpaceCompanyApi.GetOrganizationUnitsAsync(_accessTokenRequest, companyId))
                .Should().ThrowAsync<Exception>()
                .WithMessage("API Error");
        }

        [Test]
        public async Task CompanySettingsAsync_ShouldReturnData()
        {
            var companyId = 1L;
            var expectedSettings = new List<CompanySetting> { new() { Code = "CS001" } };

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<CompanySetting>(_accessTokenRequest, $"{companyId}/CompanySetting"))
                .ReturnsAsync(expectedSettings);

            var result = await _paySpaceCompanyApi.CompanySettingsAsync(_accessTokenRequest, companyId);

            result.Should().BeEquivalentTo(expectedSettings);
        }

        [Test]
        public async Task CompanySettingsAsync_ShouldReturnEmptyList_WhenNoDataAvailable()
        {
            var companyId = 1L;

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<CompanySetting>(_accessTokenRequest, $"{companyId}/CompanySetting"))
                .ReturnsAsync(new List<CompanySetting>());

            var result = await _paySpaceCompanyApi.CompanySettingsAsync(_accessTokenRequest, companyId);

            result.Should().BeEmpty();
        }

        [Test]
        public async Task CompanySettingsAsync_ShouldThrowException_OnApiFailure()
        {
            var companyId = 1L;

            _paySpaceApiClientMock
                .Setup(client => client.GetListAsync<CompanySetting>(_accessTokenRequest, $"{companyId}/CompanySetting"))
                .ThrowsAsync(new Exception("API Error"));

            await FluentActions.Invoking(() => _paySpaceCompanyApi.CompanySettingsAsync(_accessTokenRequest, companyId))
                .Should().ThrowAsync<Exception>()
                .WithMessage("API Error");
        }
    }
}
