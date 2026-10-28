using System.Text.Json;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Entities.FixedInfo;

namespace Invensys.ExternalApi.PaySpace.Interfaces.V2_0;

/// <summary>Reads and creates employee, employment status, position, and banking records.</summary>
/// <remarks>
/// Create operations throw <see cref="Core.Exceptions.PaySpaceValidationException"/> when PaySpace
/// returns Success=false, including HTTP 2xx failures. Creation is not retried after transient failures.
/// Each operation is independent; failures do not roll back previously created records.
/// </remarks>
public interface IPaySpaceEmployeeApi
{
   /// <summary>Creates an employee from a typed v2 request.</summary>
   /// <param name="accessTokenRequest">PaySpace API credentials.</param>
   /// <param name="companyId">The company in which to create the employee.</param>
   /// <param name="createEmployeeRequest">The employee fields defined by PaySpace metadata.</param>
   /// <param name="cancellationToken">Cancels the request.</param>
   /// <returns>The employee record returned by PaySpace.</returns>
   Task<EmployeeOnboardingResponse> CreateEmployeeAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      CreateEmployeeRequest createEmployeeRequest,
      CancellationToken cancellationToken = default
   );

   /// <summary>Creates a biographical employee record.</summary>
   /// <param name="accessTokenRequest">PaySpace API credentials.</param>
   /// <param name="companyId">The company in which to create the employee.</param>
   /// <param name="jsonElement">A JSON object containing the employee fields.</param>
   /// <param name="cancellationToken">Cancels the request.</param>
   /// <returns>The JSON record returned by PaySpace.</returns>
   Task<JsonElement> CreateEmployeeAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      JsonElement jsonElement,
      CancellationToken cancellationToken = default
   );

   /// <summary>
   ///  https://api.payspace.com/odata/v1.1/:company-id/Employee({{EmployeeId}})
   /// </summary>
   /// <param name="token"></param>
   /// <param name="employeeId"></param>
   /// <returns>Single Employee</returns>
   Task<Employee> EmployeeAsync(JwtAccessTokenRequest accessTokenRequest, long companyId, long employeeId);

   /// <summary>
   /// Get a list of employees
   /// https://api.payspace.com/odata/v1.1/:company-id/Employee?$orderby={{$employee-field}}&$top={{$top}}&$skip={{$skip}}&$count={{$count}}&$filter={{$filter}}&$select={{$select}}
   /// </summary>
   /// <param name="token"></param>
   /// <param name="companyId"></param>
   /// <returns>List of employees</returns>
   Task<List<Employee>> EmployeeListAsync(JwtAccessTokenRequest accessTokenRequest, long companyId);

   /// <summary>
   /// Get a list of employees
   /// https://api.payspace.com/odata/v1.1/:company-id/Employee/effective/:effectivedate?$orderby={{$employee-field}}&$top={{$top}}&$skip={{$skip}}&$count={{$count}}&$filter={{$filter}}
   /// </summary>
   /// <param name="token"></param>
   /// <param name="companyId"></param>
   /// <param name="effectiveDate"></param>
   /// <returns>List of employees</returns>
   Task<List<Employee>> EmployeeListAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate
   );

   /// <summary>
   /// https://api.payspace.com/odata/v1.1/:company-id/Employee/effective/:effectivedate?$orderby={{$employee-field}}&$top={{$top}}&$skip={{$skip}}&$count={{$count}}&$filter={{$filter}}
   /// </summary>
   /// <param name="token"></param>
   /// <param name="companyId"></param>
   /// <param name="effectiveDate"></param>
   /// <param name="employeeNumbers"></param>
   /// <returns></returns>
   Task<List<Employee>> EmployeeListAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate,
      IEnumerable<string> employeeNumbers
   );

   /// <summary>Creates an employment status from a typed v2 request.</summary>
   /// <param name="accessTokenRequest">PaySpace API credentials.</param>
   /// <param name="companyId">The company containing the employee.</param>
   /// <param name="createEmploymentStatusRequest">The employment status fields and employee reference.</param>
   /// <param name="cancellationToken">Cancels the request.</param>
   /// <returns>The employment status record returned by PaySpace.</returns>
   Task<EmploymentStatusOnboardingResponse> CreateEmploymentStatusAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      CreateEmploymentStatusRequest createEmploymentStatusRequest,
      CancellationToken cancellationToken = default
   );

   /// <summary>Creates an employment status record for an employee.</summary>
   /// <param name="accessTokenRequest">PaySpace API credentials.</param>
   /// <param name="companyId">The company containing the employee.</param>
   /// <param name="jsonElement">A JSON object containing the employment status fields and employee reference.</param>
   /// <param name="cancellationToken">Cancels the request.</param>
   /// <returns>The JSON record returned by PaySpace.</returns>
   Task<JsonElement> CreateEmploymentStatusAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      JsonElement jsonElement,
      CancellationToken cancellationToken = default
   );

   /// <summary>
   /// Get a list of employee statuses
   /// https://api.payspace.com/odata/v1.1/:company-id/EmployeeEmploymentStatus/effective/:effectivedate?$orderby={{$employment-status-field}}&$top={{$top}}&$skip={{$skip}}&$count={{$count}}
   /// </summary>
   /// <param name="token"></param>
   /// <param name="companyId"></param>
   /// <param name="effectiveDate"></param>
   /// <returns>List of employee Statuses</returns>
   Task<List<EmploymentStatus>> EmployeeEmploymentStatusAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate
   );

   /// <summary>
   /// https://api.payspace.com/odata/v1.1/:company-id/EmployeeEmploymentStatus/all?$orderby={{$employment-status-field}}&$top={{$top}}&$skip={{$skip}}&$count={{$count}}&$filter={{$filter}}
   /// </summary>
   /// <param name="token"></param>
   /// <param name="companyId"></param>
   /// <param name="effectiveDate"></param>
   /// <returns></returns>
   Task<List<EmploymentStatus>> EmployeeEmploymentStatusAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate,
      IEnumerable<string> employeeNumbers
   );

   /// <summary>Creates a bank detail from a typed v2 request.</summary>
   /// <param name="accessTokenRequest">PaySpace API credentials.</param>
   /// <param name="companyId">The company containing the employee.</param>
   /// <param name="createBankDetailRequest">The bank fields and employee reference.</param>
   /// <param name="cancellationToken">Cancels the request.</param>
   /// <returns>The bank detail record returned by PaySpace.</returns>
   Task<BankDetailOnboardingResponse> CreateBankDetailAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      CreateBankDetailRequest createBankDetailRequest,
      CancellationToken cancellationToken = default
   );

   /// <summary>Creates a bank detail record for an employee.</summary>
   /// <param name="accessTokenRequest">PaySpace API credentials.</param>
   /// <param name="companyId">The company containing the employee.</param>
   /// <param name="jsonElement">A JSON object containing the bank detail fields and employee reference.</param>
   /// <param name="cancellationToken">Cancels the request.</param>
   /// <returns>The JSON record returned by PaySpace.</returns>
   Task<JsonElement> CreateBankDetailAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      JsonElement jsonElement,
      CancellationToken cancellationToken = default
   );

   /// <summary>
   /// Get a list of employee bank details
   /// https://api.payspace.com/odata/v1.1/:company-id/EmployeeBankDetail?$orderby={{$bank-detail-field}}&$top={{$top}}&$skip={{$skip}}&$count={{$count}}&$filter={{$filter}}
   /// </summary>
   /// <param name="token"></param>
   /// <param name="companyId"></param>
   /// <returns>List of employee bank details</returns>
   Task<List<EmployeeBankDetail>> EmployeeBankDetailAsync(JwtAccessTokenRequest accessTokenRequest, long companyId);

   /// <summary>Creates a position from a typed v2 request.</summary>
   /// <param name="accessTokenRequest">PaySpace API credentials.</param>
   /// <param name="companyId">The company containing the employee.</param>
   /// <param name="createPositionRequest">The position fields and employee reference.</param>
   /// <param name="cancellationToken">Cancels the request.</param>
   /// <returns>The position record returned by PaySpace.</returns>
   Task<PositionOnboardingResponse> CreatePositionAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      CreatePositionRequest createPositionRequest,
      CancellationToken cancellationToken = default
   );

   /// <summary>Creates a position record for an employee.</summary>
   /// <param name="accessTokenRequest">PaySpace API credentials.</param>
   /// <param name="companyId">The company containing the employee.</param>
   /// <param name="jsonElement">A JSON object containing the position fields and employee reference.</param>
   /// <param name="cancellationToken">Cancels the request.</param>
   /// <returns>The JSON record returned by PaySpace.</returns>
   Task<JsonElement> CreatePositionAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      JsonElement jsonElement,
      CancellationToken cancellationToken = default
   );

   /// <summary>
   ///     /// Get a list of employee positions
   /// https://api.payspace.com/odata/v1.1/:company-id/EmployeePosition/effective/:effectivedate?$orderby={{$employee-position-field}}&$top={{$top}}&$skip={{$skip}}&$count={{$count}}&$expand=OrganizationGroups
   /// </summary>
   /// <param name="token"></param>
   /// <param name="companyId"></param>
   /// <param name="effectiveDate"></param>
   /// <returns>List of Employee Positions</returns>
   Task<List<EmployeePosition>> EmployeePositionsAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate
   );

   /// <summary>
   /// https://api.payspace.com/odata/v1.1/:company-id/EmployeePosition/effective/:effectivedate?$orderby={{$employee-position-field}}&$top={{$top}}&$skip={{$skip}}&$count={{$count}}&$expand=OrganizationGroups
   /// </summary>
   /// <param name="token"></param>
   /// <param name="companyId"></param>
   /// <param name="effectiveDate"></param>
   /// <param name="employeeNumbers"></param>
   /// <returns></returns>
   Task<List<EmployeePosition>> EmployeePositionsAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate,
      IEnumerable<string> employeeNumbers
   );

   /// <summary>
   /// Gets employee recurring costing headers and split details as of the effective date. Requires API v2.0.
   /// https://api.payspace.com/odata/v2.0/:company-id/EmployeeRecurringCosting/effective/:effectivedate
   /// </summary>
   /// <param name="accessTokenRequest">The authentication request.</param>
   /// <param name="companyId">The company ID.</param>
   /// <param name="effectiveDate">The effective date, formatted in South African time.</param>
   /// <returns>Employee recurring costing records.</returns>
   Task<List<EmployeeRecurringCosting>> EmployeeRecurringCostingAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate
   );

   /// <summary>
   /// Gets employee recurring costing headers and split details for the specified employees as of the effective date.
   /// Requires API v2.0.
   /// https://api.payspace.com/odata/v2.0/:company-id/EmployeeRecurringCosting/effective/:effectivedate
   /// </summary>
   /// <param name="accessTokenRequest">The authentication request.</param>
   /// <param name="companyId">The company ID.</param>
   /// <param name="effectiveDate">The effective date, formatted in South African time.</param>
   /// <param name="employeeNumbers">The employee numbers to filter by.</param>
   /// <returns>Employee recurring costing records for the specified employees.</returns>
   Task<List<EmployeeRecurringCosting>> EmployeeRecurringCostingAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate,
      IEnumerable<string> employeeNumbers
   );

   /// <summary>
   /// Gets employee recurring costing splits as of the effective date.
   /// https://api.payspace.com/odata/v2.0/:company-id/EmployeeRecurringCostingSplit/effective/:effectivedate
   /// </summary>
   /// <param name="accessTokenRequest">The authentication request.</param>
   /// <param name="companyId">The company ID.</param>
   /// <param name="effectiveDate">The effective date, formatted in South African time.</param>
   /// <returns>Employee recurring costing split records.</returns>
   Task<List<EmployeeRecurringCostingSplit>> EmployeeRecurringCostingSplitAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate
   );

   /// <summary>
   /// Gets employee recurring costing splits for the specified employees as of the effective date.
   /// https://api.payspace.com/odata/v2.0/:company-id/EmployeeRecurringCostingSplit/effective/:effectivedate
   /// </summary>
   /// <param name="accessTokenRequest">The authentication request.</param>
   /// <param name="companyId">The company ID.</param>
   /// <param name="effectiveDate">The effective date, formatted in South African time.</param>
   /// <param name="employeeNumbers">The employee numbers to filter by.</param>
   /// <returns>Employee recurring costing split records for the specified employees.</returns>
   Task<List<EmployeeRecurringCostingSplit>> EmployeeRecurringCostingSplitAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      DateTime effectiveDate,
      IEnumerable<string> employeeNumbers
   );

   /// <summary>
   /// https://api.payspace.com/odata/v1.1/:company-id/EmployeeAddress/{{EmployeeNumber}}?$select={{$select}}
   /// </summary>
   /// <param name="token"></param>
   /// <param name="companyId"></param>
   /// <param name="employeeNumber"></param>
   /// <returns></returns>
   ///
   Task<List<Address>> EmployeeAddressAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      string employeeNumber
   );

   /// <summary>
   /// https://api.payspace.com/odata/v2.0/:company-id/EmployeeEmploymentStatus/all?$orderby={{$employment-status-field}}&$top={{$top}}&$skip={{$skip}}&$count={{$count}}&$filter={{$filter}}
   /// </summary>
   /// <param name="token"></param>
   /// <param name="companyId"></param>
   /// <returns></returns>
   Task<List<EmploymentStatus>> EmployeeEmploymentStatusAllAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId
   );
}
