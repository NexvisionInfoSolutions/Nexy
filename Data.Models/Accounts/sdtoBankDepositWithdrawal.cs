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
    public class sdtoBankDepositWithdrawal : sdtoBaseData
    {
        public long Id { get; set; }
        public string VoucherNo { get; set; }
        public decimal VoucherTotal { get; set; }
        public string FromModule { get; set; }
        public string TransType { get; set; }
        public DateTime TransDate { get; set; }
        public string Transaction { get; set; }
        public long TransId { get; set; }
        public string Narration { get; set; }
        public decimal Amount { get; set; }
        public string Instrument { get; set; }
        public string InstrumentNo { get; set; }
        public DateTime InstrumentDate { get; set; }
        public string PeriodName { get; set; }
        public string BookName { get; set; }
        public string AccountName { get; set; }
        public string AccountCode { get; set; }
        public string AccountType { get; set; }
    }
}
