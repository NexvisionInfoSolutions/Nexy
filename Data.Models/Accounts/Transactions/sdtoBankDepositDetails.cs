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
    [System.ComponentModel.DataAnnotations.Schema.Table("BankDepositDetails")]
    public class sdtoBankDepositDetails : sdtoBaseData
    {
        // Id, BankDepositId, AccountId, Narration, Amount, InstrId, InstrNo, InstrDate, Bank,  Display
        [Key]
        public long BankTransDtlId { get; set; }
        public long AccountId { get; set; }               
        public long BankDepositId { get; set; }

        [ForeignKey("AccountId")]
        public virtual sdtoAccountHead AccountDetails { get; set; }

        [ForeignKey("BankDepositId")]
        public virtual sdtoBankDepositHeader BankTransDetail { get; set; }

        [MaxLength(1000)]
        public string Narration { get; set; }

        public float DbAmount { get; set; }

        public float CrAmount { get; set; }

        public Instrument Instrument { get; set; }

        [MaxLength(1000)]
        public string InstrumentNo { get; set; }

        public DateTime InstrumentDate { get; set; }

        public int Display { get; set; }//0 or 1          
    }
}
