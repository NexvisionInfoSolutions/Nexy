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
    [System.ComponentModel.DataAnnotations.Schema.Table("FinancialPeriod")]
    public class sdtoFinancialPeriod : sdtoBaseData
    {
        [Key]
        public long FinancialPeriodId { get; set; }

        [MaxLength(100)]
        [Display(Name = "Code")]
        public string PeriodCode { get; set; }

        [MaxLength(500)]
        [Display(Name = "Financial Period Name")]
        public string PeriodName { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public bool IsCurrentYear { get; set; }//0 or 1
    }
}
