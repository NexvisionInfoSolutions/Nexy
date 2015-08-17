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
    [System.ComponentModel.DataAnnotations.Schema.Table("LoanInfo")]
    public class sdtoLoanInfo : sdtoBaseData
    {
        [Key]
        public long LoanId { get; set; }
        /// <summary>
        /// User Id of the member
        /// </summary>
        [Display(Name = "User ID")]
        public long UserId { get; set; }

        [Display(Name = "Repayment Start Date")]
        public DateTime? RepaymentStartDate { get; set; }

        [Display(Name = "Repayment Interval")]
        public RepaymentInterval RePaymentInterval { get; set; }

        [Display(Name = "Requested Amount")]
        public decimal RequestedAmount { get; set; }

        [Display(Name = "Proposed Amount")]
        public decimal ProposedAmount { get; set; }

        [Display(Name = "Loan Amount")]
        public decimal LoanAmount { get; set; }

        [Display(Name = "Total Installments")]
        public int TotalInstallments { get; set; }
        public LoanStatus Status { get; set; }

        [Display(Name = "Cheque Details")]
        public string ChequeDetails { get; set; }

        [Display(Name = "Interes Rate")]
        public float InteresRate { get; set; }

        [Display(Name = "Sanctioned Date")]
        public DateTime? SanctionedDate { get; set; }

        [Display(Name = "Sanctioned By")]
        public long SanctionedBy { get; set; }
        public string Notes { get; set; }

        [ForeignKey("UserId")]
        public sdtoUser Member { get; set; }

        [Display(Name = "Installment Amount")]
        public decimal InstallmentAmount { get; set; }
    }
}
