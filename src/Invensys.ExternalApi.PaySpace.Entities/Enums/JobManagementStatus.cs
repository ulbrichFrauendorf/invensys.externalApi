using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum JobManagementStatus
{
   [Description("Vacant")]
   Vacant = 0,

   [Description("Active")]
   Active = 1,

   [Description("Filled")]
   Filled = 2
}
