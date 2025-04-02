using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum ItemStatus
{
   [Description("Pending Acceptance")]
   PendingAcceptance = 1,

   [Description("Accepted")]
   Accepted = 2,

   [Description("Rejected")]
   Rejected = 3
}
