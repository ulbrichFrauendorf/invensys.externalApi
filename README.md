# Invensys.ExternalApi.Common

## Overview

`Invensys.ExternalApi.Common` is a library that provides a base implementation for making authenticated HTTP requests to external APIs. It includes an `ExternalApiClient` class and an `IAuthenticationProvider` interface to handle authentication.

## Installation

To install the package, use the following command:

```bash
dotnet add package Invensys.ExternalApi.Common
```

## Usage

### Implementing the Authentication Provider

First, you need to implement the `IAuthenticationProvider` interface. This interface defines a method to obtain an access token.

```csharp
public class MyAuthenticationProvider : AuthenticationProvider<AdditionalAuthenticationResultData>(httpClientFactory),
    IAuthenticationProvider<AdditionalAuthenticationResultData[]>
{
   protected override async Task<AuthenticationResult<AdditionalAuthenticationResultData[]>> Authenticate(AccessTokenRequest accessTokenRequest)
   {
      var jwtAccessTokenRequest = AccessTokenRequestValidator.Validate<JwtAccessTokenRequest>(accessTokenRequest);

      var response = await _httpClient.PostAsync("AuthorizationUrl", jwtAccessTokenRequest);

      var myTokenResponse = await response.Content.ReadFromJsonAsync<MyApiTokenResponse>();

      var authResult = new AuthenticationResult<GroupCompany[]>
      {
         AuthenticationResultData = myTokenResponse.AdditionalAuthenticationResultData
      };

      authResult.SetAccessToken(myTokenResponse.AccessToken, myTokenResponse.ExpiresIn);

      return authResult;
   }
}
```

### Using the ExternalApiClient

Next, you can derive from use the `ExternalApiClient` to make authenticated HTTP requests. You need to pass the dotnet `IHttpClientFactory` and the `IAuthenticationProvider` as above.

```csharp
public class MyApiClient(IHttpClientFactory httpClientFactory, IMyAuthenticationProvider MyAuthenticationProvider)
   : ExternalApiClient(httpClientFactory, MyAuthenticationProvider, MyHttpClient.MyRateLimitedApi, HeaderType.Authorization, TokenAppendType.Header)
   , IMyApiClient
{ }
```


## License

This project is licensed under the MIT License.
