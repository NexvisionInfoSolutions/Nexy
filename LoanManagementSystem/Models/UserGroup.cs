using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class UserGroup
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(10)]
        public string Code { get; set; }

        public string Description { get; set; }


    }
}