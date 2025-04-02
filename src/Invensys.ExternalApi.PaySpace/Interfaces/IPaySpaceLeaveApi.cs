using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Entities.Leave;

namespace Invensys.ExternalApi.PaySpace.Interfaces;

public interface IPaySpaceLeaveApi
{
    /// <summary>
    /// https://api.payspace.com/odata/v1.1/:company-id/EmployeeLeaveApplication?$top={{$top}}&$skip={{$skip}}&$count={{$count}}&$filter={{$filter}}
    /// </summary>
    /// <param name="token"></param>
    /// <param name="companyId"></param>
    /// <param name="employeeNumbers"></param>
    /// <returns></returns>
    Task<List<EmployeeLeaveApplication>> EmployeeLeaveApplicationListAsync(JwtAccessTokenRequest accessTokenRequest, long companyId);

    /// <summary>
    /// https://api.payspace.com/odata/v1.1/:company-id/EmployeeLeaveApplication/:year/:month?$top={{$top}}&$skip={{$skip}}&$count={{$count}}&$filter={{$filter}}
    /// </summary>
    /// <param name="token"></param>
    /// <param name="companyId"></param>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="employeeNumbers"></param>
    /// <returns></returns>
    Task<List<EmployeeLeaveApplication>> EmployeeLeaveApplicationListAsync(
       JwtAccessTokenRequest accessTokenRequest,
       long companyId,
       int year,
       int month
    );
}
