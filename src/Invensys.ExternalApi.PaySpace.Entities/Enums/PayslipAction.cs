using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum PayslipAction
{
    [Description("Allowances")]
    Allowances = 1,

    [Description("Company Contributions")]
    CompanyContributions = 2,

    [Description("Deductions")]
    Deductions = 3,

    [Description("Fringe Benefits")]
    FringeBenefits = 4,

    [Description("Personals")]
    Personals = 5,

    [Description("Notes")]
    Notes = 7,

    [Description("Totals")]
    Totals = 8
}