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
    [System.ComponentModel.DataAnnotations.Schema.Table("OpeningBalance")]
    public class sdtoOpeningBalance : sdtoBaseData
    {
        // Id, BookId, TransDate, VoucherNo, VoucherTotal, TransType, FinYear,   FromModule, [Transaction], TransId, Cancelled
        [Key]
        public long OpeningBalanceId { get; set; }

        public long AccountHeadId { get; set; }

        [ForeignKey("AccountHeadId")]
        public virtual sdtoAccountHead AccountHead { get; set; }

        [Display(Name = "Schedule")]
        public long ScheduleId { get; set; }

        [ForeignKey("ScheduleId")]
        public virtual Data.Models.Accounts.Schedules.sdtoSchedule Schedule { get; set; }

        public decimal DebitOpeningBalance { get; set; }
        public decimal CreditOpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public long FinancialYearId { get; set; }
        [ForeignKey("ScheduleId")]
        public virtual sdtoFinancialPeriod FinancialPeriod { get; set; }
    }
}
