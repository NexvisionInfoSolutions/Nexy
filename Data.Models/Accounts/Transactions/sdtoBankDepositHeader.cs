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
    [System.ComponentModel.DataAnnotations.Schema.Table("AccBankDepositHeader")]
    public class sdtoBankDepositHeader : sdtoBaseData
    {
        //  Id, BookId, TransDate, VoucherNo, VoucherTotal, TransType, FinYear,  Cancelled
        [Key]
        public long Id { get; set; }
        [Required]
        public long BookId { get; set; }
        [Required(ErrorMessage = "Please select the financial year")]
        public long FinYear { get; set; }
        [ForeignKey("BookId")]
        public virtual sdtoAccountBook AccountBook { get; set; }
        [Required(ErrorMessage = "Please enter the transaction date")]
        [Display(Name = "Transaction Date")]
        public DateTime TransDate { get; set; }
        [MaxLength(100)]
        public string VoucherNo { get; set; }
        public decimal VoucherTotal { get; set; }
        public BankTransType TransType { get; set; }//0 for Deposit 1 for Withdrawal, 
        [Display(Name = "Financial Year")]
        [ForeignKey("FinYear")]
        public sdtoFinancialPeriod FinancialYear { get; set; }
        public int FromModule { get; set; } //0 for "From Accounts", 1 for "From Posting"
        public TransactionType Transaction { get; set; } //0 for deposit 1 for withdrawal, 2 for "Loan Entry", 3 for "Loan repayment"
        public long TransId { get; set; }// Transaction id 
        public int Cancelled { get; set; }//0 or 1

        public sdtoBankDepositHeader()
        {
            TransDate = DateTime.Now;
        }
    }
}
