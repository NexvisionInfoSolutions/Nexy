using Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Data.Models.Common;

namespace LoanManagementSystem.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Company")]
    public class sdtoCompany : sdtoBaseData
    {
        [Key]
        public long CompanyId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(10)]
        public string Code { get; set; }

        public long AddressId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("AddressId")]
        public sdtoAddress Address { get; set; }

        public long ContactId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("ContactId")]
        public sdtoContact Contact { get; set; }

        public long CommunicationAddressId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("CommunicationAddressId")]
        public sdtoAddress CommunicationAddress { get; set; }

        public long CommunicationContactId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("CommunicationContactId")]
        public sdtoContact CommunicationContact { get; set; }
        public long BillingAddressId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("BillingAddressId")]
        public sdtoAddress BillingAddress { get; set; }

        public long BillingContactId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("BillingContactId")]
        public sdtoContact BillingContact { get; set; }

        public CompanyStatus Status { get; set; }

        public bool IsDeleted { get; set; }

        [MaxLength(200)]
        public string Owner { get; set; }

        [MaxLength(200)]
        public string WebUrl { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string LogoUrl { get; set; }

        [MaxLength(20)]
        public string TIN { get; set; }

        public DateTime CreatedOn { get; set; }

        public long CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public long ModifiedBy { get; set; }
    }
}