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
    [System.ComponentModel.DataAnnotations.Schema.Table("ReceiptDetails")]
    public class sdtoReceiptDetails : sdtoBaseData
    {
        // Id, ReceiptsId, AccountId, Narration, Amount, OppAccountId, OppAmount, Display
        [Key]
        public long ReceiptDtlId { get; set; }

        [ForeignKey("ReceiptId")]
        public virtual sdtoReceiptHeader ReceiptDetail { get; set; }
        public long ReceiptId { get; set; }

        [ForeignKey("AccountId")]
        public virtual sdtoAccountHead AccountDetails { get; set; }
        public long AccountId { get; set; }

        [MaxLength(1000)]
        public string Narration { get; set; }

        public float DbAmount { get; set; }

        public float CrAmount { get; set; }

        public int Display { get; set; }//0 or 1
    }
}
