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
    [System.ComponentModel.DataAnnotations.Schema.Table("AccJournalDetail")]
    public class sdtoJournalDetails : sdtoBaseData
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("JournalId")]
        public virtual sdtoJournalHeader JournalDetail { get; set; }
        [Required(ErrorMessage = "Please select the Journal")]
        public long JournalId { get; set; }

        [ForeignKey("AccountId")]
        public virtual sdtoAccountHead AccountDetails { get; set; }

        [Required(ErrorMessage = "Please select the account head for the transaction")]
        public long AccountId { get; set; }

        [MaxLength(1000)]
        public string Narration { get; set; }

        [Range(double.MinValue, double.MaxValue)]
        public float DrAmount { get; set; }

        [Range(double.MinValue, double.MaxValue)]
        public float CrAmount { get; set; }

    }
}
