using System.Net;
using FluentAssertions;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.Odata;
using Invensys.ExternalApi.PaySpace.Core;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Text.Json;

namespace Invensys.ExternalApi.UnitTests.PaySpace
{
    [TestFixture]
    public class PayspaceApiClientTests
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private Mock<IPaySpaceAuthenticationProvider> _paySpaceAuthenticationProviderMock;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpClient _httpClient;
        private IPaySpaceApiClient _payspaceApiClient;
        private JwtAccessTokenRequest _accessTokenRequest;

        [SetUp]
        public void SetUp()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _paySpaceAuthenticationProviderMock = new Mock<IPaySpaceAuthenticationProvider>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            _httpClientFactoryMock
                .Setup(factory => factory.CreateClient(It.IsAny<string>()))
                .Returns(_httpClient);

            _payspaceApiClient = new PaySpaceApiClient(_httpClientFactoryMock.Object, _paySpaceAuthenticationProviderMock.Object);
            _accessTokenRequest = new JwtAccessTokenRequest("clientId", "clientSecret", "scope");
        }

        private class Data
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        [Test]
        public async Task GetAsync_ShouldReturnData()
        {
            var url = "https://api.payspace.com/data";
            var expectedData = new Data{ Id = 1, Name = "Test" };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri!.ToString() == url),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(expectedData))
                });

            var result = await _payspaceApiClient.GetAsync<Data>(_accessTokenRequest, url);

            result.Should().BeEquivalentTo(expectedData);
        }

        [Test]
        public async Task GetListAsync_ShouldReturnData()
        {
            var url = "https://api.payspace.com/data";
            var expectedData = new List<Data> { new() { Id = 1, Name = "Test" } };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri!.ToString() == url),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(new { value = expectedData }))
                });

            var result = await _payspaceApiClient.GetListAsync<Data>(_accessTokenRequest, url);

            result.Should().BeEquivalentTo(expectedData);
        }

        [Test]
        public async Task GetAllPagesWithAuthRetry_ShouldReturnAllPagesData()
        {
            var url = "https://api.payspace.com/data";
            var expectedData = new List<Data> { new() { Id = 1, Name = "Test" }, new() { Id = 2, Name = "Test2" } };

            _httpMessageHandlerMock
                .Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(new ListResponse<Data> { Value = new List<Data> { expectedData[0] }, OdataNextLink = new Uri("https://api.payspace.com/data?page=2") }))
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(new { value = new List<Data> { expectedData[1] } }))
                });

            var result = await _payspaceApiClient.GetListAsync<Data>(_accessTokenRequest, url);

            result.Should().BeEquivalentTo(expectedData);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }
    }
}
