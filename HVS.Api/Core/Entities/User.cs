using HVS.Api.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HVS.Api.Core.Entities
{
    [Table("User")]
    public class User : BaseEntity
    {
        #region Constructors

        public User() : base()
        {

        }

        public User(string email)
            : this()
        {
            Email = email;
        }

        #endregion

        #region Properties

        public int? Age
        {
            get
            {
                if (DateOfBirth.HasValue)
                {
                    return (int)(DateTime.Now - DateOfBirth.Value).TotalDays / 365;
                }
                return null;
            }
        }

        #region Base Properties

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(512)]
        [Required]
        public string Password { get; set; }

        [StringLength(512)]
        [Required]
        public string PasswordSalt { get; set; }

        [StringLength(512)]
        [Required]
        public string Name { get; set; }

        [StringLength(50)]
        [Required]
        public string Mobile { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public UserEnums.Gender Gender { get; set; }

        public string About { get; set; }

        [StringLength(512)]
        public string AvatarUrl { get; set; }

        [StringLength(1024)]
        public string Address { get; set; }

        [StringLength(512)]
        public string Facebook { get; set; }

        [StringLength(512)]
        public string Twitter { get; set; }

        [StringLength(512)]
        public string LinkedIn { get; set; }

        public string ResetPasswordCode { get; set; }

        public DateTime? ResetPasswordExpiryDate { get; set; }

        public bool Verified { get; set; }

        public string VerifyCode { get; set; }

        #endregion

        public List<UserInRole> UserInRoles { get; set; }

        #endregion
    }
}