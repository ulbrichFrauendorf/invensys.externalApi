using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum BatchStatus
{
    [Description("To be Finalised")]
    ToBeFinalised = 1,

    [Description("Currently in Workflow")]
    CurrentlyInWorkflow = 2,

    [Description("Completed")]
    Completed = 3,

    [Description("Declined")]
    Declined = 4,

    [Description("Processing")]
    Processing = 5
}