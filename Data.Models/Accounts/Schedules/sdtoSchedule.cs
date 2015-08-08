using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Accounts.Schedules
{
    [Table("AccountSchedule")]
    public class sdtoSchedule
    {
        [Key]
        public long ScheduleId { get; set; }

        [MaxLength(100)]
        public string ScheduleName { get; set; }

        [Editable(false)]
        public long ParentId { get; set; }

        [NotMapped]
        public sdtoSchedule Parent { get; set; }
    }
}
