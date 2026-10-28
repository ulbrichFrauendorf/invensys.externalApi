using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Attributes;
using Invensys.ExternalApi.PaySpace.Entities.Components;
using Invensys.ExternalApi.PaySpace.Entities.Lookups;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Invensys.ExternalApi.PaySpace.Interfaces.V2_0;

namespace Invensys.ExternalApi.PaySpace.Core.Apis.V2_0;

[PaySpaceApi("v2.0", "odata")]
public class PaySpacePayrollProcessingApi(IPaySpaceApiClient payspaceApiClient)
   : PaySpaceApiBase(payspaceApiClient),
      IPaySpacePayrollProcessingApi
{
   /// <inheritdoc/>
   public async Task<List<EmployeePayRate>> EmployeePayRateAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate
   )
   {
      return await GetListAsync<EmployeePayRate>(
         accessTokenRequest,
         $"{companyId}/EmployeePayRate/effective/{PaySpaceDateHelper.FormatEffectiveDate(effectiveDate)}"
      );
   }

   /// <inheritdoc/>
   public async Task<List<EmployeePayRate>> EmployeePayRateAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate,
      IEnumerable<string> employeeNumbers
   )
   {
      return await GetListAsyncWithListFilter<EmployeePayRate>(
         accessTokenRequest,
         $"{companyId}/EmployeePayRate/effective/{PaySpaceDateHelper.FormatEffectiveDate(effectiveDate)}",
         "EmployeeNumber",
         employeeNumbers
      );
   }

   /// <inheritdoc/>
   public async Task<List<EmployeePensionFund>> GetEmployeePensionFundAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      string companyFrequencyValue,
      string companyRunValue,
      IEnumerable<string> employeeNumbers
   )
   {
      return await GetListAsyncWithListFilter<EmployeePensionFund>(
         accessTokenRequest,
         $"{companyId}/EmployeePensionFund?frequency={companyFrequencyValue}&period={companyRunValue}",
         "EmployeeNumber",
         employeeNumbers
      );
   }

   /// <inheritdoc/>
   public async Task<List<EmployeeComponent>> GetEmployeeComponentsAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      string companyFrequencyValue,
      string companyRunValue,
      List<string> employeeNumbers
   )
   {
      return await GetListAsyncWithListFilter<EmployeeComponent>(
         accessTokenRequest,
         $"{companyId}/EmployeeComponent?frequency={companyFrequencyValue}&period={companyRunValue}",
         "EmployeeNumber",
         employeeNumbers
      );
   }
}
