using Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Taluk")]
    public class sdtoTaluk : sdtoBaseData
    {
        [Key]
        public long TalukId { get; set; }

        [MaxLength(150)]
        public string TalukName { get; set; }

        [MaxLength(50)]
        public string TalukAbbr { get; set; }
       
        public long? DistrictId { get; set; }

        [ForeignKey("DistrictId")]
        public sdtoDistrict DistrictDetails { get; set; }
    }
}