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
    [NotMapped]
    public class sdtoTransactionAccountDetails : sdtoBaseData
    {
        public long AccountBookId { get; set; }

        public string BookName { get; set; }

        public string BookCode { get; set; }

        public string ReceiptVoucherPrefix { get; set; }

        public string ReceiptVoucherSuffix { get; set; }

        public string PaymentVoucherPrefix { get; set; }

        public string PaymentVoucherSuffix { get; set; }

        public decimal ClosingBalance { get; set; }

        public long VoucherNo { get; set; }
    }
}