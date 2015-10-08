using Data.Models.Accounts;
using Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("SysCode")]
    public class sdtoSysCode
    {
        [Key]
        [Display(Name = "SystemCodeId")]
        public long SysCodeId { get; set; }
        public string TableName { get; set; }
        public string IdField { get; set; }
        public string CodeField { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
    }
}