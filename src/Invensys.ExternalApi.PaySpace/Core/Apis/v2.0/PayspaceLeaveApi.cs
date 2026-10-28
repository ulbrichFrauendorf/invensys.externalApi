using System.Globalization;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Attributes;
using Invensys.ExternalApi.PaySpace.Entities.Leave;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Invensys.ExternalApi.PaySpace.Interfaces.V2_0;

namespace Invensys.ExternalApi.PaySpace.Core.Apis.V2_0;

[PaySpaceApi("v2.0", "odata")]
public class PaySpaceLeaveApi(IPaySpaceApiClient payspaceApiClient)
   : PaySpaceApiBase(payspaceApiClient),
      IPaySpaceLeaveApi
{
   /// <inheritdoc/>
   [Obsolete("Use the overload with year and month parameters.")]
   public async Task<List<EmployeeLeaveApplication>> EmployeeLeaveApplicationListAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId
   )
   {
      return await GetListAsync<EmployeeLeaveApplication>(accessTokenRequest, $"{companyId}/EmployeeLeaveApplication");
   }

   /// <inheritdoc/>
   public async Task<List<EmployeeLeaveApplication>> EmployeeLeaveApplicationListAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      int year,
      int month
   )
   {
      return await GetListAsync<EmployeeLeaveApplication>(
         accessTokenRequest,
         $"{companyId}/EmployeeLeaveApplication/{year}/{month}"
      );
   }

   private static string BuildODataFilter(DateTime startDate, DateTime endDate)
   {
      // Convert dates to ISO 8601 format
      var startDateString = PaySpaceDateHelper.FormatODataDate(startDate);
      var endDateString = PaySpaceDateHelper.FormatODataDate(endDate);

      // Build the OData filter string
      return $"LeaveStartDate ge {startDateString} and LeaveEndDate le {endDateString}";
   }
}
