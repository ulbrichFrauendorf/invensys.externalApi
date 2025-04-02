using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum Gender
{
   [Description("Male")]
   Male = 1,

   [Description("Female")]
   Female = 2,

   [Description("Unclassified")]
   Unclassified = 3
}
