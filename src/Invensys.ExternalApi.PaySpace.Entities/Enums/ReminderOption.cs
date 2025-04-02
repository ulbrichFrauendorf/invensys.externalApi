using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum ReminderOption
{
    [Description("None")]
    None = 1,

    [Description("Myself")]
    Myself = 2,

    [Description("Myself and the employee's directly reports to")]
    MyselfAndEmployeesDirectReports = 3
}