using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum IncidentType
{
   [Description("DISC")]
   DISC = 1,

   [Description("APP")]
   APP = 2,

   [Description("CONS")]
   CONS = 3,

   [Description("COURT")]
   COURT = 5,

   [Description("REV")]
   REV = 7,

   [Description("GRIEV")]
   GRIEV = 8,

   [Description("DISCUS")]
   DISCUS = 9,

   [Description("OTH")]
   OTH = 1012,

   [Description("ARB")]
   ARB = 1013
}
