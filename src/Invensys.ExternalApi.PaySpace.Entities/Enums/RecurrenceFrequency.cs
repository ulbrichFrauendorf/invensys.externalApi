using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum RecurrenceFrequency
{
    [Description("Daily")]
    Daily = 1,

    [Description("Weekly")]
    Weekly = 2,

    [Description("Monthly")]
    Monthly = 3,

    [Description("Yearly")]
    Yearly = 4
}