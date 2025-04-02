using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Entities.Components;
using Invensys.ExternalApi.PaySpace.Entities.Lookups;
using Invensys.ExternalApi.PaySpace.Interfaces;

namespace Invensys.ExternalApi.PaySpace.Core.Apis;

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
      return await _payspaceApiClient.GetListAsync<EmployeePayRate>(
         accessTokenRequest,
         $"{companyId}/EmployeePayRate/effective/{effectiveDate:yyyy-MM-dd}"
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
      return await _payspaceApiClient.GetListAsyncWithListFilter<EmployeePayRate>(
         accessTokenRequest,
         $"{companyId}/EmployeePayRate/effective/{effectiveDate:yyyy-MM-dd}",
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
      return await _payspaceApiClient.GetListAsyncWithListFilter<EmployeePensionFund>(
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
      return await _payspaceApiClient.GetListAsyncWithListFilter<EmployeeComponent>(
         accessTokenRequest,
         $"{companyId}/EmployeeComponent?frequency={companyFrequencyValue}&period={companyRunValue}",
         "EmployeeNumber",
         employeeNumbers
      );
   }
}
