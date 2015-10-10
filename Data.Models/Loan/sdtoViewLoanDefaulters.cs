using Data.Models.Enumerations;
using LoanManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementSystem.Models.Accounts
{
    [NotMapped]
    public class sdtoViewLoanDefaulters
    {
        public int LoanDefaultInterval { get; set; }
        public string InputSelection { get; set; }
        public List<sdtoViewLoanDefaulterDetails> Defaulters { get; set; }
        public bool Checked { get; set; }
        public sdtoViewLoanDefaulters()
        {
            Defaulters = new List<sdtoViewLoanDefaulterDetails>();
        }
    }
}
