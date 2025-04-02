using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum TaxableOption
{
    [Description("tax company car at 80% (default)")]
    TaxCompanyCarAt80Percent = 1,

    [Description("tax company car at 100%")]
    TaxCompanyCarAt100Percent = 2,

    [Description("tax company car at 20%")]
    TaxCompanyCarAt20Percent = 3
}