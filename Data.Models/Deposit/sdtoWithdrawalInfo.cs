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
    [System.ComponentModel.DataAnnotations.Schema.Table("WithdrawalInfo")]
    public class sdtoWithdrawalInfo : sdtoBaseData
    {
        [Key]
        public long WithdrawalId { get; set; }

        [Display(Name = "Deposit Id")]
        public long DepositId { get; set; }

        [Display(Name = "Withdrawal Code")]
        public string WithdrawalCode { get; set; }

        [Display(Name = "Withdrawal Amount")]
        public decimal WithdrawalAmount { get; set; }

        [Display(Name = "Interest Amount")]
        public decimal InterestAmount { get; set; }

        [Display(Name = "Interest Rate")]
        public float InterestRate { get; set; }       

        [Display(Name = "Balance Deposit Amount")]
        public decimal BalanceDepositAmount { get; set; }

        [Display(Name = "Interest Rate for Balance Amount")]
        public float NewInterestRate { get; set; }

        public WithdrawalStatus Status { get; set; }

        [Display(Name = "Payment Mode")]
        public ModeOfPayment PaymentMode { get; set; }

        [Display(Name = "Cheque Details")]
        public string ChequeDetails { get; set; }
        public string Notes { get; set; }

        [ForeignKey("DepositId")]
        public sdtoDepositInfo Deposit { get; set; }
    }
}
