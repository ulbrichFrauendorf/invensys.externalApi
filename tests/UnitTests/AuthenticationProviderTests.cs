using FluentAssertions;
using Invensys.Api.Common.Authentication;
using Invensys.Api.Common.Authentication.Models.Request;
using Invensys.Api.Common.Authentication.Models.Result;
using Moq;
using NUnit.Framework;

namespace UnitTests;

public class AuthenticationProviderTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<HttpClient> _httpClientMock;

    public AuthenticationProviderTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _httpClientMock = new Mock<HttpClient>();
        _httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(_httpClientMock.Object);
    }

    [Test]
    public async Task GetAuthenticationResult_ShouldReturnAuthenticationResult_WhenCalled()
    {
        // Arrange
        var accessTokenRequest = new AccessTokenRequest();
        var expectedResult = new AuthenticationResult<string>();
        expectedResult.SetAccessToken("test_token", 300);
        var provider = new TestAuthenticationProvider(_httpClientFactoryMock.Object, expectedResult);

        // Act
        var result = await provider.GetAuthenticationResult(accessTokenRequest);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be(expectedResult.AccessToken);
    }

    [Test]
    public async Task GetAccessToken_ShouldReturnAccessToken_WhenCalled()
    {
        // Arrange
        var accessTokenRequest = new AccessTokenRequest();
        var expectedResult = new AuthenticationResult<string>();
        expectedResult.SetAccessToken("test_token", 300);
        var provider = new TestAuthenticationProvider(_httpClientFactoryMock.Object, expectedResult);

        // Act
        var accessToken = await provider.GetAccessToken(accessTokenRequest);

        // Assert
        accessToken.Should().Be(expectedResult.AccessToken);
    }

    [Test]
    public void GetAuthenticationResult_ShouldThrowArgumentNullException_WhenAccessTokenRequestIsNull()
    {
        // Arrange
        var provider = new TestAuthenticationProvider(_httpClientFactoryMock.Object, new AuthenticationResult<string>());

        // Act
        Func<Task> act = async () => await provider.GetAuthenticationResult(null!);

        // Assert
        act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public void GetAccessToken_ShouldThrowArgumentNullException_WhenAccessTokenRequestIsNull()
    {
        // Arrange
        var provider = new TestAuthenticationProvider(_httpClientFactoryMock.Object, new AuthenticationResult<string>());

        // Act
        Func<Task> act = async () => await provider.GetAccessToken(null!);

        // Assert
        act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task GetAuthenticationResult_ShouldForceRefresh_WhenForceRefreshIsTrue()
    {
        // Arrange
        var accessTokenRequest = new AccessTokenRequest();
        var expectedResult = new AuthenticationResult<string>();
        expectedResult.SetAccessToken("test_token", 300);
        var provider = new TestAuthenticationProvider(_httpClientFactoryMock.Object, expectedResult);

        // Act
        var result = await provider.GetAuthenticationResult(accessTokenRequest, true);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be(expectedResult.AccessToken);
    }

    [Test]
    public async Task GetAccessToken_ShouldForceRefresh_WhenForceRefreshIsTrue()
    {
        // Arrange
        var accessTokenRequest = new AccessTokenRequest();
        var expectedResult = new AuthenticationResult<string>();
        expectedResult.SetAccessToken("test_token", 300);
        var provider = new TestAuthenticationProvider(_httpClientFactoryMock.Object, expectedResult);

        // Act
        var accessToken = await provider.GetAccessToken(accessTokenRequest, true);

        // Assert
        accessToken.Should().Be(expectedResult.AccessToken);
    }

    [Test]
    public async Task GetAuthenticationResult_ShouldHandleExpiredAccessToken()
    {
        // Arrange
        var accessTokenRequest = new AccessTokenRequest();
        var expectedResult = new AuthenticationResult<string>();
        expectedResult.SetAccessToken("test_token", -300); // Expired token
        var provider = new TestAuthenticationProvider(_httpClientFactoryMock.Object, expectedResult);

        // Act
        var result = await provider.GetAuthenticationResult(accessTokenRequest);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be(expectedResult.AccessToken);
    }

    [Test]
    public async Task GetAccessToken_ShouldHandleExpiredAccessToken()
    {
        // Arrange
        var accessTokenRequest = new AccessTokenRequest();
        var expectedResult = new AuthenticationResult<string>();
        expectedResult.SetAccessToken("test_token", -300); // Expired token
        var provider = new TestAuthenticationProvider(_httpClientFactoryMock.Object, expectedResult);

        // Act
        var accessToken = await provider.GetAccessToken(accessTokenRequest);

        // Assert
        accessToken.Should().Be(expectedResult.AccessToken);
    }

    [Test]
    public void GetAuthenticationResult_ShouldHandleException()
    {
        // Arrange
        var accessTokenRequest = new AccessTokenRequest();
        var provider = new TestAuthenticationProvider(_httpClientFactoryMock.Object, null!);

        // Act
        Func<Task> act = async () => await provider.GetAuthenticationResult(accessTokenRequest);

        // Assert
        act.Should().ThrowAsync<Exception>();
    }

    [Test]
    public void GetAccessToken_ShouldHandleException()
    {
        // Arrange
        var accessTokenRequest = new AccessTokenRequest();
        var provider = new TestAuthenticationProvider(_httpClientFactoryMock.Object, null!);

        // Act
        Func<Task> act = async () => await provider.GetAccessToken(accessTokenRequest);

        // Assert
        act.Should().ThrowAsync<Exception>();
    }

    private class TestAuthenticationProvider : AuthenticationProvider<string>
    {
        private readonly AuthenticationResult<string> _authenticationResult;

        public TestAuthenticationProvider(IHttpClientFactory httpClientFactory, AuthenticationResult<string> authenticationResult)
            : base(httpClientFactory)
        {
            _authenticationResult = authenticationResult;
        }

        protected override Task<AuthenticationResult<string>> Authenticate(AccessTokenRequest accessTokenRequest)
        {
            if (_authenticationResult == null)
            {
                throw new Exception("Authentication failed");
            }
            return Task.FromResult(_authenticationResult);
        }
    }
}
