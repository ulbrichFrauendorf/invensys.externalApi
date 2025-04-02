using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum RunStatus
{
   [Description("Closed")]
   Closed = 1,

   [Description("Open")]
   Open = 2,

   [Description("Captured")]
   Captured = 3
}
