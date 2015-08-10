using Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class sdtoBaseData
    {
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long CreatedBy { get; set; }

        public long ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public long DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}