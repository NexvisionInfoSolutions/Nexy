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
    public class sdtoViewLoanRepayments
    {       
        public long LoanId { get; set; }
        public string InputSelection { get; set; }
        public List<sdtoViewRepaymentOfLoans> Repayments { get; set; }
        public sdtoViewLoanRepayments()
        {
            Repayments = new List<sdtoViewRepaymentOfLoans>();
        }
    }
}
