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
    [System.ComponentModel.DataAnnotations.Schema.Table("AccJournalHeader")]
    public class sdtoJournalHeader : sdtoBaseData
    {
        //Id, BookId, TransDate, VoucherNo, VoucherTotal, TransType, FinYear, CrTotal,   FromModule, [Transaction], TransId, Cancelled
        [Key]
        public long Id { get; set; }

        [ForeignKey("BookId")]
        public virtual sdtoAccountBook AccountBook { get; set; }
        public long BookId { get; set; }

        [Display(Name = "Transaction Date")]
        public DateTime TransDate { get; set; }

        [MaxLength(100)]
        public string VoucherNo { get; set; }

        public float VoucherTotal { get; set; }

        //[MaxLength(1)]
        //public int TransType { get; set; }//0 for Bank Deposit , 1 for Bank Withdrawal, 

        [Display(Name = "Financial Year")]
        [ForeignKey("FinYear")]
        public sdtoFinancialPeriod FinancialYear { get; set; }
        public long FinYear { get; set; }

        public int FromModule { get; set; } //0 for "From Accounts", 1 for "From Posting"

        public int Transaction { get; set; } //0 for Bank Deposit, 1 for Withdrawal, 2 for "Loan Entry", 3 for "Loan repayment"

        public long TransId { get; set; }// Transaction id 

        public int Cancelled { get; set; }//0 or 1
    }
}
