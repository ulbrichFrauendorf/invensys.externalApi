using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Entities.Company;
using Invensys.ExternalApi.PaySpace.Entities.Components;
using Invensys.ExternalApi.PaySpace.Entities.Lookups;

namespace Invensys.ExternalApi.PaySpace.Interfaces;

public interface IPaySpaceCompanyApi
{
    /// <summary>
    /// https://api.payspace.com/odata/v1.1/:company-id/OrganizationUnit?$orderby={{$organization-unit-field}}&$top={{$top}}&$skip={{$skip}}&$count={{$count}}&$filter={{$filter}}
    /// </summary>
    /// <param name="token"></param>
    /// <param name="companyId"></param>
    /// <returns></returns>
    Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(JwtAccessTokenRequest accessTokenRequest, long companyId);

    /// <summary>
    /// https://api.payspace.com/odata/v1.1/:company-id/CompanyFrequency?$top=100&$skip=0&$count=true
    /// </summary>
    /// <param name="token"></param>
    /// <param name="companyId"></param>
    /// <returns></returns>
    Task<List<CompanyFrequency>> GetCompanyFrequenciesAsync(JwtAccessTokenRequest accessTokenRequest, long companyId);

    /// <summary>
    /// https://api.payspace.com/odata/v1.1/:company-id/CompanyRun?frequency=/:FequencyName&$top=100&$skip=0&$count=true
    /// </summary>
    /// <param name="token"></param>
    /// <param name="companyId"></param>
    /// <param name="frequencyValue"></param>
    /// <returns></returns>
    Task<List<CompanyRun>> GetCompanyRunsAsync(JwtAccessTokenRequest accessTokenRequest, long companyId, string frequencyValue);

    /// <summary>
    /// https://api.payspace.com/odata/v1.1/:company-id/ComponentCompanyDetail?frequency={{frequency}}&period={{run}}&$count={{$count}}&$filter={{$filter}}
    /// </summary>
    /// <param name="token"></param>
    /// <param name="companyId"></param>
    /// <param name="companyFrequencyValue"></param>
    /// <param name="companyRunValue"></param>
    /// <returns></returns>
    Task<List<ComponentCompanyDetail>> GetComponentCompanyDetailAsync(
       JwtAccessTokenRequest accessTokenRequest,
       long companyId,
       string companyFrequencyValue,
       string companyRunValue
    );

    /// <summary>
    /// https://api.payspace.com/odata/v1.1/:company-id/CompanyRun?frequency={{frequency}}&$filter=PeriodStartDate ge {{periodStartDate}} and PeriodEndDate le {{periodEndDate}}
    /// </summary>
    /// <param name="token"></param>
    /// <param name="companyId"></param>
    /// <param name="frequencyValue"></param>
    /// <param name="periodStartDate"></param>
    /// <param name="periodEndDate"></param>
    /// <returns></returns>
    Task<List<CompanyRun>> GetCompanyRunsAsync(
       JwtAccessTokenRequest accessTokenRequest,
       long companyId,
       string frequencyValue,
       DateTime periodStartDate,
       DateTime periodEndDate
    );

    /// <summary>
    /// https://api.payspace.com/odata/v1.1/:company-id/PensionFundComponentCompany?frequency={{frequency}}&period={{run}}
    /// </summary>
    /// <param name="token"></param>
    /// <param name="companyId"></param>
    /// <param name="companyFrequencyValue"></param>
    /// <param name="companyRunValue"></param>
    /// <returns></returns>
    Task<List<PensionFundComponentCompany>> GetPensionFundComponentCompanyAsync(
       JwtAccessTokenRequest accessTokenRequest,
       long companyId,
       string companyFrequencyValue,
       string companyRunValue
    );

    /// <summary>
    /// https://api.payspace.com/odata/v1.1/:company-id/CompanySetting?$top={{$top}}&$skip={{$skip}}&$orderby={{fieldname}}&$filter={{$filter}}
    /// </summary>
    /// <param name="token"></param>
    /// <param name="companyId"></param>
    /// <returns></returns>
    Task<List<CompanySetting>> CompanySettingsAsync(JwtAccessTokenRequest accessTokenRequest, long companyId);
}
