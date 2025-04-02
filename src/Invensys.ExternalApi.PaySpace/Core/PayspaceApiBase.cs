using Invensys.ExternalApi.PaySpace.Interfaces;

namespace Invensys.ExternalApi.PaySpace.Core;

public class PaySpaceApiBase(IPaySpaceApiClient payspaceApiClient)
{
    protected readonly IPaySpaceApiClient _payspaceApiClient = payspaceApiClient;
}
