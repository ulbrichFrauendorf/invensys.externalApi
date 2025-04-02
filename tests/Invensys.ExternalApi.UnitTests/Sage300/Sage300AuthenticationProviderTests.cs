using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Invensys.ExternalApi.Sage300.Core.Authentication;
using Invensys.ExternalApi.Sage300.Core.Authentication.Models.Response;
using Invensys.ExternalApi.Sage300.Core.Models.Request;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Invensys.ExternalApi.UnitTests.Sage300
{
   [TestFixture]
   public class Sage300AuthenticationProviderTests
   {
      private Mock<IHttpClientFactory> _httpClientFactoryMock;
      private Mock<HttpMessageHandler> _httpMessageHandlerMock;
      private Sage300AuthenticationProvider _provider;

      [SetUp]
      public void SetUp()
      {
         _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
         var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
         _httpClientFactoryMock = new Mock<IHttpClientFactory>();
         _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

         _provider = new Sage300AuthenticationProvider(_httpClientFactoryMock.Object);
      }

      [Test]
      public async Task GetAuthenticationResult_ShouldReturnAuthenticationResult_WhenResponseIsSuccessful()
      {
         // Arrange
         var accessTokenRequest = new ResourceOwnerPasswordCredentialTokenRequest
         {
            AuthorizationUrl = "https://example.com/auth",
         };

         var tokenResponse = new Sage300ApiTokenResponse { ExpirySeconds = 3600 };

         var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
         {
            Content = JsonContent.Create(tokenResponse)
         };
         responseMessage.Headers.Add("set-cookie", "cookieToken");

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
         result.AccessToken.Should().Be("cookieToken");
         result.AccessTokenExpiryDateTime.Should().BeCloseTo(DateTime.UtcNow.AddSeconds(3600), TimeSpan.FromSeconds(1));
      }

      [Test]
      public void GetAuthenticationResult_ShouldThrowException_WhenResponseIsUnsuccessful()
      {
         // Arrange
         var accessTokenRequest = new ResourceOwnerPasswordCredentialTokenRequest
         {
            AuthorizationUrl = "https://example.com/auth",
         };

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
         var accessTokenRequest = new ResourceOwnerPasswordCredentialTokenRequest
         {
            AuthorizationUrl = "https://example.com/auth",
         };

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
}
