using FluentAssertions;
using Invensys.ExternalApi.Common.Authentication;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.Common.Exceptions;
using Invensys.ExternalApi.Common.Http;
using Invensys.ExternalApi.Common.Http.Models.Enums;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;

namespace Invensys.ExternalApi.UnitTests;

[TestFixture]
public class ExternalApiClientTests
{
   private Mock<IHttpClientFactory> _httpClientFactoryMock;
   private Mock<IAuthenticationProvider> _authenticationProviderMock;
   private Mock<HttpMessageHandler> _httpMessageHandlerMock;
   private HttpClient _httpClient;

   [SetUp]
   public void SetUp()
   {
      _httpClientFactoryMock = new Mock<IHttpClientFactory>();
      _authenticationProviderMock = new Mock<IAuthenticationProvider>();
      _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
      _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

      _httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(_httpClient);
      _httpMessageHandlerMock.Protected().Setup("Dispose", ItExpr.IsAny<bool>());

   }

   [Test]
   public async Task SendRequestWithAuthRetry_ShouldReturnContent_WhenRequestIsSuccessful()
   {
      // Arrange
      var accessTokenRequest = new JwtAccessTokenRequest("clientId", "clientSecret", "scope");
      var expectedContent = new { Data = "test" };
      var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
      {
         Content = JsonContent.Create(expectedContent)
      };

      _httpMessageHandlerMock.SetupRequest(HttpMethod.Get, "http://test.com/", responseMessage);

      _authenticationProviderMock.Setup(provider => provider.GetAccessToken(It.IsAny<AccessTokenRequest>(), It.IsAny<bool>()))
          .ReturnsAsync("test_token");

      var client = new TestExternalApiClient(_httpClientFactoryMock.Object, _authenticationProviderMock.Object, "testClient", HeaderType.Authorization, TokenAppendType.Header);

      // Act
      var result = await client.SendRequestWithAuthRetry<TestData>(accessTokenRequest, () => _httpClient.GetAsync("http://test.com"));

      // Assert
      result.Should().BeEquivalentTo(expectedContent);
   }

   [Test]
   public void SendRequestWithAuthRetry_ShouldThrowExternalApiException_WhenRequestFails()
   {
      // Arrange
      var accessTokenRequest = new JwtAccessTokenRequest("clientId", "clientSecret", "scope");
      var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
      {
         Content = new StringContent("Bad Request")
      };

      _httpMessageHandlerMock.SetupRequest(HttpMethod.Get, "http://test.com", responseMessage);

      _authenticationProviderMock.Setup(provider => provider.GetAccessToken(It.IsAny<AccessTokenRequest>(), It.IsAny<bool>()))
          .ReturnsAsync("test_token");

      var client = new TestExternalApiClient(_httpClientFactoryMock.Object, _authenticationProviderMock.Object, "testClient", HeaderType.Authorization, TokenAppendType.Header);

      // Act
      Func<Task> act = async () => await client.SendRequestWithAuthRetry<object>(accessTokenRequest, () => _httpClient.GetAsync("http://test.com"));

      // Assert
      act.Should().ThrowAsync<ExternalApiException>().WithMessage("API operation unsuccessful, Bad Request");
   }

   [Test]
   public async Task AuthenticateHttpClient_ShouldSetAuthorizationHeader_WhenHeaderTypeIsAuthorization()
   {
      // Arrange
      var accessTokenRequest = new AccessTokenRequest();
      _authenticationProviderMock.Setup(provider => provider.GetAccessToken(It.IsAny<AccessTokenRequest>(), It.IsAny<bool>()))
          .ReturnsAsync("test_token");

      var client = new TestExternalApiClient(_httpClientFactoryMock.Object, _authenticationProviderMock.Object, "testClient", HeaderType.Authorization, TokenAppendType.Header);

      // Act
      await client.AuthenticateHttpClient(accessTokenRequest);

      // Assert
      _httpClient.DefaultRequestHeaders.Authorization.Should().NotBeNull();
      _httpClient.DefaultRequestHeaders.Authorization!.Scheme.Should().Be("Bearer");
      _httpClient.DefaultRequestHeaders.Authorization.Parameter.Should().Be("test_token");
   }

   [Test]
   public async Task AuthenticateHttpClient_ShouldSetQueryParameter_WhenTokenAppendTypeIsQuery()
   {
      // Arrange
      var accessTokenRequest = new AccessTokenRequest();
      _authenticationProviderMock.Setup(provider => provider.GetAccessToken(It.IsAny<AccessTokenRequest>(), It.IsAny<bool>()))
          .ReturnsAsync("test_token");

      var client = new TestExternalApiClient(_httpClientFactoryMock.Object, _authenticationProviderMock.Object, "testClient", HeaderType.Authorization, TokenAppendType.Query);
      _httpClient.BaseAddress = new Uri("http://test.com");

      // Act
      await client.AuthenticateHttpClient(accessTokenRequest);

      // Assert
      _httpClient.BaseAddress.Query.Should().Contain("Authorization=bearer%20test_token");
   }
   [TearDown]
   public void TearDown()
   {
      _httpClient.Dispose();
   }

   private class TestExternalApiClient : ExternalApiClient
   {
      public TestExternalApiClient(IHttpClientFactory httpClientFactory, IAuthenticationProvider authenticationProvider, string httpClientName, HeaderType headerType, TokenAppendType tokenAppendType)
          : base(httpClientFactory, authenticationProvider, httpClientName, headerType, tokenAppendType)
      {
      }

      public new async Task<T> SendRequestWithAuthRetry<T>(AccessTokenRequest accessTokenRequest, Func<Task<HttpResponseMessage>> requestFunc)
      {
         return await base.SendRequestWithAuthRetry<T>(accessTokenRequest, requestFunc);
      }

      public new async Task AuthenticateHttpClient(AccessTokenRequest accessTokenRequest, bool forceRefresh = false)
      {
         await base.AuthenticateHttpClient(accessTokenRequest, forceRefresh);
      }
   }
}

public static class HttpMessageHandlerExtensions
{
    public static Mock<HttpMessageHandler> SetupRequest(
        this Mock<HttpMessageHandler> mockHandler, HttpMethod method, string requestUri, HttpResponseMessage response)
    {
        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == method &&
                    req.RequestUri!.ToString() == requestUri &&
                    req.Headers.Authorization != null &&
                    req.Headers.Authorization.Scheme == "Bearer" && // Match the exact case
                    req.Headers.Authorization.Parameter == "test_token"),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        return mockHandler;
    }
}

public class TestData
{
   public string Data { get; set; } = null!;
}
