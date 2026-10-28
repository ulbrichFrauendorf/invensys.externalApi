using Invensys.ExternalApi.PaySpace.Interfaces;
using Invensys.ExternalApi.PaySpace.Interfaces.V2_0;

namespace Invensys.ExternalApi.PaySpace.Core.Context;

public class PaySpaceApiContext(
   IPaySpaceCompanyApi paySpaceCompanyApi,
   IPaySpaceEmployeeApi payspaceEmployeeApi,
   IPaySpaceLeaveApi paySpaceLeaveApi,
   IPaySpaceLookupApi paySpaceLookupApi,
   IPaySpacePayrollProcessingApi paySpacePayrollProcessingApi,
   IPaySpacePayslipApi paySpacePayslipApi,
   IPaySpaceIncidentApi paySpaceIncidentApi,
   IPaySpaceLoanApi paySpaceLoanApi,
   IPaySpaceReportApi paySpaceReportApi
) : IPaySpaceApiContext
{
   public IPaySpaceReportApi PaySpaceReportApi { get; } = paySpaceReportApi;

   public IPaySpaceCompanyApi PaySpaceCompanyApi { get; } = paySpaceCompanyApi;
   public IPaySpaceEmployeeApi PayspaceEmployeeApi { get; } = payspaceEmployeeApi;
   public IPaySpaceLeaveApi PaySpaceLeaveApi { get; } = paySpaceLeaveApi;
   public IPaySpaceLookupApi PaySpaceLookupApi { get; } = paySpaceLookupApi;
   public IPaySpacePayrollProcessingApi PaySpacePayrollProcessingApi { get; } = paySpacePayrollProcessingApi;
   public IPaySpacePayslipApi PaySpacePayslipApi { get; } = paySpacePayslipApi;
   public IPaySpaceIncidentApi PaySpaceIncidentApi { get; } = paySpaceIncidentApi;
   public IPaySpaceLoanApi PaySpaceLoanApi { get; } = paySpaceLoanApi;
}
