using System.Net;
using System.Text.Json;
using FluentAssertions;
using Invensys.ExternalApi.Common.Authentication;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.Iserve.Core;
using Invensys.ExternalApi.Iserve.Interfaces;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Invensys.ExternalApi.UnitTests.Iserve;

[TestFixture]
public class IserveApiClientTests
{
    private Mock<IHttpClientFactory> _httpClientFactoryMock;
    private Mock<IIserveAuthenticationProvider> _authProviderMock;
    private Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private HttpClient _httpClient;
    private IserveApiClient _client;
    private JwtAccessTokenRequest _accessTokenRequest;

    [SetUp]
    public void Setup()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _authProviderMock = new Mock<IIserveAuthenticationProvider>();
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://localhost:5002/")
        };

        _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_httpClient);

        // Mock the authentication provider to return a valid token
        _authProviderMock.As<IAuthenticationProvider>()
            .Setup(x => x.GetAccessToken(It.IsAny<AccessTokenRequest>(), It.IsAny<bool>()))
            .ReturnsAsync("valid-token");

        _client = new IserveApiClient(_httpClientFactoryMock.Object, _authProviderMock.Object);

        _accessTokenRequest = new JwtAccessTokenRequest(
           "a683e4dc-5254-439c-aaf7-f6d9c4530873",
           "test-secret",
           "iserve.user.registration iserve.users"
        );
    }

    [Test]
    public async Task GetAsync_ShouldReturnObject_WhenResponseIsSuccessful()
    {
        // Arrange
        var expectedData = new UserRegistrationResponse
        {
            Email = "invensys.za@gmail.com",
            FirstName = "Jane",
            LastName = "Doe"
        };

        var expectedResponse = new IserveResponse<UserRegistrationResponse>
        {
            Response = expectedData
        };

        _httpMessageHandlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
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
        var result = await _client.GetAsync<UserRegistrationResponse>(_accessTokenRequest, "api/ExternalRegistration/users/123");

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be("invensys.za@gmail.com");
        result.FirstName.Should().Be("Jane");
        result.LastName.Should().Be("Doe");
    }

    [Test]
    public async Task GetAsync_ShouldThrowException_WhenResponseIsUnsuccessful()
    {
        // Arrange
        _httpMessageHandlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
              ItExpr.IsAny<CancellationToken>()
           )
           .ReturnsAsync(
              new HttpResponseMessage
              {
                  StatusCode = HttpStatusCode.BadRequest,
                  Content = new StringContent("{\"error\": \"Invalid request\"}")
              }
           );

        // Act & Assert
        await _client.Invoking(c => c.GetAsync<UserRegistrationResponse>(_accessTokenRequest, "api/ExternalRegistration/users/123"))
           .Should().ThrowAsync<Exception>();
    }

    [Test]
    public async Task GetListAsync_ShouldReturnList_WhenResponseIsSuccessful()
    {
        // Arrange
        var expectedData = new List<UserRegistrationResponse>
      {
         new() { Email = "user1@example.com", FirstName = "John", LastName = "Doe" },
         new() { Email = "user2@example.com", FirstName = "Jane", LastName = "Smith" }
      };

        var expectedResponse = new IserveResponse<List<UserRegistrationResponse>>
        {
            Response = expectedData
        };

        _httpMessageHandlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
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
        var result = await _client.GetListAsync<UserRegistrationResponse>(_accessTokenRequest, "api/ExternalRegistration/users");

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Email.Should().Be("user1@example.com");
        result[1].FirstName.Should().Be("Jane");
    }

    [Test]
    public async Task GetListAsync_ShouldReturnEmptyList_WhenResponseHasNoData()
    {
        // Arrange
        var emptyResponse = new IserveResponse<List<UserRegistrationResponse>>
        {
            Response = []
        };

        _httpMessageHandlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
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
        var result = await _client.GetListAsync<UserRegistrationResponse>(_accessTokenRequest, "api/ExternalRegistration/users");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Test]
    public async Task PostAsync_ShouldReturnResponse_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var requestData = new UserRegistrationRequest
        {
            Email = "invensys.za@gmail.com",
            TenantIds = ["2c9d6f9b-8a61-4796-8eeb-a4962a0b3898"],
            FirstName = "Jane",
            LastName = "Doe",
            RoleClaims = ["iserve.administrator"],
            SystemClaims = ["iserve.access"]
        };

        var expectedData = new UserRegistrationResponse
        {
            Email = "invensys.za@gmail.com",
            FirstName = "Jane",
            LastName = "Doe"
        };

        var expectedResponse = new IserveResponse<UserRegistrationResponse>
        {
            Response = expectedData
        };

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
        var result = await _client.PostAsync<UserRegistrationResponse, UserRegistrationRequest>(
           _accessTokenRequest,
           "api/ExternalRegistration/register",
           requestData
        );

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be("invensys.za@gmail.com");
        result.FirstName.Should().Be("Jane");
        result.LastName.Should().Be("Doe");
    }

    [Test]
    public async Task PostAsync_ShouldIncludeRequestBody_InHttpRequest()
    {
        // Arrange
        var requestData = new UserRegistrationRequest
        {
            Email = "invensys.za@gmail.com",
            TenantIds = ["2c9d6f9b-8a61-4796-8eeb-a4962a0b3898"],
            FirstName = "Jane",
            LastName = "Doe",
            RoleClaims = ["iserve.administrator"],
            SystemClaims = ["iserve.access"]
        };

        var expectedResponse = new IserveResponse<UserRegistrationResponse>
        {
            Response = new UserRegistrationResponse
            {
                Email = "invensys.za@gmail.com",
                FirstName = "Jane",
                LastName = "Doe"
            }
        };

        HttpRequestMessage? capturedRequest = null;

        _httpMessageHandlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
           )
           .Callback<HttpRequestMessage, CancellationToken>((req, _) => capturedRequest = req)
           .ReturnsAsync(
              new HttpResponseMessage
              {
                  StatusCode = HttpStatusCode.OK,
                  Content = new StringContent(JsonSerializer.Serialize(expectedResponse))
              }
           );

        // Act
        await _client.PostAsync<UserRegistrationResponse, UserRegistrationRequest>(
           _accessTokenRequest,
           "api/ExternalRegistration/register",
           requestData
        );

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.Method.Should().Be(HttpMethod.Post);
        capturedRequest.Content.Should().NotBeNull();

        var contentString = await capturedRequest.Content!.ReadAsStringAsync();
        contentString.Should().Contain("invensys.za@gmail.com");
        contentString.Should().Contain("Jane");
        contentString.Should().Contain("Doe");
    }

    [Test]
    public async Task PostAsync_ShouldThrowException_WhenResponseIsUnsuccessful()
    {
        // Arrange
        var requestData = new UserRegistrationRequest
        {
            Email = "invensys.za@gmail.com",
            TenantIds = ["2c9d6f9b-8a61-4796-8eeb-a4962a0b3898"],
            FirstName = "Jane",
            LastName = "Doe",
            RoleClaims = ["iserve.administrator"],
            SystemClaims = ["iserve.access"]
        };

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
                  StatusCode = HttpStatusCode.BadRequest,
                  Content = new StringContent("{\"error\": \"Invalid registration data\"}")
              }
           );

        // Act & Assert
        await _client.Invoking(c => c.PostAsync<UserRegistrationResponse, UserRegistrationRequest>(
           _accessTokenRequest,
           "api/ExternalRegistration/register",
           requestData
        )).Should().ThrowAsync<Exception>();
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }

    // Test models
    private class UserRegistrationRequest
    {
        public string Email { get; set; } = string.Empty;
        public List<string> TenantIds { get; set; } = [];
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<string> RoleClaims { get; set; } = [];
        public List<string> SystemClaims { get; set; } = [];
    }

    private class UserRegistrationResponse
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
