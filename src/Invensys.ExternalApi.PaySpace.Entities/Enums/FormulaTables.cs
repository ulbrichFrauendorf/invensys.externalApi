using System.ComponentModel;

namespace Invensys.ExternalApi.PaySpace.Entities.Enums;

public enum FormulaTables
{
   [Description("Pension")]
   Pension = 1,

   [Description("RSC")]
   RSC = 2,

   [Description("Union")]
   Union = 3,

   [Description("Medical")]
   Medical = 4,

   [Description("Disability")]
   Disability = 5,

   [Description("GroupLife")]
   GroupLife = 6,

   [Description("RetirementAnnuity")]
   RetirementAnnuity = 7,

   [Description("IncomeProtection")]
   IncomeProtection = 8,

   [Description("Loan")]
   Loan = 9,

   [Description("Savings")]
   Savings = 10,

   [Description("CompanyCar")]
   CompanyCar = 11,

   [Description("CompanyCarVAT")]
   CompanyCarVAT = 12,

   [Description("BCOE")]
   BCOE = 13,

   [Description("BonusProvision")]
   BonusProvision = 14,

   [Description("Garnishee")]
   Garnishee = 15,

   [Description("LeaveAdjustment")]
   LeaveAdjustment = 16,

   [Description("LeaveTrigger")]
   LeaveTrigger = 17,

   [Description("Travel")]
   Travel = 18,

   [Description("EmploymentTaxIncentive")]
   EmploymentTaxIncentive = 19,

   [Description("FinancialHousePayments")]
   FinancialHousePayments = 20,

   [Description("House")]
   House = 21,

   [Description("MultiContractWork")]
   MultiContractWork = 22,

   [Description("DSRCalculation")]
   DSRCalculation = 23,

   [Description("Alimony")]
   Alimony = 24,

   [Description("BrazilLeave")]
   BrazilLeave = 25
}
