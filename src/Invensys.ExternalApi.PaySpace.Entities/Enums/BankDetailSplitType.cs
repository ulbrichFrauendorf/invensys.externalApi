using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum BankDetailSplitType
{
   [Description("Percentage")]
   Percentage = 1,

   [Description("Amount")]
   Amount = 2,

   [Description("Component")]
   Component = 3
}
