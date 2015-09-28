using Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("District")]
    public class sdtoDistrict : sdtoBaseData
    {
        [Key]
        public long DistrictId { get; set; }

        [MaxLength(150)]
        public string DistrictName { get; set; }

        [MaxLength(50)]
        public string DistrictAbbr { get; set; }
        
        public long? StateId { get; set; }

        [ForeignKey("StateId")]
        public sdtoState StateDetails { get; set; }
    }
}