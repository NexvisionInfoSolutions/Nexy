using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [MaxLength(10)]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }

        
        public int UserGroupID { get; set; }
        [ForeignKey("UserGroupID")]
        public virtual UserGroup UserGroup { get; set; }

        public bool IsActive { get; set; }

    }
}