using System.Globalization;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Entities.Leave;
using Invensys.ExternalApi.PaySpace.Interfaces;

namespace Invensys.ExternalApi.PaySpace.Core.Apis;

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
      return await _payspaceApiClient.GetListAsync<EmployeeLeaveApplication>(
         accessTokenRequest,
         $"{companyId}/EmployeeLeaveApplication"
      );
   }

   /// <inheritdoc/>
   public async Task<List<EmployeeLeaveApplication>> EmployeeLeaveApplicationListAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      int year,
      int month
   )
   {
      return await _payspaceApiClient.GetListAsync<EmployeeLeaveApplication>(
         accessTokenRequest,
         $"{companyId}/EmployeeLeaveApplication/{year}/{month}"
      );
   }

   private static string BuildODataFilter(DateTime startDate, DateTime endDate)
   {
      // Convert dates to ISO 8601 format
      var startDateString = startDate.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
      var endDateString = endDate.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);

      // Build the OData filter string
      return $"LeaveStartDate ge {startDateString} and LeaveEndDate le {endDateString}";
   }
}
