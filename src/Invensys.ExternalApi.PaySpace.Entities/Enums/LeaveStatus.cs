using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum LeaveStatus
{
   [Description("Approved")]
   Approved = 1,

   [Description("Declined")]
   Declined = 2,

   [Description("Waiting")]
   Waiting = 3
}
