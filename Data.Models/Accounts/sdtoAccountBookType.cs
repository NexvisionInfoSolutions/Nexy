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
    [System.ComponentModel.DataAnnotations.Schema.Table("AccountBookType")]
    public class sdtoAccountBookType
    {
        [Key]
        public long AccountBookTypeId { get; set; }

        [MaxLength(100)]
        public string AccountBookType { get; set; }

        public AccountBookTypeStatus Status { get; set; }
    }
}
