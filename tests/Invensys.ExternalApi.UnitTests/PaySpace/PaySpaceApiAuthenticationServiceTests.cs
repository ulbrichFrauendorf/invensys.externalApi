using FluentAssertions;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.Common.Authentication.Models.Result;
using Invensys.ExternalApi.PaySpace.Core.Authentication;
using Invensys.ExternalApi.PaySpace.Entities.Company;
using Invensys.ExternalApi.PaySpace.Entities.Lookups;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Moq;
using NUnit.Framework;

namespace Invensys.ExternalApi.UnitTests.PaySpace
{
   public class PaySpaceApiAuthenticationServiceTests
   {
      private Mock<IPaySpaceAuthenticationProvider> _payspaceAuthenticationProviderMock;
      private Mock<IPaySpaceLookupApi> _payspaceLookupApiMock;
      private PaySpaceApiAuthenticationService _service;

      [SetUp]
      public void SetUp()
      {
         _payspaceAuthenticationProviderMock = new Mock<IPaySpaceAuthenticationProvider>();
         _payspaceLookupApiMock = new Mock<IPaySpaceLookupApi>();
         _service = new PaySpaceApiAuthenticationService(
            _payspaceAuthenticationProviderMock.Object,
            _payspaceLookupApiMock.Object
         );
      }

      [Test]
      public async Task Authenticate_ShouldReturnAuthorizationResponse_WhenRequestIsValid()
      {
         // Arrange
         var accessTokenRequest = new JwtAccessTokenRequest("clientId", "clientSecret", "scope");
         var groupCompanies = new[]
         {
            new GroupCompany
            {
               Companies = new[]
               {
                  new Company { CompanyId = 1, CompanyName = "Company1" }
               }
            }
         };
         var authenticationResult = new AuthenticationResult<GroupCompany[]>
         {
            AuthenticationResultData = groupCompanies
         };

         _payspaceAuthenticationProviderMock
            .Setup(x => x.GetAuthenticationResult(accessTokenRequest, false))
            .ReturnsAsync(authenticationResult);

         _payspaceAuthenticationProviderMock
            .Setup(x => x.GetAccessToken(accessTokenRequest, false))
            .ReturnsAsync("accessToken");

         _payspaceLookupApiMock
            .Setup(x => x.GetOrganizationLevelsAsync(accessTokenRequest, 1))
            .ReturnsAsync(new List<GenLookupValue>());

         // Act
         var result = await _service.Authenticate(accessTokenRequest);

         // Assert
         result.Should().NotBeNull();
         result.AccessToken.Should().Be("accessToken");
         result.Companies.Should().HaveCount(1);
         result.Companies.First().CompanyId.Should().Be(1);
      }

      [Test]
      public void Authenticate_ShouldThrowException_WhenGroupsAreNull()
      {
         // Arrange
         var accessTokenRequest = new JwtAccessTokenRequest("clientId", "clientSecret", "scope");
         var authenticationResult = new AuthenticationResult<GroupCompany[]> { AuthenticationResultData = null };

         _payspaceAuthenticationProviderMock
            .Setup(x => x.GetAuthenticationResult(accessTokenRequest, false))
            .ReturnsAsync(authenticationResult);

         // Act
         Func<Task> act = async () => await _service.Authenticate(accessTokenRequest);

         // Assert
         act.Should().ThrowAsync<ArgumentNullException>();
      }

      [Test]
      public void Authenticate_ShouldThrowException_WhenCompanyIdIsNull()
      {
         // Arrange
         var accessTokenRequest = new JwtAccessTokenRequest("clientId", "clientSecret", "scope");
         var groupCompanies = new[] { new GroupCompany { Companies = new Company[0] } };
         var authenticationResult = new AuthenticationResult<GroupCompany[]>
         {
            AuthenticationResultData = groupCompanies
         };

         _payspaceAuthenticationProviderMock
            .Setup(x => x.GetAuthenticationResult(accessTokenRequest, false))
            .ReturnsAsync(authenticationResult);

         // Act
         Func<Task> act = async () => await _service.Authenticate(accessTokenRequest);

         // Assert
         act.Should().ThrowAsync<ArgumentNullException>();
      }
   }
}
