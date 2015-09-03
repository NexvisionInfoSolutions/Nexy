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
    [System.ComponentModel.DataAnnotations.Schema.Table("JournalDetails")]
    public class sdtoJournalDetails : sdtoBaseData
    {
        [Key]
        public long JournalDtlId { get; set; }

        [ForeignKey("JournalId")]
        public virtual sdtoJournalHeader JournalDetail { get; set; }
        public long JournalId { get; set; }

        [ForeignKey("AccountId")]
        public virtual sdtoAccountHead AccountDetails { get; set; }
        public long AccountId { get; set; }

        [MaxLength(1000)]
        public string Narration { get; set; }

        public float DbAmount { get; set; }

        public float CrAmount { get; set; }

    }
}
