using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Entities.Loans;
using Invensys.ExternalApi.PaySpace.Interfaces;

namespace Invensys.ExternalApi.PaySpace.Core.Apis;

public class PaySpaceLoanApi(IPaySpaceApiClient payspaceApiClient)
   : PaySpaceApiBase(payspaceApiClient),
      IPaySpaceLoanApi
{
   /// <inheritdoc/>
   public async Task<List<EmployeeLoan>> GetEmployeeLoanAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      string companyFrequencyValue,
      string companyRunValue,
      IEnumerable<string> employeeNumbers
   )
   {
      return await _payspaceApiClient.GetListAsyncWithListFilter<EmployeeLoan>(
         accessTokenRequest,
         $"{companyId}/EmployeeLoan?frequency={companyFrequencyValue}&period={companyRunValue}",
         "EmployeeNumber",
         employeeNumbers
      );
   }
}
