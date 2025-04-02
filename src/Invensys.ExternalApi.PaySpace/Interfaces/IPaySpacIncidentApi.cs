using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Entities.Incidents;

namespace Invensys.ExternalApi.PaySpace.Interfaces;

public interface IPaySpaceIncidentApi
{
    /// <summary>
    /// https://api.payspace.com/odata/v1.1/:company-id/EmployeeIncident?$top={{$top}}&$skip={{$skip}}&$count={{$count}}&$filter={{$filter}}
    /// </summary>
    /// <param name="token"></param>
    /// <param name="companyId"></param>
    /// <returns></returns>
    Task<List<EmployeeIncident>> EmployeeIncidentListAsync(JwtAccessTokenRequest accessTokenRequest, long companyId);
}
