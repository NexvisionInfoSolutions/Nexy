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
    [System.ComponentModel.DataAnnotations.Schema.Table("LoanRepayment")]
    public class sdtoLoanRepayment : sdtoBaseData
    {
        [Key]
        public long LoanRepaymentId { get; set; }
        public long LoanId { get; set; }
        public string RepaymentCode { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public float InterestRate { get; set; }
        public decimal RepaymentAmount { get; set; }
        public decimal PendingPrincipalAmount { get; set; }
        public RepaymentStatus Status { get; set; }
        public ModeOfPayment PaymentMode { get; set; }
        public string ChequeDetails { get; set; }
        public string Notes { get; set; }
        [ForeignKey("LoanId")]
        public sdtoLoanInfo LoanDetails { get; set; }
    }
}
