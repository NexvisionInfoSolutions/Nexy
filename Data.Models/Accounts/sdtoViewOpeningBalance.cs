using Data.Models.Accounts;
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
    public class sdtoViewOpeningBalance
    {
        public long ScheduleId { get; set; }
        public List<sdtoOpeningBalance> OpeningBalances { get; set; }
        public sdtoViewOpeningBalance()
        {
            OpeningBalances = new List<sdtoOpeningBalance>();
        }
    }
}
