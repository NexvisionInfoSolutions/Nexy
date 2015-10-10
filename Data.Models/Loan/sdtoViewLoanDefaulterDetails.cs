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
    public class sdtoViewLoanDefaulterDetails
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public long LoanId { get; set; }
        public string LoanCode { get; set; }
        public DateTime LastPaidDate { get; set; }
        public sdtoViewLoanDefaulterDetails()
        {
        }
    }
}
