using Invensys.ExternalApi.Sage300.Core.Models.Request;
using static Invensys.ExternalApi.IntegrationTests.Testing;

namespace Invensys.ExternalApi.IntegrationTests.Sage300;

internal class Sage300ApiTest : BaseTestFixture
{
   [Test]
   public async Task ShouldReturnAuthenticationResultAuthorized()
   {
      var tokenResponse = await GetSage300AuthTokenResponse(new ResourceOwnerPasswordCredentialTokenRequest
      {
         AuthorizationUrl = "https://online.sage.co.za/S05878/token",
         Scope = "apiKey=4a3918da-9135-4b9e-8304-b0a11c41acda",
      });

      tokenResponse.Should().NotBeNull();
      tokenResponse.AccessToken.Should().NotBeEmpty();
      tokenResponse.AccessTokenExpiryDateTime.Should().BeLessThan(new TimeSpan(10));
   }

   [Test]
   public async Task ShouldReturnEmployeeInfoList()
   {
      var sage300Api = ISage300ApiClient();

      var list = await sage300Api.GetListAsync<object>(new ResourceOwnerPasswordCredentialTokenRequest
      {
         AuthorizationUrl = "https://online.sage.co.za/S05878/token",
         Scope = "apiKey=4a3918da-9135-4b9e-8304-b0a11c41acda",
      }, "https://online.sage.co.za/S05878/api/apibase/GenericGet/EMP_INFO");

      list.Should().NotBeNull();
      list.Should().NotBeEmpty();
      list.Should().HaveCountGreaterThanOrEqualTo(10);
   }
}
