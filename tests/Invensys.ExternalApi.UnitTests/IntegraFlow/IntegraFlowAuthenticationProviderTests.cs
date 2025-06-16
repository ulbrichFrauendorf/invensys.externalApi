using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using FluentAssertions;
using Invensys.ExternalApi.IntegraFlow.Core.Authentication;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using Invensys.ExternalApi.IntegraFlow.Core.Authentication.Models.Response;
using Invensys.ExternalApi.Common.Authentication.Models.Request;

namespace Invensys.ExternalApi.UnitTests.IntegraFlow;

[TestFixture]
public class IntegraFlowAuthenticationProviderTests
{
   private Mock<IHttpClientFactory> _httpClientFactoryMock;
   private Mock<HttpMessageHandler> _httpMessageHandlerMock;
   private IntegraFlowAuthenticationProvider _provider;

   [SetUp]
   public void SetUp()
   {
      _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
      var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
      _httpClientFactoryMock = new Mock<IHttpClientFactory>();
      _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

      var inMemorySettings = new Dictionary<string, string?>
         {
            { "IntegraFlowConfig:ApiBaseUrl", "https://example.com/" },
            { "IntegraFlowConfig:AuthorizationUrl", "https://example.com/auth" }
         };

      var configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

      _provider = new IntegraFlowAuthenticationProvider(_httpClientFactoryMock.Object, configuration);
   }

   [Test]
   public async Task GetAuthenticationResult_ShouldReturnAuthenticationResult_WhenResponseIsSuccessful()
   {
      // Arrange
      var accessTokenRequest = new ClientCredentialsTokenRequest("", "", "");

      var tokenResponse = new IntegraFlowAuthTokenResponse { AccessToken = "TestToken", ExpiresIn = 3600 };

      var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
      {
         Content = JsonContent.Create(tokenResponse)
      };
     

      _httpMessageHandlerMock
         .Protected()
         .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
         )
         .ReturnsAsync(responseMessage);

      // Act
      var result = await _provider.GetAuthenticationResult(accessTokenRequest);

      // Assert
      result.Should().NotBeNull();
      result.AccessToken.Should().Be("TestToken");
      result.AccessTokenExpiryDateTime.Should().BeCloseTo(DateTime.UtcNow.AddSeconds(3600), TimeSpan.FromSeconds(1));
   }

   [Test]
   public void GetAuthenticationResult_ShouldThrowException_WhenResponseIsUnsuccessful()
   {
      // Arrange
      var accessTokenRequest = new ClientCredentialsTokenRequest("", "", "");

      var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

      _httpMessageHandlerMock
         .Protected()
         .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
         )
         .ReturnsAsync(responseMessage);

      // Act
      Func<Task> act = async () => await _provider.GetAuthenticationResult(accessTokenRequest);

      // Assert
      act.Should().ThrowAsync<HttpRequestException>();
   }

   [Test]
   public void GetAuthenticationResult_ShouldThrowException_WhenTokenResponseIsNull()
   {
      // Arrange
      var accessTokenRequest = new ClientCredentialsTokenRequest("", "", "");

      var responseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) };

      _httpMessageHandlerMock
         .Protected()
         .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
         )
         .ReturnsAsync(responseMessage);

      // Act
      Func<Task> act = async () => await _provider.GetAuthenticationResult(accessTokenRequest);

      // Assert
      act.Should().ThrowAsync<ArgumentNullException>();
   }
}
