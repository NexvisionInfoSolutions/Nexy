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
    public class sdtoViewAccExpenseEntry
    {
        [Display(Name = "Bank Book")]
        public sdtoAccountBook Book { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }      
        
        [Display(Name = "Description")]
        public string Description{get;set;}

        [Display(Name = "Amount")]
        public decimal Amount{get;set;}

        public int SourceClick { get; set; }

        public List<sdtoViewAccExpenseEntryDetails> Details { get; set; }
        public sdtoViewAccExpenseEntry()
        {
            Date = DateTime.Now.Date;
            Details = new List<sdtoViewAccExpenseEntryDetails>();
        }
    }
}
