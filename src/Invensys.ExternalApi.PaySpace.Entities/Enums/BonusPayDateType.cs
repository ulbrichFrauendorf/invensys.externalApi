using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum BonusPayDateType
{
    [Description("Anniversary")]
    Anniversary = 1,

    [Description("Birth date")]
    BirthDate = 2,

    [Description("Specify a day and month")]
    SpecifyDayAndMonth = 3
}