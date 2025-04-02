using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum DependantType
{
    [Description("Adult")]
    Adult = 1,

    [Description("Child")]
    Child = 2,

    [Description("Spouse")]
    Spouse = 3
}