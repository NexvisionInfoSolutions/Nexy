using Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Village")]
    public class sdtoVillage : sdtoBaseData
    {
        [Key]
        public long VillageId { get; set; }

        [MaxLength(150)]
        public string VillageName { get; set; }

        [MaxLength(50)]
        public string VillageAbbr { get; set; }
        
        public long? TalukId { get; set; }

        [ForeignKey("TalukId")]
        public sdtoTaluk TalukDetails { get; set; }
    }
}