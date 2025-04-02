using Invensys.ExternalApi.Sage300.Core.Models.Request;

namespace Invensys.ExternalApi.Sage300.Interfaces;

public interface ISage300ApiClient
{
   Task<List<T>> GetListAsync<T>(ResourceOwnerPasswordCredentialTokenRequest accessTokenRequest, string url);
}
