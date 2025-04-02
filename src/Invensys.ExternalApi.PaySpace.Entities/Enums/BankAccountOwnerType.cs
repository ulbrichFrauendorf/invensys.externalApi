using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum BankAccountOwnerType
{
   [Description("Own")]
   Own = 1,

   [Description("Joint")]
   Joint = 2,

   [Description("Third Party")]
   ThirdParty = 3
}
