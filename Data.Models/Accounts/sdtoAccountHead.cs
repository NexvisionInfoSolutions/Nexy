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
    public class sdtoAccountHead
    {
        [Key]
        public long AccountHeadId { get; set; }

        public string AccountCode { get; set; }

        public string AccountName { get; set; }

        public long ScheduleId { get; set; }

        public long AccountTypeId { get; set; }

        public decimal CreditLimit { get; set; }

        public float CreditDays { get; set; }

        public long ContactId { get; set; }

        public long AddressId { get; set; }

        public long ContactUserId { get; set; }

        public string TIN { get; set; }

        public string CST { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        [ForeignKey("ContactId")]
        public virtual sdtoContact Contacts { get; set; }

        [ForeignKey("AddressId")]
        public virtual sdtoAddress Address { get; set; }

        [ForeignKey("ContactUserId")]
        public virtual sdtoUser ContactPerson { get; set; }

        [ForeignKey("AccountTypeId")]
        public virtual sdtoAccountType AccountType { get; set; }
    }
}
