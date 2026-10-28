using System.Text.Json;
using Ardalis.GuardClauses;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Attributes;
using Invensys.ExternalApi.PaySpace.Entities.FixedInfo;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Invensys.ExternalApi.PaySpace.Interfaces.V2_0;

namespace Invensys.ExternalApi.PaySpace.Core.Apis.V2_0;

[PaySpaceApi("v2.0", "odata")]
public class PaySpaceEmployeeApi(IPaySpaceApiClient payspaceApiClient)
   : PaySpaceApiBase(payspaceApiClient),
      IPaySpaceEmployeeApi
{
   /// <inheritdoc/>
   public Task<EmployeeOnboardingResponse> CreateEmployeeAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      CreateEmployeeRequest createEmployeeRequest,
      CancellationToken cancellationToken = default
   ) =>
      PostAsync<EmployeeOnboardingResponse>(
         accessTokenRequest,
         EmployeeEntityPath(companyId, "Employee"),
         createEmployeeRequest,
         cancellationToken
      );

   /// <inheritdoc/>
   public Task<JsonElement> CreateEmployeeAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      JsonElement jsonElement,
      CancellationToken cancellationToken = default
   ) =>
      PostAsync<JsonElement>(
         accessTokenRequest,
         EmployeeEntityPath(companyId, "Employee"),
         jsonElement,
         cancellationToken
      );

   /// <inheritdoc/>
   public async Task<Employee> EmployeeAsync(JwtAccessTokenRequest accessTokenRequest, long companyId, long employeeId)
   {
      return await GetAsync<Employee>(accessTokenRequest, $"{companyId}/Employee/{employeeId}");
   }

   /// <inheritdoc/>
   public async Task<List<Employee>> EmployeeListAsync(JwtAccessTokenRequest accessTokenRequest, long companyId)
   {
      return await GetListAsync<Employee>(accessTokenRequest, $"{companyId}/Employee");
   }

   /// <inheritdoc/>
   public async Task<List<Employee>> EmployeeListAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate
   )
   {
      return await GetListAsync<Employee>(
         accessTokenRequest,
         $"{companyId}/Employee/effective/{PaySpaceDateHelper.FormatEffectiveDate(effectiveDate)}"
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
      return await GetListAsyncWithListFilter<Employee>(
         accessTokenRequest,
         $"{companyId}/Employee/effective/{PaySpaceDateHelper.FormatEffectiveDate(effectiveDate)}",
         "EmployeeNumber",
         employeeNumbers
      );
   }

   /// <inheritdoc/>
   public Task<PositionOnboardingResponse> CreatePositionAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      CreatePositionRequest createPositionRequest,
      CancellationToken cancellationToken = default
   ) =>
      PostAsync<PositionOnboardingResponse>(
         accessTokenRequest,
         EmployeeEntityPath(companyId, "EmployeePosition"),
         createPositionRequest,
         cancellationToken
      );

   /// <inheritdoc/>
   public Task<JsonElement> CreatePositionAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      JsonElement jsonElement,
      CancellationToken cancellationToken = default
   ) =>
      PostAsync<JsonElement>(
         accessTokenRequest,
         EmployeeEntityPath(companyId, "EmployeePosition"),
         jsonElement,
         cancellationToken
      );

   /// <inheritdoc/>
   public async Task<List<EmployeePosition>> EmployeePositionsAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate
   )
   {
      return await GetListAsync<EmployeePosition>(
         accessTokenRequest,
         $"{companyId}/EmployeePosition/effective/{PaySpaceDateHelper.FormatEffectiveDate(effectiveDate)}"
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
      return await GetListAsyncWithListFilter<EmployeePosition>(
         accessTokenRequest,
         $"{companyId}/EmployeePosition/effective/{PaySpaceDateHelper.FormatEffectiveDate(effectiveDate)}",
         "EmployeeNumber",
         employeeNumbers
      );
   }

   /// <inheritdoc/>
   public async Task<List<EmployeeRecurringCosting>> EmployeeRecurringCostingAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate
   )
   {
      return await GetListAsync<EmployeeRecurringCosting>(
         accessTokenRequest,
         $"{companyId}/EmployeeRecurringCosting/effective/{PaySpaceDateHelper.FormatEffectiveDate(effectiveDate)}"
      );
   }

   /// <inheritdoc/>
   public async Task<List<EmployeeRecurringCosting>> EmployeeRecurringCostingAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate,
      IEnumerable<string> employeeNumbers
   )
   {
      return await GetListAsyncWithListFilter<EmployeeRecurringCosting>(
         accessTokenRequest,
         $"{companyId}/EmployeeRecurringCosting/effective/{PaySpaceDateHelper.FormatEffectiveDate(effectiveDate)}",
         "EmployeeNumber",
         employeeNumbers
      );
   }

   /// <inheritdoc/>
   public async Task<List<EmployeeRecurringCostingSplit>> EmployeeRecurringCostingSplitAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate
   )
   {
      return await GetListAsync<EmployeeRecurringCostingSplit>(
         accessTokenRequest,
         $"{companyId}/EmployeeRecurringCostingSplit/effective/{PaySpaceDateHelper.FormatEffectiveDate(effectiveDate)}"
      );
   }

   /// <inheritdoc/>
   public async Task<List<EmployeeRecurringCostingSplit>> EmployeeRecurringCostingSplitAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate,
      IEnumerable<string> employeeNumbers
   )
   {
      return await GetListAsyncWithListFilter<EmployeeRecurringCostingSplit>(
         accessTokenRequest,
         $"{companyId}/EmployeeRecurringCostingSplit/effective/{PaySpaceDateHelper.FormatEffectiveDate(effectiveDate)}",
         "EmployeeNumber",
         employeeNumbers
      );
   }

   /// <inheritdoc/>
   public Task<EmploymentStatusOnboardingResponse> CreateEmploymentStatusAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      CreateEmploymentStatusRequest createEmploymentStatusRequest,
      CancellationToken cancellationToken = default
   ) =>
      PostAsync<EmploymentStatusOnboardingResponse>(
         accessTokenRequest,
         EmployeeEntityPath(companyId, "EmployeeEmploymentStatus"),
         createEmploymentStatusRequest,
         cancellationToken
      );

   /// <inheritdoc/>
   public Task<JsonElement> CreateEmploymentStatusAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      JsonElement jsonElement,
      CancellationToken cancellationToken = default
   ) =>
      PostAsync<JsonElement>(
         accessTokenRequest,
         EmployeeEntityPath(companyId, "EmployeeEmploymentStatus"),
         jsonElement,
         cancellationToken
      );

   /// <inheritdoc/>
   public async Task<List<EmploymentStatus>> EmployeeEmploymentStatusAllAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId
   )
   {
      return await GetListAsync<EmploymentStatus>(accessTokenRequest, $"{companyId}/EmployeeEmploymentStatus/all");
   }

   /// <inheritdoc/>
   public async Task<List<EmploymentStatus>> EmployeeEmploymentStatusAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate
   )
   {
      return await GetListAsync<EmploymentStatus>(
         accessTokenRequest,
         $"{companyId}/EmployeeEmploymentStatus/effective/{PaySpaceDateHelper.FormatEffectiveDate(effectiveDate)}"
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
      return await GetListAsyncWithListFilter<EmploymentStatus>(
         accessTokenRequest,
         $"{companyId}/EmployeeEmploymentStatus/effective/{PaySpaceDateHelper.FormatEffectiveDate(effectiveDate)}",
         "EmployeeNumber",
         employeeNumbers
      );
   }

   /// <inheritdoc/>
   public Task<BankDetailOnboardingResponse> CreateBankDetailAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      CreateBankDetailRequest createBankDetailRequest,
      CancellationToken cancellationToken = default
   ) =>
      PostAsync<BankDetailOnboardingResponse>(
         accessTokenRequest,
         EmployeeEntityPath(companyId, "EmployeeBankDetail"),
         createBankDetailRequest,
         cancellationToken
      );

   /// <inheritdoc/>
   public Task<JsonElement> CreateBankDetailAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      JsonElement jsonElement,
      CancellationToken cancellationToken = default
   ) =>
      PostAsync<JsonElement>(
         accessTokenRequest,
         EmployeeEntityPath(companyId, "EmployeeBankDetail"),
         jsonElement,
         cancellationToken
      );

   /// <inheritdoc/>
   public async Task<List<EmployeeBankDetail>> EmployeeBankDetailAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId
   )
   {
      return await GetListAsync<EmployeeBankDetail>(accessTokenRequest, $"{companyId}/EmployeeBankDetail");
   }

   /// <inheritdoc/>
   public async Task<List<Address>> EmployeeAddressAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      string employeeNumber
   )
   {
      return await GetListAsync<Address>(accessTokenRequest, $"{companyId}/EmployeeAddress/{employeeNumber}");
   }

   private static string EmployeeEntityPath(long companyId, string entity)
   {
      Guard.Against.OutOfRange(companyId, nameof(companyId), 1L, long.MaxValue);

      return $"{companyId}/{entity}";
   }
}
