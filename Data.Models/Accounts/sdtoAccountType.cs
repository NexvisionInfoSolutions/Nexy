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
    [System.ComponentModel.DataAnnotations.Schema.Table("AccountType")]
    public class sdtoAccountType
    {
        [Key]
        public long sdtoAccountTypeId { get; set; }

        [MaxLength(100)]
        public string AccountType { get; set; }

        public AccountTypeStatus Status { get; set; }
    }
}
