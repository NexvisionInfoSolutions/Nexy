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
    [System.ComponentModel.DataAnnotations.Schema.Table("AccReceiptsHeader")]
    public class sdtoReceiptHeader : sdtoBaseData
    {
        // Id, BookId, TransDate, VoucherNo, VoucherTotal, TransType, FinYear,   FromModule, [Transaction], TransId, Cancelled
        [Key]
        public long Id { get; set; }

        public long BookId { get; set; }

        [ForeignKey("BookId")]
        public virtual sdtoAccountBook AccountBook { get; set; }

        [Display(Name = "Transaction Date")]
        public DateTime TransDate { get; set; }

        [MaxLength(100)]
        public string VoucherNo { get; set; }

        public decimal VoucherTotal { get; set; }

        public ReceiptType TransType { get; set; }//0 for Cash Receipt, 1 for Cash Payment, 

        [Column("FinYear")]
        public long FinancialYearId { get; set; }

        [Display(Name = "Financial Year")]
        [ForeignKey("FinancialYearId")]
        public sdtoFinancialPeriod FinancialYear { get; set; }
        
        public int FromModule { get; set; } //0 for "From Accounts", 1 for "From Posting"

        public TransactionType Transaction { get; set; } //0 for Cash Receipt, 1 for Cash Payment, 2 for "Loan Entry", 3 for "Loan repayment"

        public long TransId { get; set; }// Transaction id 

        public int Cancelled { get; set; }//0 or 1
    }
}
