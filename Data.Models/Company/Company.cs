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
        public long CompanyId { get; set; }

        [MaxLength(10)]
        public string Code { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } 

        public long AddressId { get; set; }

        [MaxLength(200)]
        public string Owner { get; set; }

        [MaxLength(20)]
        public string TIN { get; set; }

         
        public long ContactId { get; set; }
 
         
        //public CompanyStatus Status { get; set; } 
        public bool IsDeleted { get; set; }

         
        [MaxLength(200)]
        public string WebUrl { get; set; }
         
        public string LogoUrl { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        [ForeignKey("ContactId")]
        public virtual sdtoContact Contacts { get; set; }

        [ForeignKey("AddressId")]
        public virtual sdtoAddress Address { get; set; }      
    }
}