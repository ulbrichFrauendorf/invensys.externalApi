using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core;
using Invensys.ExternalApi.PaySpace.Entities.Incidents;
using Invensys.ExternalApi.PaySpace.Interfaces;

namespace Invensys.ExternalApi.PaySpace.Core.Apis;

public class PaySpaceIncidentApi(IPaySpaceApiClient payspaceApiClient)
   : PaySpaceApiBase(payspaceApiClient),
      IPaySpaceIncidentApi
{
    /// <inheritdoc/>
    public async Task<List<EmployeeIncident>> EmployeeIncidentListAsync(JwtAccessTokenRequest accessTokenRequest, long companyId)
    {
        return await _payspaceApiClient.GetListAsync<EmployeeIncident>(accessTokenRequest, $"{companyId}/EmployeeIncident");
    }
}
