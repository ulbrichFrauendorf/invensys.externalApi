using Invensys.ExternalApi.PaySpace.Interfaces;
using Invensys.ExternalApi.PaySpace.Interfaces.V2_0;

namespace Invensys.ExternalApi.PaySpace.Core.Context;

public interface IPaySpaceApiContext
{
   IPaySpaceCompanyApi PaySpaceCompanyApi { get; }
   IPaySpaceEmployeeApi PayspaceEmployeeApi { get; }
   IPaySpaceLeaveApi PaySpaceLeaveApi { get; }
   IPaySpaceLookupApi PaySpaceLookupApi { get; }
   IPaySpacePayrollProcessingApi PaySpacePayrollProcessingApi { get; }
   IPaySpacePayslipApi PaySpacePayslipApi { get; }
   IPaySpaceIncidentApi PaySpaceIncidentApi { get; }
   IPaySpaceLoanApi PaySpaceLoanApi { get; }
   IPaySpaceReportApi PaySpaceReportApi { get; }
}
