using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum EmployeeTrainingStatus
{
    [Description("Completed")]
    Completed = 2,

    [Description("In progress")]
    InProgress = 3,

    [Description("Upcoming")]
    Upcoming = 4,

    [Description("Did not attend")]
    DidNotAttend = 5,

    [Description("Did not complete")]
    DidNotComplete = 6,

    [Description("Attended")]
    Attended = 7,

    [Description("Planned")]
    Planned = 8,

    [Description("Nominated")]
    Nominated = 9,

    [Description("Passed")]
    Passed = 10,

    [Description("Failed")]
    Failed = 11
}