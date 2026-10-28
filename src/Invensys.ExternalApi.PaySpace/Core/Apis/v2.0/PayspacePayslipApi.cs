using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Attributes;
using Invensys.ExternalApi.PaySpace.Entities.Payslips;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Invensys.ExternalApi.PaySpace.Interfaces.V2_0;

namespace Invensys.ExternalApi.PaySpace.Core.Apis.V2_0;

[PaySpaceApi("v2.0", "odata")]
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
         return await GetListAsync<EmployeePayslip>(
            accessTokenRequest,
            $"{companyId}/EmployeePayslip/{year}/{month}/consolidated?componentCodes={string.Join(',', componentCodes)}"
         );
      }

      return await GetListAsync<EmployeePayslip>(
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
      return await GetListAsync<EmployeePayslipLine>(
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
         return await GetListAsync<EmployeePayslip>(
            accessTokenRequest,
            $"{companyId}/EmployeePayslip/{year}/{month}?componentCodes={string.Join(',', componentCodes)}"
         );
      }

      return await GetListAsync<EmployeePayslip>(accessTokenRequest, $"{companyId}/EmployeePayslip/{year}/{month}");
   }
}
