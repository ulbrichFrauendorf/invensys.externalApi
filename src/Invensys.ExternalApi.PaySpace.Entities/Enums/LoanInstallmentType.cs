using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum LoanInstallmentType
{
   [Description("Amount")]
   Amount = 1,

   [Description("Component")]
   Component = 2
}
