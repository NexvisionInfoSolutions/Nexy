﻿using Data.Models.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementSystem.Models
{
    [NotMapped]
    public class sdtoViewAccCashReceiptPayment
    {
        public long HeaderId { get; set; }

        public decimal PreviousVoucherTotal { get; set; }

        [Display(Name = "Bank Book")]
        public sdtoAccountBook Book { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Balance")]
        public decimal Balance { get; set; }

        [Display(Name = "Voucher No")]
        public string Voucher { get; set; }

        [Display(Name = "Voucher Total")]
        public decimal VoucherTotal { get; set; }

        [Display(Name = "Account Balance")]
        public decimal AccountBalance { get; set; }

        public int SourceClick { get; set; }

        public List<sdtoViewAccCashReceiptPaymentDetails> Details { get; set; }
        public sdtoViewAccCashReceiptPayment()
        {
            Details = new List<sdtoViewAccCashReceiptPaymentDetails>();
            Voucher = string.Empty;
        }
    }
}
