using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum PaySpaceLeaveType
{
    [Description("Annual")]
    Annual = 0,

    [Description("Sick")]
    Sick = 1,

    [Description("Family Responsibility")]
    FamilyResponsibility = 2,

    [Description("Study")]
    Study = 3,

    [Description("Special")]
    Special = 4
}
