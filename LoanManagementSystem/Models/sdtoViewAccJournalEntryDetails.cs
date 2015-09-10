using Data.Models.Accounts;
using Data.Models.Enumerations;
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
    public class sdtoViewAccJournalEntryDetails
    {
        [Display(Name = "Account Head")]
        public sdtoAccountHead AccountHead { get; set; }

        [Display(Name = "Narration")]
        public string Narration { get; set; }

        [Display(Name = "Debit Amount")]
        public float DebitAmount { get; set; }

        [Display(Name = "Credit Amount")]
        public float CreditAmount { get; set; }
    }
}
