using Data.Models.Accounts.Schedules;
using Data.Models.Enumerations;
using LoanManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Accounts
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Settings")]
    public class sdtoSettings : sdtoBaseData
    {
        [Key]
        public long SettingsId { get; set; }

        public long? CompanyId { get; set; }

        public decimal? BankInterest { get; set; }

        public decimal?  BankCharges { get; set; }

        [ForeignKey("CompanyId")]
        public virtual sdtoCompany Company { get; set; }

        public long? AssetScheduleId { get; set; }
        [Range(1,int.MaxValue,ErrorMessage="Please select a schedule")]
        //[Association("FK_Settings_MemberSchedule", "AssetScheduleId", "ScheduleId", IsForeignKey = true)]
        [ForeignKey("AssetScheduleId")]
        public Data.Models.Accounts.Schedules.sdtoSchedule AssetSchedule { get; set; }

        //[Display(Name = "Sales Account")]
        //public long SalesAccountId { get; set; }
        //[Association("FK_Settings_SalesAccount", "SalesAccountId", "AccountHeadId", IsForeignKey = true)]
        //public sdtoAccountHead SalesAccount { get; set; }

        //[Display(Name = "Purchase Account")]
        //public long PurchaseAccountId { get; set; }
        //[Association("FK_Settings_PurchaseAccount", "PurchaseAccountId", "AccountHeadId", IsForeignKey = true)]
        //public sdtoAccountHead PurchaseAccount { get; set; }

        //[Display(Name = "Discount Account")]
        //public long DiscountAccountId { get; set; }
        //[Association("FK_Settings_DiscountAccount", "DiscountAccountId", "AccountHeadId", IsForeignKey = true)]
        //public sdtoAccountHead DiscountAccount { get; set; }

        [Display(Name = "Round Off Account")]
        [ForeignKey("RoundOffAccount")]
        public long? RoundOffAccountId { get; set; }
        //[Association("FK_Settings_RoundOffAccount", "RoundOffAccountId", "AccountHeadId", IsForeignKey = true)]
        //[InverseProperty("AccountHeadId")]
        public sdtoAccountHead RoundOffAccount { get; set; }

        [Display(Name = "Sundry Debtor")]
        [ForeignKey("SundryDebtorAccount")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Sundry Debtor")]
        public long? SundryDebtorAccountId { get; set; }
        //[Association("FK_Settings_SundryDebtorAccount", "SundryDebtorAccountId", "AccountHeadId", IsForeignKey = true)]
        //[InverseProperty("AccountHeadId")]
        public sdtoSchedule SundryDebtorAccount { get; set; }

        [Display(Name = "Sundry Creditor")]
        [ForeignKey("SundryCreditorAccount")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Sundry Creditor")]
        public long? SundryCreditorAccountId { get; set; }
        //[Association("FK_Settings_SundryCreditorAccount", "SundryCreditorAccountId", "AccountHeadId", IsForeignKey = true)]
        //[InverseProperty("AccountHeadId")]
        public sdtoSchedule SundryCreditorAccount { get; set; }

        [Display(Name = "P & L Expense Schedule")]
        [ForeignKey("PLExpenseSchedule")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a P & L Expense Schedule")]
        public long? PLExpenseScheduleId { get; set; }
        //[Association("FK_Settings_PLExpenseSchedule", "PLExpenseScheduleId", "ScheduleId", IsForeignKey = true)]
        //[InverseProperty("ScheduleId")]
        public sdtoSchedule PLExpenseSchedule { get; set; }

        [Display(Name = "P & L Income Schedule")]
        [ForeignKey("PLIncomeSchedule")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a P & L Income Schedule")]
        public long? PLIncomeScheduleId { get; set; }
        //[Association("FK_Settings_PLIncomeSchedule", "PLIncomeScheduleId", "ScheduleId", IsForeignKey = true)]
        //[InverseProperty("ScheduleId")]
        public sdtoSchedule PLIncomeSchedule { get; set; }

        [Display(Name = "Trading Expense Schedule")]
        [ForeignKey("TradingExpenseSchedule")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Trading Expense Schedule")]
        public long? TradingExpenseScheduleId { get; set; }
        //[Association("FK_Settings_TradingExpenseSchedule", "TradingExpenseScheduleId", "ScheduleId", IsForeignKey = true)]
        //[InverseProperty("ScheduleId")]
        public sdtoSchedule TradingExpenseSchedule { get; set; }

        [Display(Name = "Trading Income Schedule")]
        [ForeignKey("TradingIncomeSchedule")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Trading Income Schedule")]
        public long? TradingIncomeScheduleId { get; set; }
        //[Association("FK_Settings_TradingIncomeSchedule", "TradingIncomeScheduleId", "ScheduleId", IsForeignKey = true)]
        //[InverseProperty("ScheduleId")]
        public sdtoSchedule TradingIncomeSchedule { get; set; }

        [Display(Name = "Cash Book")]
        [ForeignKey("CashBook")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Cash Book")]
        public long? CashBookId { get; set; }
        //[Association("FK_Settings_CashBook", "CashBookId", "AccountBookId", IsForeignKey = true)]
        //[InverseProperty("AccountBookId")]
        public sdtoAccountBook CashBook { get; set; }

        [Display(Name = "Bank Book")]
        [ForeignKey("BankBook")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Bank Book")]
        public long? BankBookId { get; set; }
        //[Association("FK_Settings_BankBook", "BankBookId", "AccountBookId", IsForeignKey = true)]
        //[InverseProperty("AccountBookId")]
        public sdtoAccountBook BankBook { get; set; }

        [Display(Name = "Interest Account")]
        [ForeignKey("InterestAccount")]
        //[Range(1, int.MaxValue, ErrorMessage = "Please select a Interest Account")]
        public long? InterestAccountId { get; set; }
        //[Association("FK_Settings_InterestBook", "InterestBookId", "AccountBookId", IsForeignKey = true)]
        //[InverseProperty("AccountHeadId")]
        public sdtoAccountHead InterestAccount { get; set; }

        //[Display(Name = "Journal")]
        //public long SalesJournalId { get; set; }
        //[Association("FK_Settings_SalesJournal", "SalesJournalId", "AccountBookId", IsForeignKey = true)]
        //public sdtoAccountBook SalesJournal { get; set; }

        //[Display(Name = "Purchase Journal")]
        //public long PurchaseJournalId { get; set; }
        //[Association("FK_Settings_PurchaseJournal", "PurchaseJournalId", "AccountBookId", IsForeignKey = true)]
        //public sdtoAccountBook PurchaseJournal { get; set; }
    }
}
