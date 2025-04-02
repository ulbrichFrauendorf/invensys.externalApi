using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum WorkflowAction
{
   [Description("Approve")]
   Approve = 0,

   [Description("Reject")]
   Reject = 1
}
