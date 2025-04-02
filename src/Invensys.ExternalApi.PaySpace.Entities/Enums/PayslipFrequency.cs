using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum PayslipFrequency
{
    [Description("Weekly")]
    Weekly = 1,

    [Description("Monthly")]
    Monthly = 2,

    [Description("Fortnightly")]
    Fortnightly = 3
}