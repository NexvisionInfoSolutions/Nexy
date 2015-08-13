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
    [System.ComponentModel.DataAnnotations.Schema.Table("Settings")]
    public class sdtoSettings : sdtoBaseData
    {
        [Key]
        public long SettingsId { get; set; }

        public long? CompanyId { get; set; }

        public float BankInterest { get; set; }

        public float BankCharges { get; set; }

        //public long CreatedBy { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public long ModifiedBy { get; set; }
        //public DateTime ModifiedOn { get; set; }
    }
}
