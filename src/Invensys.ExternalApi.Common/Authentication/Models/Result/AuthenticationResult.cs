namespace Invensys.Api.Common.Authentication.Models.Result;

public class AuthenticationResult
{
   public string AccessToken { get; private set; } = string.Empty;

   public string RefreshToken { get; private set; } = string.Empty;

   public DateTime AccessTokenExpiryDateTime { get; private set; }

   public DateTime RefreshTokenExpiryDateTime { get; private set; }

   /// <summary>
   /// Sets the access token and its expiry time.
   /// </summary>
   /// <param name="accessToken">The access token.</param>
   /// <param name="accessTokenExpiresInSeconds">The expiry time in seconds.</param>
   public void SetAccessToken(string accessToken, int accessTokenExpiresInSeconds)
   {
      AccessToken = accessToken;
      AccessTokenExpiryDateTime = DateTime.UtcNow.AddSeconds(accessTokenExpiresInSeconds);
   }

   /// <summary>
   /// Sets the refresh token and its expiry time.
   /// </summary>
   /// <param name="refreshToken">The refresh token.</param>
   /// <param name="refreshTokenExpiresInSeconds">The expiry time in seconds.</param>
   public void SetRefreshToken(string refreshToken, int refreshTokenExpiresInSeconds)
   {
      RefreshToken = refreshToken;
      RefreshTokenExpiryDateTime = DateTime.UtcNow.AddSeconds(refreshTokenExpiresInSeconds);
   }
}

public class AuthenticationResult<T> : AuthenticationResult
{
   public T? AuthenticationResultData { get; set; }
}
