using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum SpreadToDateOrTaxEnd
{
   [Description("Spread up until the Bonus is paid")]
   SpreadUpUntilBonusPaid = 1,

   [Description("Spread across the whole tax year")]
   SpreadAcrossWholeTaxYear = 2
}
