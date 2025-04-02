using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum TrainingNotCompleteStatus
{
   [Description("Personal")]
   Personal = 1,

   [Description("Work circumstances")]
   WorkCircumstances = 2,

   [Description("Health")]
   Health = 3,

   [Description("Competence")]
   Competence = 4,

   [Description("Termination")]
   Termination = 5,

   [Description("Other")]
   Other = 6
}
