using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Entities.Loans;

namespace Invensys.ExternalApi.PaySpace.Interfaces;

public interface IPaySpaceLoanApi
{
   /// <summary>
   /// https://api.payspace.com/odata/v1.1/:company-id/EmployeeLoan?frequency=/:frequency&period=/:run&$filter=/:$filter
   /// </summary>
   /// <param name="token"></param>
   /// <param name="companyId"></param>
   /// <param name="companyFrequencyValue"></param>
   /// <param name="companyRunValue"></param>
   /// <param name="employeeNumbers"></param>
   /// <returns></returns>
   Task<List<EmployeeLoan>> GetEmployeeLoanAsync(
      JwtAccessTokenRequest accessTokenRequest,
      long companyId,
      string companyFrequencyValue,
      string companyRunValue,
      IEnumerable<string> employeeNumbers
   );
}
