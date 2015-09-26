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
    public class sdtoViewAccExpenseEntryDetails
    {
        [Required(ErrorMessage = "Please select the expense account")]
        [Display(Name = "Expense Id")]
        public sdtoAccountHead AccountHead { get; set; }

        [Display(Name = "Description")]
        public string Narration { get; set; }

        [Required(ErrorMessage = "Please select the expense amount")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
    }
}
