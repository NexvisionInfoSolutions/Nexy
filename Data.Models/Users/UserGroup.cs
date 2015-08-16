using Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("UserGroup")]
    public class sdtoUserGroup : sdtoBaseData
    {
        public sdtoUserGroup()
        {
            CreatedOn = DateTime.Now;
            ModifiedOn = DateTime.Now;
        }

        
        [Key]
        [Display(Name = "User Group")]
        public long UserGroupId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(10)]
        public string Code { get; set; }

        public UserGroupStatus Status { get; set; }

        //public bool IsDeleted { get; set; }

        public string Description { get; set; }

        //public DateTime CreatedOn { get; set; }

        //public long CreatedBy { get; set; }

        //public DateTime ModifiedOn { get; set; }

        //public long ModifiedBy { get; set; }
    }
}