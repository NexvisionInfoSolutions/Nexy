using Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Country")]
    public class sdtoCountry : sdtoBaseData
    {
        [Key]
        [Column("Country_Id")]
        public long CountryId { get; set; }

        [MaxLength(150)]
        public string CountryName { get; set; }

        [MaxLength(50)]
        public string CountryAbbr { get; set; }                       
    }
}