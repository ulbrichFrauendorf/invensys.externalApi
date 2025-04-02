using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum AdultRelationship
{
   [Description("(Adult Rate) - Legal Spouse")]
   LegalSpouse = 1,

   [Description("(Adult Rate) - Ex-Spouse - medically dependent on member")]
   ExSpouseMedicallyDependent = 2,

   [Description("(Adult Rate) - Full time student under 25 years")]
   FullTimeStudentUnder25 = 3,

   [Description("(Adult Rate) - Disabled Child")]
   DisabledChild = 4,

   [Description("(Principal Rate) - None of these options")]
   NoneOfTheseOptions = 5,

   [Description("Invalid")]
   Invalid = -1
}
