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
    [System.ComponentModel.DataAnnotations.Schema.Table("AccountHead")]
    public class sdtoAccountHead : sdtoBaseData
    {
        [Key]
        [Display(Name = "Account Id")]
        public long AccountHeadId { get; set; }

        [Required(ErrorMessage = "Please enter an account code")]
        [Display(Name = "Account Code")]
        public string AccountCode { get; set; }

        [Required(ErrorMessage = "Please enter an account name")]
        [Display(Name = "Account Name")]
        public string AccountName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select an account schedule")]
        [Display(Name = "Schedule")]
        public long ScheduleId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select an account type")]
        [Display(Name = "Account Type")]
        public long AccountTypeId { get; set; }

        [Range(double.MinValue, double.MaxValue)]
        [Display(Name = "Credit Limit")]
        public decimal? CreditLimit { get; set; }

        [Range(float.MinValue, float.MaxValue)]
        [Display(Name = "Credit Days")]
        public float? CreditDays { get; set; }

        [Display(Name = "Contact")]
        public long ContactId { get; set; }

        [Display(Name = "Address")]
        public long AddressId { get; set; }

        public string TIN { get; set; }

        public string CST { get; set; }

        //public long CreatedBy { get; set; }

        //public DateTime CreatedOn { get; set; }

        //public long ModifiedBy { get; set; }

        //public DateTime? ModifiedOn { get; set; }

        [ForeignKey("ContactId")]
        public virtual sdtoContact Contacts { get; set; }

        [ForeignKey("AddressId")]
        public virtual sdtoAddress Address { get; set; }

        [ForeignKey("AccountTypeId")]
        public virtual sdtoAccountType AccountType { get; set; }

        [ForeignKey("ScheduleId")]
        public virtual Data.Models.Accounts.Schedules.sdtoSchedule Schedule { get; set; }
    }
}
