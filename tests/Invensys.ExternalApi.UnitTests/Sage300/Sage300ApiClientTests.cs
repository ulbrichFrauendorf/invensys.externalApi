using System.Net;
using System.Text.Json;
using FluentAssertions;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.Sage300.Core;
using Invensys.ExternalApi.Sage300.Core.Models.Request;
using Invensys.ExternalApi.Sage300.Interfaces;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Invensys.ExternalApi.Sage300.Tests
{
   [TestFixture]
   public class Sage300ApiClientTests
   {
      private Mock<IHttpClientFactory> _httpClientFactoryMock;
      private Mock<ISage300AuthenticationProvider> _authProviderMock;
      private Mock<HttpMessageHandler> _httpMessageHandlerMock;
      private HttpClient _httpClient;
      private Sage300ApiClient _client;
      private ResourceOwnerPasswordCredentialTokenRequest _accessTokenRequest;

      [SetUp]
      public void Setup()
      {
         _httpClientFactoryMock = new Mock<IHttpClientFactory>();
         _authProviderMock = new Mock<ISage300AuthenticationProvider>();
         _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

         _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
         {
            BaseAddress = new Uri("https://api.sage300.com/")
         };

         _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_httpClient);

         // Mock the authentication provider to return a valid token
         _authProviderMock.Setup(x => x.GetAccessToken(It.IsAny<AccessTokenRequest>(), It.IsAny<bool>()))
             .ReturnsAsync("valid-token");

         _client = new Sage300ApiClient(_httpClientFactoryMock.Object, _authProviderMock.Object);

         _accessTokenRequest = new ResourceOwnerPasswordCredentialTokenRequest
         {
            AuthorizationUrl = "https://example.com/auth",
         };
      }

      [Test]
      public async Task GetAsync_ShouldReturnDeserializedObject_WhenResponseIsSuccessful()
      {
         // Arrange
         var expectedResponse = new TestModel { Name = "Test Data", Id = 123 };
         var jsonResponse = JsonSerializer.Serialize(expectedResponse);
         var httpMessageHandler = CreateMockHandler(jsonResponse, HttpStatusCode.OK);
         var url = "https://api.sage300.com/";

         _httpClient = new HttpClient(httpMessageHandler)
         {
            BaseAddress = new Uri(url)
         };

         _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_httpClient);

         _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
         "SendAsync",
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
               ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(
               new HttpResponseMessage
               {
                  StatusCode = HttpStatusCode.OK,
                  Content = new StringContent(JsonSerializer.Serialize(expectedResponse))
               }
            );
         // Act
         var result = await _client.GetAsync<TestModel>(_accessTokenRequest, "test-endpoint");

         // Assert
         result.Should().NotBeNull();
         result.Name.Should().Be("Test Data");
         result.Id.Should().Be(123);
      }

      [Test]
      public async Task GetListAsync_ShouldReturnList_WhenResponseIsSuccessful()
      {
         // Arrange
         var expectedResponse = new Sage300Response<TestModel>
         {
            Data = new List<TestModel>
                {
                    new() { Id = 1, Name = "Item1" },
                    new() { Id = 2, Name = "Item2" }
                }
         };

         var jsonResponse = JsonSerializer.Serialize(expectedResponse);
         var httpMessageHandler = CreateMockHandler(jsonResponse, HttpStatusCode.OK);

         _httpClient = new HttpClient(httpMessageHandler)
         {
            BaseAddress = new Uri("https://api.sage300.com/")
         };

         _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_httpClient);

         _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
         "SendAsync",
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
               ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(
               new HttpResponseMessage
               {
                  StatusCode = HttpStatusCode.OK,
                  Content = new StringContent(JsonSerializer.Serialize(expectedResponse))
               }
            );
         // Act
         var result = await _client.GetListAsync<TestModel>(_accessTokenRequest, "test-endpoint");

         // Assert
         result.Should().NotBeNull();
         result.Should().HaveCount(2);
         result[0].Id.Should().Be(1);
         result[1].Name.Should().Be("Item2");
      }

      [Test]
      public async Task GetListAsync_ShouldReturnEmptyList_WhenResponseHasNoData()
      {
         // Arrange
         var emptyResponse = new Sage300Response<TestModel> { Data = null };
         var jsonResponse = JsonSerializer.Serialize(emptyResponse);
         var httpMessageHandler = CreateMockHandler(jsonResponse, HttpStatusCode.OK);

         _httpClient = new HttpClient(httpMessageHandler)
         {
            BaseAddress = new Uri("https://api.sage300.com/")
         };

         _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_httpClient);

         _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
         "SendAsync",
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
               ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(
               new HttpResponseMessage
               {
                  StatusCode = HttpStatusCode.OK,
                  Content = new StringContent(JsonSerializer.Serialize(emptyResponse))
               }
            );

         // Act
         var result = await _client.GetListAsync<TestModel>(_accessTokenRequest, "test-endpoint");

         // Assert
         result.Should().NotBeNull();
         result.Should().BeEmpty();
      }

      //Todo This is shit.
      private static HttpMessageHandler CreateMockHandler(string responseContent, HttpStatusCode statusCode)
      {
         var handlerMock = new Mock<HttpMessageHandler>();

         handlerMock
             .Protected()
             .Setup<Task<HttpResponseMessage>>(
                 "SendAsync",
                 ItExpr.IsAny<HttpRequestMessage>(),
                 ItExpr.IsAny<CancellationToken>()
             )
             .ReturnsAsync(new HttpResponseMessage
             {
                StatusCode = statusCode,
                Content = new StringContent(responseContent)
             });

         return handlerMock.Object;
      }

      [TearDown]
      public void TearDown()
      {
         _httpClient.Dispose();
      }

      private class TestModel
      {
         public int Id { get; set; }
         public string? Name { get; set; }
      }
   }
}
