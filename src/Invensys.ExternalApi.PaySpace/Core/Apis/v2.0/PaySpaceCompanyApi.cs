using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Attributes;
using Invensys.ExternalApi.PaySpace.Entities.Company;
using Invensys.ExternalApi.PaySpace.Entities.Components;
using Invensys.ExternalApi.PaySpace.Entities.Lookups;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Invensys.ExternalApi.PaySpace.Interfaces.V2_0;

namespace Invensys.ExternalApi.PaySpace.Core.Apis.V2_0;

[PaySpaceApi("v2.0", "odata")]
public class PaySpaceCompanyApi(IPaySpaceApiClient paySpaceApiClient)
   : PaySpaceApiBase(paySpaceApiClient),
      IPaySpaceCompanyApi
{
   /// <inheritdoc/>
   public async Task<List<CompanyFrequency>> GetCompanyFrequenciesAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId
   )
   {
      return await GetListAsync<CompanyFrequency>(accessTokenRequest, $"{companyId}/Lookup/CompanyFrequency");
   }

   /// <inheritdoc/>
   public async Task<List<PensionFundComponentCompany>> GetPensionFundComponentCompanyAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      string companyFrequencyValue,
      string companyRunValue
   )
   {
      return await GetListAsync<PensionFundComponentCompany>(
         accessTokenRequest,
         $"{companyId}/Lookup/PensionFundComponentCompany?frequency={companyFrequencyValue}&period={companyRunValue}"
      );
   }

   /// <inheritdoc/>
   public async Task<List<CompanyRun>> GetCompanyRunsAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      string frequencyValue
   )
   {
      return await GetListAsync<CompanyRun>(
         accessTokenRequest,
         $"{companyId}/Lookup/CompanyRun?frequency={frequencyValue}"
      );
   }

   /// <inheritdoc/>
   public async Task<List<CompanyRun>> GetCompanyRunsAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      string frequencyValue,
      DateTime periodStartDate,
      DateTime periodEndDate
   )
   {
      var url =
         $"{companyId}/Lookup/CompanyRun?frequency={frequencyValue}&$filter=PeriodStartDate ge {PaySpaceDateHelper.FormatODataDate(periodStartDate)} and PeriodEndDate le {PaySpaceDateHelper.FormatODataDate(periodEndDate)}";
      return await GetListAsync<CompanyRun>(accessTokenRequest, url);
   }

   /// <inheritdoc/>
   public async Task<List<ComponentCompanyDetail>> GetComponentCompanyDetailAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      string companyFrequencyValue,
      string companyRunValue
   )
   {
      return await GetListAsync<ComponentCompanyDetail>(
         accessTokenRequest,
         $"{companyId}/Lookup/ComponentCompanyDetail?frequency={companyFrequencyValue}&period={companyRunValue}"
      );
   }

   /// <inheritdoc/>
   public async Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId
   )
   {
      return await GetListAsync<OrganizationUnit>(accessTokenRequest, $"{companyId}/OrganizationUnit");
   }

   /// <inheritdoc/>
   public async Task<List<CompanySetting>> CompanySettingsAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId
   )
   {
      return await GetListAsync<CompanySetting>(accessTokenRequest, $"{companyId}/CompanySetting");
   }
}
