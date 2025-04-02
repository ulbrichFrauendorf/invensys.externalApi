using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Entities.Components;
using Invensys.ExternalApi.PaySpace.Entities.Lookups;

namespace Invensys.ExternalApi.PaySpace.Interfaces;

public interface IPaySpacePayrollProcessingApi
{
    /// <summary>
    /// https://api.payspace.com/odata/v1.1/:company-id/EmployeeComponent?frequency={{frequency}}&period={{run}}&$filter={{$filter}}
    /// </summary>
    /// <param name="token"></param>
    /// <param name="companyId"></param>
    /// <param name="companyFrequencyValue"></param>
    /// <param name="companyRunValue"></param>
    /// <param name="employeeNumbers"></param>
    /// <returns></returns>
    Task<List<EmployeeComponent>> GetEmployeeComponentsAsync(
       JwtAccessTokenRequest accessTokenRequest,
       long companyId,
       string companyFrequencyValue,
       string companyRunValue,
       List<string> employeeNumbers
    );

    /// <summary>
    /// https://api.payspace.com/odata/v1.1/:company-id/EmployeePayRate/effective/:effectivedate?$orderby=/:$pay-rate-field&$top=/:$top&$skip=/:$skip&$count=/:$count
    /// </summary>
    /// <param name="token"></param>
    /// <param name="companyId"></param>
    /// <param name="effectiveDate"></param>
    /// <returns></returns>
    Task<List<EmployeePayRate>> EmployeePayRateAsync(JwtAccessTokenRequest accessTokenRequest, long companyId, DateTime effectiveDate);

    /// <summary>
    /// https://api.payspace.com/odata/v1.1/:company-id/EmployeePayRate/effective/:effectivedate?$orderby=/:$pay-rate-field&$top=/:$top&$skip=/:$skip&$count=/:$count
    /// </summary>
    /// <param name="token"></param>
    /// <param name="companyId"></param>
    /// <param name="effectiveDate"></param>
    /// <param name="employeeNumbers"></param>
    /// <returns></returns>
    Task<List<EmployeePayRate>> EmployeePayRateAsync(
       JwtAccessTokenRequest accessTokenRequest,
       long companyId,
       DateTime effectiveDate,
       IEnumerable<string> employeeNumbers
    );

    /// <summary>
    /// https://api.payspace.com/odata/v1.1/:company-id/EmployeePensionFund?frequency=/:frequency&period=/:run&$filter=/:$filter
    /// </summary>
    /// <param name="token"></param>
    /// <param name="companyId"></param>
    /// <param name="companyFrequencyValue"></param>
    /// <param name="companyRunValue"></param>
    /// <param name="employeeNumbers"></param>
    /// <returns></returns>
    Task<List<EmployeePensionFund>> GetEmployeePensionFundAsync(
       JwtAccessTokenRequest accessTokenRequest,
       long companyId,
       string companyFrequencyValue,
       string companyRunValue,
       IEnumerable<string> employeeNumbers
    );
}
