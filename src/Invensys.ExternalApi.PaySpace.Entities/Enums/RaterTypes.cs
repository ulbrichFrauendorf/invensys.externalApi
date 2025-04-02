using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum RaterTypes
{
   [Description("Self")]
   Self = 1,

   [Description("Peer")]
   Peer = 2,

   [Description("Manager")]
   Manager = 3,

   [Description("Direct Report")]
   DirectReport = 4,

   [Description("Other")]
   Other = 5
}
