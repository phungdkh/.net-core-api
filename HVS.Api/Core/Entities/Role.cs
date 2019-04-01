using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HVS.Api.Core.Entities
{
    [Table("Role")]
    public class Role : BaseEntity
    {
        public Role() : base()
        {

        }

        public string Name { get; set; }
		public List<UserInRole> UserInRoles { get; set; }
	}
}
