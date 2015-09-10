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
    [System.ComponentModel.DataAnnotations.Schema.Table("AccReceiptsDetail")]
    public class sdtoReceiptDetails : sdtoBaseData
    {
        // Id, ReceiptsId, AccountId, Narration, Amount, OppAccountId, OppAmount, Display
        [Key]
        public long Id { get; set; }

        [ForeignKey("ReceiptsId")]
        public virtual sdtoReceiptHeader ReceiptDetail { get; set; }
        public long ReceiptsId { get; set; }

        [ForeignKey("AccountId")]
        public virtual sdtoAccountHead AccountDetails { get; set; }
        public long AccountId { get; set; }

        [MaxLength(1000)]
        public string Narration { get; set; }

        public float Amount { get; set; }

        //public float Amount { get; set; }

        public int Display { get; set; }//0 or 1
    }
}
