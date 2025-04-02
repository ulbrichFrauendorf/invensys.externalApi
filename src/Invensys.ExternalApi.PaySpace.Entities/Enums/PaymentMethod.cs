using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum PaymentMethod
{
   [Description("Cash")]
   Cash = 1,

   [Description("Cheque")]
   Cheque = 3,

   [Description("EFT")]
   EFT = 4
}
