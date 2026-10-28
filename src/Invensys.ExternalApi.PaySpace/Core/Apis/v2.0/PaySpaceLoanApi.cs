using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Attributes;
using Invensys.ExternalApi.PaySpace.Entities.Loans;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Invensys.ExternalApi.PaySpace.Interfaces.V2_0;

namespace Invensys.ExternalApi.PaySpace.Core.Apis.V2_0;

[PaySpaceApi("v2.0", "odata")]
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
      return await GetListAsyncWithListFilter<EmployeeLoan>(
         accessTokenRequest,
         $"{companyId}/EmployeeLoan?frequency={companyFrequencyValue}&period={companyRunValue}",
         "EmployeeNumber",
         employeeNumbers
      );
   }
}
