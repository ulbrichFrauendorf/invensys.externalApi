using Ardalis.GuardClauses;
using FluentAssertions;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.Iserve.Core.Configuration;
using Invensys.ExternalApi.Iserve.Interfaces;
using NUnit.Framework;

namespace Invensys.ExternalApi.IntegrationTests.Iserve;

using static Testing;

[TestFixture]
public class IserveApiClientIntegrationTests : BaseTestFixture
{
   private IIserveApiClient _iserveApiClient = null!;
   private JwtAccessTokenRequest _accessTokenRequest = null!;
   private IserveTestClient _testClient = null!;
   private string _apiBaseUrl = null!;

   [SetUp]
   public new async Task TestSetUp()
   {
      await base.TestSetUp();

      _iserveApiClient = IIserveApiClient();
      _testClient = GetIserveTestClientConfig().IserveTestClients!.First();
      _accessTokenRequest = await GetIserveAuthTokenResponse();
      _apiBaseUrl = GetIserveConfig().ApiBaseUrl;
   }

   [Test]
   [Category("Integration")]
   [Category("Iserve")]
   public async Task PostAsync_ShouldRegisterExternalUser_WhenValidDataProvided()
   {
      // Arrange
      var registrationRequest = new ExternalUserRegistrationRequest
      {
         Email = $"test.user+{Guid.NewGuid():N}@invensys.co.za",
         TenantIds = [_testClient.TenantId!],
         FirstName = "Jane",
         LastName = "Doe",
         RoleClaims = ["iserve.administrator"],
         SystemClaims = ["iserve.access"]
      };

      // Act
      var result = await _iserveApiClient.PostAsync<CreateOrUpdateUserResult, ExternalUserRegistrationRequest>(
         _accessTokenRequest,
         $"{_apiBaseUrl}ExternalRegistration/register",
         registrationRequest
      );

      // Assert
      result.Should().NotBeNull();
      result.UserId.Should().NotBeEmpty();
   }

   [Test]
   [Category("Integration")]
   [Category("Iserve")]
   [Category("Validation")]
   public async Task PostAsync_ShouldThrowException_WhenRequiredFieldsMissing()
   {
      // Arrange
      var registrationRequest = new ExternalUserRegistrationRequest
      {
         Email = $"test.missing+{Guid.NewGuid():N}@invensys.co.za",
         TenantIds = [], // Empty tenant IDs
         FirstName = "",
         LastName = "",
         RoleClaims = [],
         SystemClaims = []
      };

      // Act & Assert
      await _iserveApiClient.Invoking(client => client.PostAsync<CreateOrUpdateUserResult, ExternalUserRegistrationRequest>(
         _accessTokenRequest,
         $"{_apiBaseUrl}ExternalRegistration/register",
         registrationRequest
      )).Should().ThrowAsync<Exception>();
   }
}

// DTOs for the integration tests
public class ExternalUserRegistrationRequest
{
   public string Email { get; set; } = string.Empty;
   public List<string> TenantIds { get; set; } = [];
   public string FirstName { get; set; } = string.Empty;
   public string LastName { get; set; } = string.Empty;
   public List<string> RoleClaims { get; set; } = [];
   public List<string> SystemClaims { get; set; } = [];
}

public record CreateOrUpdateUserResult
{
   public required string UserId { get; init; }
}
