using Data.Models.Accounts;
using Data.Models.Base;
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
        //[Display(Name = "Account Head")]
        //public sdtoAccountHead AccountHead { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a account")]
        [Display(Name = "Account Head")]
        public long AccountHeadId { get; set; }

        [Display(Name = "Narration")]
        public string Narration { get; set; }

        [CustomValidationMutualExclude("DebitAmount", "CreditAmount", ErrorMessage = "Please enter either debit amount or credit amount")]
        [Range(double.MinValue, double.MaxValue)]
        [Display(Name = "Debit Amount")]
        public float DebitAmount { get; set; }

        [CustomValidationMutualExclude("DebitAmount", "CreditAmount", ErrorMessage = "Please enter either debit amount or credit amount")]
        [Range(double.MinValue, double.MaxValue)]
        [Display(Name = "Credit Amount")]
        public float CreditAmount { get; set; }
    }
}
