using Data.Models.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementSystem.Models
{
    [NotMapped]
    public class sdtoViewAccJournalEntry
    {
        [Display(Name = "Bank Book")]
        public sdtoAccountBook Book { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Balance")]
        public decimal Balance { get; set; }

        [Display(Name = "Voucher No")]
        public string Voucher { get; set; }

        [Display(Name = "Debit Voucher Total")]
        public decimal DebitVoucherTotal { get; set; }

         [Display(Name = "Credit Voucher Total")]
        public decimal CreditVoucherTotal { get; set; }      

        public int SourceClick { get; set; }

        public List<sdtoViewAccJournalEntryDetails> Details { get; set; }
        public sdtoViewAccJournalEntry()
        {
            Details = new List<sdtoViewAccJournalEntryDetails>();
        }
    }
}
