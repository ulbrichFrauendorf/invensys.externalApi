using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum ReviewProcessStatus
{
   [Description("In Progress")]
   InProgress = 1,

   [Description("Closed")]
   Closed = 2,

   [Description("Captured")]
   Captured = 3
}
