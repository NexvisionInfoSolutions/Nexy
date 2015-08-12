using Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Users")]
    public class sdtoUser : sdtoBaseData
    {
        [Key]
        public long UserID { get; set; }

        [MaxLength(10)]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }

        public long? UserGroupId { get; set; }

        [ForeignKey("UserGroupId")]
        public sdtoUserGroup UserGroup { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(50)]
        public string Designation { get; set; }

        public UserType? UserType { get; set; }

        public long? ContactId { get; set; }

        public long? UserAddressId { get; set; }

        public long? PermanentAddressId { get; set; }

        public long? GuaranterAddressId { get; set; }

        public long? PermanentContactId { get; set; }

        public long? GuaranterContactId { get; set; }

        public string FatherName { get; set; }

        public string GuaranterName { get; set; }

        public string Occupation { get; set; }

        //public DateTime CreatedOn { get; set; }

        [ForeignKey("ContactId")]
        public sdtoContact Contacts { get; set; }

        [ForeignKey("UserAddressId")]
        public sdtoAddress UserAddress { get; set; }

        [ForeignKey("PermanentAddressId")]
        public sdtoAddress PermanentAddress { get; set; }

        [ForeignKey("GuaranterAddressId")]
        public sdtoAddress GuaranterAddress { get; set; }

        [ForeignKey("PermanentContactId")]
        public sdtoContact PermanentContacts { get; set; }

        [ForeignKey("GuaranterContactId")]
        public sdtoContact GuaranterContacts { get; set; }
    }
}