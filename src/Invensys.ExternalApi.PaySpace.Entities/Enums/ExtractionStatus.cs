using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum ExtractionStatus
{
    [Description("Queued")]
    Queued = 0,

    [Description("InProgress")]
    InProgress = 1,

    [Description("Completed")]
    Completed = 2,

    [Description("Error")]
    Error = 3,

    [Description("Canceled")]
    Canceled = 4
}