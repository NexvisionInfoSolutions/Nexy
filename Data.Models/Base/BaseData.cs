using Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class sdtoBaseData
    {
        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [ForeignKey("CreatedByUser")]
        public long? CreatedBy { get; set; }

        [ForeignKey("ModifiedByUser")]
        public long? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        [ForeignKey("DeletedByUser")]
        public long? DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }

        public sdtoUser CreatedByUser { get; set; }
        public sdtoUser DeletedByUser { get; set; }
        public sdtoUser ModifiedByUser { get; set; }

    }
}