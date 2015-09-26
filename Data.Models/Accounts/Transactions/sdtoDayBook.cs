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
    [System.ComponentModel.DataAnnotations.Schema.Table("DayBook")]
    public class sdtoDayBook : sdtoBaseData
    {
        // Id, BookId, TransDate, VoucherNo, VoucherTotal, TransType, FinYear,   FromModule, [Transaction], TransId, Cancelled
        [Key]
        public long DayBookId { get; set; }

        public long AccountHeadId { get; set; }

        [ForeignKey("AccountHeadId")]
        public virtual sdtoAccountHead AccountHead { get; set; }
        public DateTime Date { get; set; }
        public decimal Receipt { get; set; }
        public decimal Payment { get; set; }
        public decimal ClosingBalance { get; set; }
        public long FinancialYearId { get; set; }
        [ForeignKey("FinancialYearId")]
        public virtual sdtoFinancialPeriod FinancialPeriod { get; set; }
    }
}
