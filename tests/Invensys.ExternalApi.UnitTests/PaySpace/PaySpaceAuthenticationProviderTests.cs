using FluentAssertions;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Authentication;
using Invensys.ExternalApi.PaySpace.Core.Authentication.Models;
using Invensys.ExternalApi.PaySpace.Entities.Company;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;

namespace Invensys.ExternalApi.UnitTests.PaySpace
{
    public class PaySpaceAuthenticationProviderTests
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private Mock<IConfiguration> _configurationMock;
        private PaySpaceAuthenticationProvider _provider;

        [SetUp]
        public void SetUp()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _configurationMock = new Mock<IConfiguration>();
            var inMemorySettings = new Dictionary<string, string?>
            {
                { "PaySpaceConfig:ApiBaseUrl", "https://example.com/" },
                { "PaySpaceConfig:AuthorizationUrl", "https://example.com/auth" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _provider = new PaySpaceAuthenticationProvider(_httpClientFactoryMock.Object, configuration);
        }

        [Test]
        public async Task GetAuthenticationResult_ShouldReturnAuthenticationResult_WhenResponseIsValid()
        {
            // Arrange
            var accessTokenRequest = new JwtAccessTokenRequest("clientId", "clientSecret", "scope");
            var tokenResponse = new PaySpaceApiTokenResponse
            {
                AccessToken = "valid_token",
                ExpiresIn = 3600,
                GroupCompanies = new GroupCompany[0]
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(tokenResponse)
                });

            // Act
            var result = await _provider.GetAuthenticationResult(accessTokenRequest);

            // Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().Be("valid_token");
            result.AuthenticationResultData.Should().BeEquivalentTo(tokenResponse.GroupCompanies);
        }

        [Test]
        public void GetAuthenticationResult_ShouldThrowException_WhenTokenResponseIsNull()
        {
            // Arrange
            var accessTokenRequest = new JwtAccessTokenRequest("clientId", "clientSecret", "scope");

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("")
                });

            // Act
            Func<Task> act = async () => await _provider.GetAuthenticationResult(accessTokenRequest);

            // Assert
            act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Test]
        public void GetAuthenticationResult_ShouldThrowException_WhenAccessTokenIsNullOrEmpty()
        {
            // Arrange
            var accessTokenRequest = new JwtAccessTokenRequest("clientId", "clientSecret", "scope");
            var tokenResponse = new PaySpaceApiTokenResponse
            {
                AccessToken = null,
                ExpiresIn = 3600,
                GroupCompanies = new GroupCompany[0]
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(tokenResponse)
                });

            // Act
            Func<Task> act = async () => await _provider.GetAuthenticationResult(accessTokenRequest);

            // Assert
            act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
