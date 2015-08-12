using Data.Models.Enumerations;
using LoanManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Accounts
{
    [System.ComponentModel.DataAnnotations.Schema.Table("UrlInfo")]
    public class sdtoUrlInfo : sdtoBaseData
    {
        [Key]
        public long UrlId { get; set; }

        [MaxLength(100)]
        public string UrlText { get; set; }

        public string Url { get; set; }

        [InverseProperty("Parent")]
        public long ParentId { get; set; }

        //public DateTime CreatedOn { get; set; }
        //public DateTime ModifiedOn { get; set; }
        //public long CreatedBy { get; set; }
        //public long ModifiedBy { get; set; }

        public UrlInfoStatus Status { get; set; }

        //public bool IsDeleted { get; set; }

        //public long DeletedBy { get; set; }

        //public DateTime DeletedOn { get; set; }
        
        public sdtoUrlInfo Parent { get; set; }
    }
}
