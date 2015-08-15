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
        public long UserId { get; set; }
        public DateTime? RepaymentStartDate { get; set; }
        public RepaymentInterval RePaymentInterval { get; set; }
        public decimal RequestedAmount { get; set; }
        public decimal ProposedAmount { get; set; }
        public decimal LoanAmount { get; set; }
        public int TotalInstallments { get; set; }
        public LoanStatus Status { get; set; }
        public string ChequeDetails { get; set; }
        public float InteresRate { get; set; }
        public DateTime? SanctionedDate { get; set; }
        public long SanctionedBy { get; set; }
        public string Notes { get; set; }

        [ForeignKey("UserId")]
        public sdtoUser Member { get; set; }
        public decimal InstallmentAmount { get; set; }
    }
}
