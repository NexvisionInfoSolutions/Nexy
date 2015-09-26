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
        [Display(Name = "Schedule Id")]
        public long ScheduleId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ScheduleName { get; set; }

        [Editable(false)]
        public long ParentId { get; set; }

        public long BaseScheduleId { get; set; }

        [NotMapped]
        public sdtoSchedule Parent { get; set; }

        [NotMapped]
        public sdtoSchedule BaseSchedule { get; set; }

        public string ShortName { get; set; }
    }
}
