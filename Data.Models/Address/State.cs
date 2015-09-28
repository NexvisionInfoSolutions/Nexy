using Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("State")]
    public class sdtoState : sdtoBaseData
    {
        [Key]
        [Display(Name="State")]
        public long StateId { get; set; }

        [MaxLength(150)]
        public string StateName { get; set; }

        [MaxLength(50)]
        public string StateAbbr { get; set; }

        [Display(Name = "Country")]
        public long? CountryId { get; set; }

        [ForeignKey("CountryId")]
        public sdtoCountry CountryDetails { get; set; }
    }
}