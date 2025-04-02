using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum BEELevel
{
   [Description("Junior Management")]
   JuniorManagement = 6,

   [Description("Middle Management")]
   MiddleManagement = 7,

   [Description("Skilled/Semi Skilled")]
   SkilledSemiSkilled = 8,

   [Description("Supervisory")]
   Supervisory = 9,

   [Description("Top/Senior Management")]
   TopSeniorManagement = 10,

   [Description("Unskilled")]
   Unskilled = 11,

   [Description("Executive Director")]
   ExecutiveDirector = 12,

   [Description("Non-Executive Director")]
   NonExecutiveDirector = 13,

   [Description("Other Executive Management")]
   OtherExecutiveManagement = 14
}
