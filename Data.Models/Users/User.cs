﻿using Data.Models.Enumerations;
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

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        /// <summary>
        /// Username or email of the user that is used to validate the user.
        /// </summary>
        [MaxLength(200)]
        public string UserName { get; set; }

        [MaxLength(100)]
        public string Password { get; set; }

        public string Description { get; set; }

        public long? UserGroupId { get; set; }

        [ForeignKey("UserGroupId")]
        public sdtoUserGroup UserGroup { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(50)]
        public string Designation { get; set; }

        public UserType? UserType { get; set; }

        public long UserContactId { get; set; }

        public long? UserAddressId { get; set; }

        public long? PermanentAddressId { get; set; }

        public long? GuaranterAddressId { get; set; }

        public long? PermanentContactId { get; set; }

        public long? GuaranterContactId { get; set; }

        public string FatherName { get; set; }

        public string GuaranterName { get; set; }

        public string Occupation { get; set; }

        //public DateTime CreatedOn { get; set; }

        [ForeignKey("UserContactId")]
        public virtual sdtoContact Contacts { get; set; }

        [ForeignKey("UserAddressId")]
        public virtual sdtoAddress UserAddress { get; set; }

        [ForeignKey("PermanentAddressId")]
        public virtual sdtoAddress PermanentAddress { get; set; }

        [ForeignKey("GuaranterAddressId")]
        public virtual sdtoAddress GuaranterAddress { get; set; }

        [ForeignKey("PermanentContactId")]
        public virtual sdtoContact PermanentContacts { get; set; }

        [ForeignKey("GuaranterContactId")]
        public virtual sdtoContact GuaranterContacts { get; set; }

        public long? CompanyId { get; set; }
    }
}