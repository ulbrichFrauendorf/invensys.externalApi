using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum RunType
{
   [Description("Main Run")]
   MainRun = 1,

   [Description("Interim Run")]
   InterimRun = 2
}
