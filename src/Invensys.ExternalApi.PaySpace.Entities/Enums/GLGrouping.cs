using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum GLGrouping
{
    [Description("Totals grouped by GL Number")]
    TotalsGroupedByGLNumber = 1,

    [Description("Totals grouped by GL Number and payroll component description")]
    TotalsGroupedByGLNumberAndPayrollComponentDescription = 2,

    [Description("Totals grouped Frequency, GL Number and payroll component description")]
    TotalsGroupedFrequencyGLNumberAndPayrollComponentDescription = 3,

    [Description("Totals grouped by Employee Number, GL Number and payroll component description")]
    TotalsGroupedByEmployeeNumberGLNumberAndPayrollComponentDescription = 4
}