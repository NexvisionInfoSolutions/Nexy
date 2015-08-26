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

        [Display(Name = "First Name")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(100)]
        public string LastName { get; set; }

        /// <summary>
        /// Username or email of the user that is used to validate the user.
        /// </summary>
        [Display(Name = "User Name")]
        [MaxLength(200)]
        public string UserName { get; set; }

        [MaxLength(100)]        
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string Description { get; set; }

        [Display(Name = "User Group")]
        public long? UserGroupId { get; set; }

        [ForeignKey("UserGroupId")]
        [Display(Name = "User Group")]
        public sdtoUserGroup UserGroup { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [MaxLength(50)]
        public string Designation { get; set; }

        public UserType? UserType { get; set; }

        public long? UserContactId { get; set; }

        public long? UserAddressId { get; set; }

        public long? PermanentAddressId { get; set; }

        public long? GuaranterAddressId { get; set; }

        public long? PermanentContactId { get; set; }

        public long? GuaranterContactId { get; set; }

        [Display(Name = "Father's Name")]
        public string FatherName { get; set; }

        [Display(Name = "Guaranter Name")]
        public string GuaranterName { get; set; }

        public string Occupation { get; set; }

        //public DateTime CreatedOn { get; set; }

        [ForeignKey("UserContactId")]
        public virtual sdtoContact Contacts { get; set; }

        [ForeignKey("UserAddressId")]
        public virtual sdtoAddress UserAddress { get; set; }

        [Display(Name = "Permanent Address")]
        [ForeignKey("PermanentAddressId")]
        public virtual sdtoAddress PermanentAddress { get; set; }

         [Display(Name = "Guaranter Address")]
        [ForeignKey("GuaranterAddressId")]
        public virtual sdtoAddress GuaranterAddress { get; set; }

        [ForeignKey("PermanentContactId")]
        public virtual sdtoContact PermanentContacts { get; set; }

        [ForeignKey("GuaranterContactId")]
        public virtual sdtoContact GuaranterContacts { get; set; }

        public long? CompanyId { get; set; }
    }
}