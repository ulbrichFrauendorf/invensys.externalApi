using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum AccountType
{
   [Description("Current/Cheques")]
   CurrentOrCheques = 1,

   [Description("Savings")]
   Savings = 2,

   [Description("Transmission")]
   Transmission = 3,

   [Description("Bond Accounts")]
   BondAccounts = 4,

   [Description("Subscription Share Accounts")]
   SubscriptionShareAccounts = 5
}
