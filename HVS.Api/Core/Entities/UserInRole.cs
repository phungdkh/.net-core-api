using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HVS.Api.Core.Entities
{
    [Table("UserInRole")]
    public class UserInRole : BaseEntity
    {
        public UserInRole() : base()
        {

        }

        public Guid UserId { get; set; }
		public User User { get; set; }
		public Guid RoleId { get; set; }
		public Role Role { get; set; }
	}
}
