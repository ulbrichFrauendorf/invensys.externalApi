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

        var expectedResponse = new CreateOrUpdateUserResult
        {
            UserId = "12345"
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
                  Content = new StringContent(
                     JsonSerializer.Serialize(expectedResponse),
                     System.Text.Encoding.UTF8,
                     "application/json")
              }
           );

        // Act
        var result = await _client.PostAsync<CreateOrUpdateUserResult, UserRegistrationRequest>(
           _accessTokenRequest,
           "api/ExternalRegistration/register",
           requestData
        );

        // Assert
        result.Should().NotBeNull();
        result.UserId.Should().Be("12345");
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

      var expectedResponse = new CreateOrUpdateUserResult
      {
         UserId = "12345"
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
                  Content = new StringContent(
                     JsonSerializer.Serialize(expectedResponse),
                     System.Text.Encoding.UTF8,
                     "application/json")
              }
           );

        // Act
        await _client.PostAsync<CreateOrUpdateUserResult, UserRegistrationRequest>(
           _accessTokenRequest,
           "api/ExternalRegistration/register",
           requestData
        );

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.Method.Should().Be(HttpMethod.Post);
        capturedRequest.Content.Should().NotBeNull();

        var contentString = await capturedRequest.Content!.ReadAsStringAsync();
        contentString.Should().NotBeNullOrEmpty();
        contentString.Should().Contain("email");
        contentString.Should().Contain("tenantIds");
        contentString.Should().Contain("firstName");
        contentString.Should().Contain("lastName");
        contentString.Should().Contain("roleClaims");
        contentString.Should().Contain("systemClaims");
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
        await _client.Invoking(c => c.PostAsync<CreateOrUpdateUserResult, UserRegistrationRequest>(
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

    private class CreateOrUpdateUserResult
   {
      public required string UserId { get; init; }
   }

}
