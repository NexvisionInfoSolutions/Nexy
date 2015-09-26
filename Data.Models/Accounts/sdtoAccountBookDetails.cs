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
    public class sdtoAccountBookDetails
    {
        public long AccountBookId { get; set; }

        public long AccountHeadId { get; set; }

        public decimal Balance { get; set; }

        public string VoucherNo { get; set; }

        public DateTime Date { get; set; }
    }
}
