using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum CustomFormScreenTypes
{
    [Description("CustomForms")]
    CustomForms = 1,

    [Description("BasicProfile")]
    BasicProfile = 2,

    [Description("TaxProfile")]
    TaxProfile = 3,

    [Description("CompanyProject")]
    CompanyProject = 4,

    [Description("BasicCompany")]
    BasicCompany = 5
}