using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Entities.Lookups;

namespace Invensys.ExternalApi.PaySpace.Interfaces;

public interface IPaySpaceLookupApi
{
   /// <summary>
   /// https://api.payspace.com/odata/v1.1/:company-id/CompanyPensionFund?$top=100&$skip=0&$count=true
   /// </summary>
   /// <param name="token"></param>
   /// <param name="companyId"></param>
   /// <param name="frequencyValue"></param>
   /// <returns></returns>
   Task<List<GenLookupValue>> GetCompanyPensionFundAsync(JwtAccessTokenRequest accessTokenRequest, long companyId);

   /// <summary>
   /// https://api.payspace.com/odata/v1.1/:company-id/$metadata#NatureOfPerson
   /// </summary>
   /// <param name="token"></param>
   /// <param name="companyId"></param>
   /// <returns></returns>
   Task<List<GenLookupValue>> GetNatureOfPersonAsync(JwtAccessTokenRequest accessTokenRequest, long companyId);

   /// <summary>
   /// https://api.payspace.com/odata/v1.1/:company-id/CompanyPensionFundLink?frequency={{frequency}}&period={{run}}
   /// </summary>
   /// <param name="token"></param>
   /// <param name="companyId"></param>
   /// <param name="companyFrequencyValue"></param>
   /// <param name="companyRunValue"></param>
   /// <returns></returns>
   Task<List<GenLookupValue>> GetCompanyPensionFundLinkAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      string companyFrequencyValue,
      string companyRunValue,
      IEnumerable<string> lookupFilter
   );

   /// <summary>
   /// https://api.payspace.com/odata/v1.1/:company-id/OrganizationLevel?$top=100&$skip=0&$count=true
   /// </summary>
   /// <param name="token"></param>
   /// <param name="companyId"></param>
   /// <returns></returns>
   Task<List<GenLookupValue>> GetOrganizationCategoriesAsync(JwtAccessTokenRequest accessTokenRequest, long companyId);

   /// <summary>
   /// https://api.payspace.com/odata/v1.1/:company-id/OrganizationGroup?$top=100&$skip=0&$count=true
   /// </summary>
   /// <param name="token"></param>
   /// <param name="companyId"></param>
   /// <returns></returns>
   Task<List<GenLookupValue>> GetOrganizationGroupsAsync(JwtAccessTokenRequest accessTokenRequest, long companyId);

   /// <summary>
   /// https://api.payspace.com/odata/v1.1/:company-id/OrganizationCategory?$top=100&$skip=0&$count=true
   /// </summary>
   /// <param name="token"></param>
   /// <param name="companyId"></param>
   /// <returns></returns>
   Task<List<GenLookupValue>> GetOrganizationLevelsAsync(JwtAccessTokenRequest accessTokenRequest, long companyId);
}
