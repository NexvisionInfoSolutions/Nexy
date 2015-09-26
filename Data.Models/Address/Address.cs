using Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Address")]
    public class sdtoAddress : sdtoBaseData
    {
        [Key]
        public long AddressId { get; set; }

        [MaxLength(150)]
        public string Address1 { get; set; }

        [MaxLength(150)]
        public string Address2 { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [Display(Name = "State")]
        [ForeignKey("StateDetails")]
        public long? StateId { get; set; }

        [Display(Name = "Country")]
        public long? CountryId { get; set; }

        public sdtoState StateDetails { get; set; }

        [ForeignKey("CountryId")]
        public sdtoCountry Country { get; set; }

        [Display(Name = "Zip code")]
        public string Zipcode { get; set; }

        public string Place { get; set; }

        public string Post { get; set; }

        public string District { get; set; }
        public string Taluk { get; set; }
        public string Village { get; set; }
    }
}