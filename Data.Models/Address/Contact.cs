using Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class sdtoContact : sdtoBaseData
    {
        [Key]
        public long ContactId { get; set; }

        [MaxLength(20)]
        public string Telephone1 { get; set; }
        
        [MaxLength(20)]
        public string Telephone2 { get; set; }

        [MaxLength(20)]
        public string Mobile1 { get; set; }

        [MaxLength(20)]
        public string Mobile2 { get; set; }

        [MaxLength(20)]
        public string Fax1 { get; set; }

        [MaxLength(20)]
        public string Fax2 { get; set; }

        [MaxLength(200)]
        public string Email1 { get; set; }

        [MaxLength(200)]
        public string Email2 { get; set; }
        
        public DateTime CreatedOn { get; set; }

        public long CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public long ModifiedBy { get; set; }
    }
}