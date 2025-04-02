using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum TravelBusinessUsageTaxPercentage
{
   [Description("Tax travel at 80 percent (default)")]
   TaxTravelAt80PercentDefault = 1,

   [Description("Tax travel at 20 percent")]
   TaxTravelAt20Percent = 2,

   [Description("Tax travel at 100 percent")]
   TaxTravelAt100Percent = 3
}
