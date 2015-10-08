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
    public class sdtoAccountBook : sdtoBaseData
    {
        [Key]
        [Display(Name = "Account Book Id")]
        public long AccountBookId { get; set; }

        [Display(Name = "Book Code")]
        [MaxLength(100)]
        public string BookCode { get; set; }

        [Display(Name = "Book Name")]
        [MaxLength(100)]
        public string BookName { get; set; }

        [Display(Name = "Description")]
        [MaxLength(100)]
        public string BookDescription { get; set; }

        [Display(Name = "Book Type")]
        [ForeignKey("AccountBookType")]
        public long AccountBookTypeId { get; set; }

        [Display(Name = "Link Account")]
        public long? AccountHeadId { get; set; }

        [Range(float.MinValue, float.MaxValue)]
        [Display(Name = "Interest")]
        public float? BankInterest { get; set; }

        [Range(float.MinValue, float.MaxValue)]
        [Display(Name = "Bank Charge")]
        public float? BankCharges { get; set; }

        [Display(Name = "Receipt Voucher Prefix")]
        [StringLength(20)]
        public string ReceiptVoucherPrefix { get; set; }

        [Display(Name = "Receipt Voucher Suffix")]
        [StringLength(20)]
        public string ReceiptVoucherSuffix { get; set; }

        [Display(Name = "Payment Voucher Prefix")]
        [StringLength(20)]
        public string PaymentVoucherPrefix { get; set; }

        [Display(Name = "Payment Voucher Suffix")]
        [StringLength(20)]
        public string PaymentVoucherSuffix { get; set; }

        public AccountBookStatus Status { get; set; }
        
        public virtual sdtoAccountBookType AccountBookType { get; set; }

        [ForeignKey("AccountHeadId")]
        public virtual sdtoAccountHead AccountHead { get; set; }

        //public DateTime CreatedOn { get; set; }
        //public DateTime? ModifiedOn { get; set; }
        //public long CreatedBy { get; set; }
        //public long ModifiedBy { get; set; }
    }
}
