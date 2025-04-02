using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum EmploymentAction
{
    [Description("New")]
    New = 0,

    [Description("terminate this employee")]
    TerminateThisEmployee = 2,

    [Description("reinstate this employee resuming this tax record")]
    ReinstateThisEmployeeResumeTaxRecord = 3,

    [Description("reinstate this employee starting a new tax record")]
    ReinstateThisEmployeeStartNewTaxRecord = 5
}