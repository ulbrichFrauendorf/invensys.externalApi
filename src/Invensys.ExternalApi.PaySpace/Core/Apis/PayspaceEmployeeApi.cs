using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Entities.FixedInfo;
using Invensys.ExternalApi.PaySpace.Interfaces;

namespace Invensys.ExternalApi.PaySpace.Core.Apis;

public class PaySpaceEmployeeApi(IPaySpaceApiClient payspaceApiClient)
   : PaySpaceApiBase(payspaceApiClient),
      IPaySpaceEmployeeApi
{
   /// <inheritdoc/>
   public async Task<Employee> EmployeeAsync(JwtAccessTokenRequest accessTokenRequest, long companyId, long employeeId)
   {
      return await _payspaceApiClient.GetAsync<Employee>(accessTokenRequest, $"{companyId}/Employee/{employeeId}");
   }

   /// <inheritdoc/>
   public async Task<List<Employee>> EmployeeListAsync(JwtAccessTokenRequest accessTokenRequest, long companyId)
   {
      return await _payspaceApiClient.GetListAsync<Employee>(accessTokenRequest, $"{companyId}/Employee");
   }

   /// <inheritdoc/>
   public async Task<List<Employee>> EmployeeListAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate
   )
   {
      return await _payspaceApiClient.GetListAsync<Employee>(
         accessTokenRequest,
         $"{companyId}/Employee/effective/{effectiveDate:yyyy-MM-dd}"
      );
   }

   /// <inheritdoc/>
   public async Task<List<Employee>> EmployeeListAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate,
      IEnumerable<string> employeeNumbers
   )
   {
      return await _payspaceApiClient.GetListAsyncWithListFilter<Employee>(
         accessTokenRequest,
         $"{companyId}/Employee/effective/{effectiveDate:yyyy-MM-dd}",
         "EmployeeNumber",
         employeeNumbers
      );
   }

   /// <inheritdoc/>
   public async Task<List<EmployeePosition>> EmployeePositionsAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate
   )
   {
      return await _payspaceApiClient.GetListAsync<EmployeePosition>(
         accessTokenRequest,
         $"{companyId}/EmployeePosition/effective/{effectiveDate:yyyy-MM-dd}"
      );
   }

   /// <inheritdoc/>
   public async Task<List<EmployeePosition>> EmployeePositionsAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate,
      IEnumerable<string> employeeNumbers
   )
   {
      return await _payspaceApiClient.GetListAsyncWithListFilter<EmployeePosition>(
         accessTokenRequest,
         $"{companyId}/EmployeePosition/effective/{effectiveDate:yyyy-MM-dd}",
         "EmployeeNumber",
         employeeNumbers
      );
   }

   /// <inheritdoc/>
   public async Task<List<EmploymentStatus>> EmployeeEmploymentStatusAllAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId
   )
   {
      return await _payspaceApiClient.GetListAsync<EmploymentStatus>(
         accessTokenRequest,
         $"{companyId}/EmployeeEmploymentStatus/all"
      );
   }

   /// <inheritdoc/>
   public async Task<List<EmploymentStatus>> EmployeeEmploymentStatusAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate
   )
   {
      return await _payspaceApiClient.GetListAsync<EmploymentStatus>(
         accessTokenRequest,
         $"{companyId}/EmployeeEmploymentStatus/effective/{effectiveDate:yyyy-MM-dd}"
      );
   }

   /// <inheritdoc/>
   public async Task<List<EmploymentStatus>> EmployeeEmploymentStatusAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate,
      IEnumerable<string> employeeNumbers
   )
   {
      return await _payspaceApiClient.GetListAsyncWithListFilter<EmploymentStatus>(
         accessTokenRequest,
         $"{companyId}/EmployeeEmploymentStatus/all",
         "EmployeeNumber",
         employeeNumbers
      );
   }

   /// <inheritdoc/>
   public async Task<List<EmployeeBankDetail>> EmployeeBankDetailAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId
   )
   {
      return await _payspaceApiClient.GetListAsync<EmployeeBankDetail>(
         accessTokenRequest,
         $"{companyId}/EmployeeBankDetail"
      );
   }

   /// <inheritdoc/>
   public async Task<List<Address>> EmployeeAddressAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      string employeeNumber
   )
   {
      return await _payspaceApiClient.GetListAsync<Address>(
         accessTokenRequest,
         $"{companyId}/EmployeeAddress/{employeeNumber}"
      );
   }
}
