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
    [System.ComponentModel.DataAnnotations.Schema.Table("DepositInfo")]
    public class sdtoDepositInfo : sdtoBaseData
    {
        [Key]
        public long DepositId { get; set; }
        /// <summary>
        /// User Id of the member
        /// </summary>
        [Display(Name = "User ID")]
        public long UserId { get; set; }

        /// <summary>
        /// Duration of the deposit in days
        /// </summary>
        [Display(Name = "Duration in Days")]
        public long Duration { get; set; }

        [Display(Name = "Deposit Type")]
        public DepositType DepositType { get; set; }

        [Display(Name = "Maturity Date")]
        public DateTime? MaturityDate { get; set; }
        
        [Display(Name = "Total Installments")]
        public int TotalInstallments { get; set; }

        [Display(Name = "Deposit Amount")]
        public decimal DepositAmount { get; set; }

        [Display(Name = "Mature Amount")]
        public decimal MatureAmount { get; set; }

        [Display(Name = "Recurring Amount")]
        public decimal InstallmentAmount { get; set; }

        [Display(Name = "Closed Date")]
        public DateTime? ClosedDate { get; set; }

        [Display(Name = "Recurring Date")]
        public DateTime? RecurringDepositDate { get; set; }

        public DepositStatus Status { get; set; }

        [Display(Name = "Cheque Details")]
        public string ChequeDetails { get; set; }

        [Display(Name = "Interes Rate")]
        public float InteresRate { get; set; }

        [Display(Name = "Approved Date")]
        public DateTime? ApprovedDate { get; set; }

        [Display(Name = "Approved By")]
        public long? ApprovedBy { get; set; }

        public string Notes { get; set; }

        [ForeignKey("UserId")]
        public sdtoUser Member { get; set; }
    }
}
