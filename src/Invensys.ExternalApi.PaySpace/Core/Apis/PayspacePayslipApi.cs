using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Entities.Payslips;
using Invensys.ExternalApi.PaySpace.Interfaces;

namespace Invensys.ExternalApi.PaySpace.Core.Apis;

public class PaySpacePayslipApi(IPaySpaceApiClient payspaceApiClient)
   : PaySpaceApiBase(payspaceApiClient),
      IPaySpacePayslipApi
{
   /// <inheritdoc/>
   public async Task<List<EmployeePayslip>> GetConsolidatedEmployeePayslipsAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      int year,
      int month,
      List<string>? componentCodes = null
   )
   {
      if (componentCodes != null && componentCodes.Any())
      {
         return await _payspaceApiClient.GetListAsync<EmployeePayslip>(
            accessTokenRequest,
            $"{companyId}/EmployeePayslip/{year}/{month}/consolidated?componentCodes={string.Join(',', componentCodes)}"
         );
      }

      return await _payspaceApiClient.GetListAsync<EmployeePayslip>(
         accessTokenRequest,
         $"{companyId}/EmployeePayslip/{year}/{month}/consolidated"
      );
   }

   /// <inheritdoc/>
   public async Task<List<EmployeePayslipLine>> GetEmployeePayslipLinesAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      int year,
      int month
   )
   {
      return await _payspaceApiClient.GetListAsync<EmployeePayslipLine>(
         accessTokenRequest,
         $"{companyId}/EmployeePayslipLine/{year}/{month}"
      );
   }

   /// <inheritdoc/>
   public async Task<List<EmployeePayslip>> GetEmployeePayslipsAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      int year,
      int month,
      List<string>? componentCodes = null
   )
   {
      if (componentCodes != null && componentCodes.Any())
      {
         return await _payspaceApiClient.GetListAsync<EmployeePayslip>(
            accessTokenRequest,
            $"{companyId}/EmployeePayslip/{year}/{month}?componentCodes={string.Join(',', componentCodes)}"
         );
      }

      return await _payspaceApiClient.GetListAsync<EmployeePayslip>(
         accessTokenRequest,
         $"{companyId}/EmployeePayslip/{year}/{month}"
      );
   }
}
