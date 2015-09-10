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
    public class sdtoLedgerReport : sdtoBaseData
    {
        //TrDate varchar(30),VoucherNo varchar(30),AccountId int,GroupAcc varchar(2000),OppAccountId int,
	    //OppAccCode varchar(2000),OppAccName varchar(2000) ,Narration varchar(2000),DrAmount decimal(18,2),CrAmount  decimal(18,2)

        public string TrDate { get; set; }
        public string VoucherNo { get; set; }
        public int AccountId { get; set; }
        public string GroupAcc { get; set; }
        public int OppAccountId { get; set; }
        public string OppAccCode { get; set; }
        public string OppAccName { get; set; }
        public string Narration { get; set; }
        public decimal DrAmount { get; set; }
        public decimal CrAmount { get; set; }

         
    }
}
