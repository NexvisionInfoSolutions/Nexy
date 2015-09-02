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
    [NotMapped]
    public class sdtoRptLoanSummary : sdtoBaseData
    {
        public long LoanId { get; set; }
        public long UserId { get; set; }
        public decimal LoanAmount { get; set; }
        public int TotalInstallments { get; set; }
        public float InteresRate { get; set; }
        public decimal InstallmentAmount { get; set; }
        public DateTime RepaymentStartDate { get; set; }
        public decimal TotalPaidAmountPerLoan { get; set; }
        public decimal TotalPaidAmountPerUser { get; set; }
        //public decimal TotalPaidAmountPerDate { get; set; }
        public decimal BalanceLoanAmount { get; set; }
        public int BalanceLoanInstallments { get; set; }
        public decimal TotalInterestPaidAmountPerLoan { get; set; }
        public decimal TotalInterestPaidAmountPerUser { get; set; }      
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string GuaranterName { get; set; }
        public string UserAddress { get; set; }
        public string UserPhone { get; set; }
        public string UserMobile { get; set; }
        public string UserEmail { get; set; }
        public string UserPermanentAddress { get; set; }
        public string PermanentPhone { get; set; }
        public string PermanentMobile { get; set; }
        public string PermanentEmail { get; set; }
    }
}
