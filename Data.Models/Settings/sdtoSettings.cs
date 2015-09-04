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

        public float BankInterest { get; set; }

        public float BankCharges { get; set; }

        [ForeignKey("CompanyId")]
        public virtual sdtoCompany Company { get; set; }

        public long AssetScheduleId { get; set; }
        //[ForeignKey("AssetScheduleId")]
        [Association("FK_Settings_MemberSchedule", "AssetScheduleId", "ScheduleId", IsForeignKey = true)]
        public Data.Models.Accounts.Schedules.sdtoSchedule AssetSchedule { get; set; }

        [Display(Name = "Sales Account")]
        public long SalesAccountId { get; set; }
        [Association("FK_Settings_SalesAccount", "SalesAccountId", "AccountHeadId", IsForeignKey = true)]
        public sdtoAccountHead SalesAccount { get; set; }

        [Display(Name = "Purchase Account")]
        public long PurchaseAccountId { get; set; }
        [Association("FK_Settings_PurchaseAccount", "PurchaseAccountId", "AccountHeadId", IsForeignKey = true)]
        public sdtoAccountHead PurchaseAccount { get; set; }

        [Display(Name = "Discount Account")]
        public long DiscountAccountId { get; set; }
        [Association("FK_Settings_DiscountAccount", "DiscountAccountId", "AccountHeadId", IsForeignKey = true)]
        public sdtoAccountHead DiscountAccount { get; set; }

        [Display(Name = "Round Off Account")]
        public long RoundOffAccountId { get; set; }
        [Association("FK_Settings_RoundOffAccount", "RoundOffAccountId", "AccountHeadId", IsForeignKey = true)]
        public sdtoAccountHead RoundOffAccount { get; set; }

        [Display(Name = "Sundry Debtor Account")]
        public long SundryDebtorAccountId { get; set; }
        [Association("FK_Settings_SundryDebtorAccount", "SundryDebtorAccountId", "AccountHeadId", IsForeignKey = true)]
        public sdtoAccountHead SundryDebtorAccount { get; set; }

        [Display(Name = "Sundry Creditor Account")]
        public long SundryCreditorAccountId { get; set; }
        [Association("FK_Settings_SundryCreditorAccount", "SundryCreditorAccountId", "AccountHeadId", IsForeignKey = true)]
        public sdtoAccountHead SundryCreditorAccount { get; set; }

        [Display(Name = "P & L Expense Schedule")]
        public long PLExpenseScheduleId { get; set; }
        [Association("FK_Settings_PLExpenseSchedule", "PLExpenseScheduleId", "ScheduleId", IsForeignKey = true)]
        public sdtoSchedule PLExpenseSchedule { get; set; }

        [Display(Name = "P & L Income Schedule")]
        public long PLIncomeScheduleId { get; set; }
        [Association("FK_Settings_PLIncomeSchedule", "PLIncomeScheduleId", "ScheduleId", IsForeignKey = true)]
        public sdtoSchedule PLIncomeSchedule { get; set; }

        [Display(Name = "Trading Expense Schedule")]
        public long TradingExpenseScheduleId { get; set; }
        [Association("FK_Settings_TradingExpenseSchedule", "TradingExpenseScheduleId", "ScheduleId", IsForeignKey = true)]
        public sdtoSchedule TradingExpenseSchedule { get; set; }

        [Display(Name = "Trading Income Schedule")]
        public long TradingIncomeScheduleId { get; set; }
        [Association("FK_Settings_TradingIncomeSchedule", "TradingIncomeScheduleId", "ScheduleId", IsForeignKey = true)]
        public sdtoSchedule TradingIncomeSchedule { get; set; }

        [Display(Name = "Cash Book")]
        public long CashBookId { get; set; }
        [Association("FK_Settings_CashBook", "CashBookId", "AccountBookId", IsForeignKey = true)]
        public sdtoAccountBook CashBook { get; set; }

        [Display(Name = "Sales Journal")]
        public long SalesJournalId { get; set; }
        [Association("FK_Settings_SalesJournal", "SalesJournalId", "AccountBookId", IsForeignKey = true)]
        public sdtoAccountBook SalesJournal { get; set; }

        [Display(Name = "Purchase Journal")]
        public long PurchaseJournalId { get; set; }
        [Association("FK_Settings_PurchaseJournal", "PurchaseJournalId", "AccountBookId", IsForeignKey = true)]
        public sdtoAccountBook PurchaseJournal { get; set; }
    }
}
