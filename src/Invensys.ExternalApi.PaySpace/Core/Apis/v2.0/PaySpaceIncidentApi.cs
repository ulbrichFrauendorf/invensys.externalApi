using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Attributes;
using Invensys.ExternalApi.PaySpace.Entities.Incidents;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Invensys.ExternalApi.PaySpace.Interfaces.V2_0;

namespace Invensys.ExternalApi.PaySpace.Core.Apis.V2_0;

[PaySpaceApi("v2.0", "odata")]
public class PaySpaceIncidentApi(IPaySpaceApiClient payspaceApiClient)
   : PaySpaceApiBase(payspaceApiClient),
      IPaySpaceIncidentApi
{
   /// <inheritdoc/>
   public async Task<List<EmployeeIncident>> EmployeeIncidentListAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId
   )
   {
      return await GetListAsync<EmployeeIncident>(accessTokenRequest, $"{companyId}/EmployeeIncident");
   }
}
