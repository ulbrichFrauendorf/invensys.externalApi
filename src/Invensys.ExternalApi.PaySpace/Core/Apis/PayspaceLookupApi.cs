using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Entities.Lookups;
using Invensys.ExternalApi.PaySpace.Interfaces;

namespace Invensys.ExternalApi.PaySpace.Core.Apis;

public class PaySpaceLookupApi(IPaySpaceApiClient payspaceApiClient)
   : PaySpaceApiBase(payspaceApiClient),
      IPaySpaceLookupApi
{
   /// <inheritdoc/>
   public async Task<List<GenLookupValue>> GetCompanyPensionFundAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId
   )
   {
      return await _payspaceApiClient.GetListAsync<GenLookupValue>(
         accessTokenRequest,
         $"{companyId}/Lookup/CompanyPensionFund"
      );
   }

   /// <inheritdoc/>
   public async Task<List<GenLookupValue>> GetCompanyPensionFundLinkAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      string companyFrequencyValue,
      string companyRunValue,
      IEnumerable<string> lookupFilter
   )
   {
      return await _payspaceApiClient.GetListAsyncWithListFilter<GenLookupValue>(
         accessTokenRequest,
         $"{companyId}/Lookup/CompanyPensionFundLink?frequency={companyFrequencyValue}&period={companyRunValue}",
         "Value",
         lookupFilter
      );
   }

   /// <inheritdoc/>
   public async Task<List<GenLookupValue>> GetNatureOfPersonAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId
   )
   {
      return await _payspaceApiClient.GetListAsync<GenLookupValue>(
         accessTokenRequest,
         $"{companyId}/Lookup/NatureOfPerson"
      );
   }

   /// <inheritdoc/>
   public async Task<List<GenLookupValue>> GetOrganizationLevelsAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId
   )
   {
      return await _payspaceApiClient.GetListAsync<GenLookupValue>(
         accessTokenRequest,
         $"{companyId}/Lookup/OrganizationLevel"
      );
   }

   /// <inheritdoc/>
   public async Task<List<GenLookupValue>> GetOrganizationGroupsAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId
   )
   {
      return await _payspaceApiClient.GetListAsync<GenLookupValue>(
         accessTokenRequest,
         $"{companyId}/Lookup/OrganizationGroup"
      );
   }

   /// <inheritdoc/>
   public async Task<List<GenLookupValue>> GetOrganizationCategoriesAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId
   )
   {
      return await _payspaceApiClient.GetListAsync<GenLookupValue>(
         accessTokenRequest,
         $"{companyId}/Lookup/OrganizationCategory"
      );
   }
}
