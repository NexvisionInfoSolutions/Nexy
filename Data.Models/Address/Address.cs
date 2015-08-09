using Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public string State { get; set; }

        public string Country { get; set; }

        public string Zipcode { get; set; }

        public string Place { get; set; }

        public string Post { get; set; }

        public string District { get; set; }


        public DateTime CreatedOn { get; set; }

        public long CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public long ModifiedBy { get; set; }
    }
}