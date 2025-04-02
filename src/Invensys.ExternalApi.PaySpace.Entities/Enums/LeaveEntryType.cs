using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum LeaveEntryType
{
   [Description("Leave Application")]
   LeaveApplication = 2,

   [Description("Cancellation")]
   Cancellation = 3
}
