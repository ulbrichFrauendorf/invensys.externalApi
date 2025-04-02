using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum AssetStatus
{
   [Description("In use")]
   InUse = 1,

   [Description("Returned")]
   Returned = 2,

   [Description("Deducted")]
   Deducted = 3,

   [Description("Exchanged")]
   Exchanged = 4,

   [Description("Refunded")]
   Refunded = 5,

   [Description("1st Issue")]
   FirstIssue = 6
}
