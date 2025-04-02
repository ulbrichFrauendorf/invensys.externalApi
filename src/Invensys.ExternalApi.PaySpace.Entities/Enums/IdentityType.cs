using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum IdentityType
{
    None = 0,

    [Description("ID Number")]
    Identity = 1,

    [Description("Passport Number")]
    Passport = 2,

    [Description("Work Permit / Passport")]
    PermitOrPassport = 3
}