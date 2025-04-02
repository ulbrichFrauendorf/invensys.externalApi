using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum PayFrequency
{
   [Description("per month")]
   PerMonth = 1,

   [Description("per week")]
   PerWeek = 2,

   [Description("per day")]
   PerDay = 3,

   [Description("per hour")]
   PerHour = 4,

   [Description("per fortnight")]
   PerFortnight = 5
}
