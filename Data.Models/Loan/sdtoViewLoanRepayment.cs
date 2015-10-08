using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace LoanManagementSystem.Models.Accounts
{
    [NotMapped]
    public class sdtoViewRepaymentOfLoans
    {
        [Display(Name = "Repayment Id")]
        public long LoanRepaymentId { get; set; }

        [Display(Name = "Loan ID")]
        public long LoanId { get; set; }

        [Display(Name = "Repayment Code")]
        public string RepaymentCode { get; set; }

        [Display(Name = "Repayment Date")]
        public DateTime? RepaymentDate { get; set; }

        [Display(Name = "Principal Amount")]
        public decimal PrincipalAmount { get; set; }

        [Display(Name = "Interest Amount")]
        public decimal InterestAmount { get; set; }

        [Display(Name = "Interest Rate")]
        public float InterestRate { get; set; }

        [Display(Name = "Repayment Amount")]
        public decimal RepaymentAmount { get; set; }

        [Display(Name = "Previous Payment Due Amount")]
        public decimal PreviousPaymentDueAmount { get; set; }

        [Display(Name = "Pending Principal Amount")]
        public decimal PendingPrincipalAmount { get; set; }

        [Display(Name = "Pending Installments")]
        public int PendingInstallments { get; set; }
        public Data.Models.Enumerations.RepaymentStatus Status { get; set; }

        [Display(Name = "Payment Mode")]
        public Data.Models.Enumerations.ModeOfPayment PaymentMode { get; set; }

        [Display(Name = "Cheque Details")]
        public string ChequeDetails { get; set; }
        public string Notes { get; set; }
        public bool Checked { get; set; }
    }
}
