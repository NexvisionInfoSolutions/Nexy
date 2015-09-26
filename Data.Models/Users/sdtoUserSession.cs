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
    [System.ComponentModel.DataAnnotations.Schema.Table("UserSession")]
    public class sdtoUserSession
    {
        [Key]
        [Display(Name = "Session Id")]
        public long SessionId { get; set; }

        [Display(Name = "User Id")]
        public long UserId { get; set; }

        [MaxLength(100)]
        public string SessionKey { get; set; }

        public long? CompanyId { get; set; }

        public long? FinancialYearId { get; set; }

        public string Browser { get; set; }

        [MaxLength(30)]
        public string IPAddress { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [ForeignKey("FinancialYearId")]
        public virtual sdtoFinancialPeriod FinancialYear { get; set; }
    }
}