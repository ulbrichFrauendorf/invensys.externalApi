using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum SuspensionPaymentMethod
{
    [Description("Advised Amount")]
    AdvisedAmount = 1,

    [Description("Percentage of Package")]
    PercentageOfPackage = 2
}