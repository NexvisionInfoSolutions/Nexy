using Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagementSystem.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Company")]
    public class sdtoCompany : sdtoBaseData
    {
        [Key]
        [Display(Name = "Company ID")]
        public long CompanyId { get; set; }

        [MaxLength(20)]
        public string Code { get; set; }

        [MaxLength(100)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Address")]
        public long AddressId { get; set; }

        [MaxLength(200)]
        public string Owner { get; set; }

        [MaxLength(20)]
        public string TIN { get; set; }

        public long ContactId { get; set; }

        //public CompanyStatus Status { get; set; } 
        //public bool IsDeleted { get; set; }

        [MaxLength(200)]
        [Display(Name = "Web Url")]
        public string WebUrl { get; set; }

        [NotMapped]
        [Display(Name = "Logo Url")]
        public string LogoUrl { get; set; }

        //public long CreatedBy { get; set; }

        //public DateTime CreatedOn { get; set; }

        //public long ModifiedBy { get; set; }

        //public DateTime ModifiedOn { get; set; }

        [ForeignKey("ContactId")]
        public sdtoContact Contacts { get; set; }

        [ForeignKey("AddressId")]
        public sdtoAddress Address { get; set; }
    }
}