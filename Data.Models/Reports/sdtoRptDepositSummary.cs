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
    public class sdtoRptDepositSummary : sdtoBaseData
    {
        public long DepositId { get; set; }
        public long UserId { get; set; }
        public decimal DepositAmount { get; set; }
        //public int TotalInstallments { get; set; }
        public float InteresRate { get; set; }
        //public decimal InstallmentAmount { get; set; }
        public DateTime DepositDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public decimal MatureAmount { get; set; }
        public decimal TotalWithdrawnAmountPerDeposit { get; set; }
        public decimal TotalWithdrawnAmountPerUser { get; set; }
        //public decimal TotalPaidAmountPerDate { get; set; }
        public decimal BalanceDepositAmount { get; set; }
        //public int BalanceLoanInstallments { get; set; }
        public decimal TotalInterestPaidAmountPerDeposit { get; set; }
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
