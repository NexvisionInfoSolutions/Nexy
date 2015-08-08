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
    [System.ComponentModel.DataAnnotations.Schema.Table("AccountBook")]
    public class sdtoAccountBook
    {
        [Key]
        public long AccountBookId { get; set; }

        [MaxLength(100)]
        public string BookCode { get; set; }

        [MaxLength(100)]
        public string BookName { get; set; }

        [MaxLength(100)]
        public string BookDescription { get; set; }

        public long AccountBookTypeId { get; set; }

        public long AccountHeadId { get; set; }

        public float BankInterest { get; set; }

        public float BankCharges { get; set; }

        [StringLength(20)]
        public string ReceiptVoucherPrefix { get; set; }

        [StringLength(20)]
        public string ReceiptVoucherSuffix { get; set; }

        [StringLength(20)]
        public string PaymentVoucherPrefix { get; set; }

        [StringLength(20)]
        public string PaymentVoucherSuffix { get; set; }

        public AccountBookStatus Status { get; set; }

        [ForeignKey("AccountBookTypeId")]
        public virtual sdtoAccountBookType AccountBookType { get; set; }

        [ForeignKey("AccountHeadId")]
        public virtual sdtoAccountHead AccountHead { get; set; }


    }
}
