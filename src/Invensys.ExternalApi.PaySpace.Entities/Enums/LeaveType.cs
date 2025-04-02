using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum LeaveType
{
    [Description("Annual")]
    Annual = 1,

    [Description("Sick")]
    Sick = 2,

    [Description("Family Responsibility")]
    FamilyResponsibility = 3,

    [Description("Study")]
    Study = 4,

    [Description("Special")]
    Special = 5
}