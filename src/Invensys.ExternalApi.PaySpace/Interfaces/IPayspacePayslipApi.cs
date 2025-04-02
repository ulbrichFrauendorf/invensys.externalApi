using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Entities.Payslips;

namespace Invensys.ExternalApi.PaySpace.Interfaces;

public interface IPaySpacePayslipApi
{
    /// <summary>
    /// https://api.payspace.com/odata/v1.1/:company-id/EmployeePayslip/:year/:month/consolidated?$top={{$top}}&$skip={{$skip}}&$count={{$count}}&$filter={{$filter}}&componentCodes={{ComponentCode}}
    /// </summary>
    /// <param name="token"></param>
    /// <param name="companyId"></param>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="componentCodes"></param>
    /// <returns></returns>
    Task<List<EmployeePayslip>> GetConsolidatedEmployeePayslipsAsync(
       JwtAccessTokenRequest accessTokenRequest,
       long companyId,
       int year,
       int month,
       List<string>? componentCodes = null
    );

    /// <summary>
    /// https://api.payspace.com/odata/v1.1/:company-id/EmployeePayslip/:year/:month?$top=/:$top&$skip=/:$skip&$count=/:$count&$filter=/:$filter&componentCodes=/:ComponentCode
    /// </summary>
    /// </summary>
    /// <param name="token"></param>
    /// <param name="companyId"></param>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="componentCodes"></param>
    /// <returns>List of payslips</returns>
    Task<List<EmployeePayslip>> GetEmployeePayslipsAsync(
       JwtAccessTokenRequest accessTokenRequest,
       long companyId,
       int year,
       int month,
       List<string>? componentCodes = null
    );

    /// <summary>
    /// https://api.payspace.com/odata/v1.1/:company-id/EmployeePayslipLine/:year/:month?$top={{$top}}&$skip={{$skip}}&$count={{$count}}&$filter={{$filter}}
    /// </summary>
    /// <param name="token"></param>
    /// <param name="companyId"></param>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <returns></returns>
    Task<List<EmployeePayslipLine>> GetEmployeePayslipLinesAsync(JwtAccessTokenRequest accessTokenRequest, long companyId, int year, int month);
}
