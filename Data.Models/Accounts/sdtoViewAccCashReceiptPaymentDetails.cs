﻿using Data.Models.Accounts;
using Data.Models.Enumerations;
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
    public class sdtoViewAccCashReceiptPaymentDetails
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please select a account")]
        [Display(Name = "Account Head")]
        public long AccountHeadId { get; set; }

        [Display(Name = "Narration")]
        public string Narration { get; set; }

        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
    }
}
