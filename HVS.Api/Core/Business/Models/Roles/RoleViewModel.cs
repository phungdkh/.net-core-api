using HVS.Api.Core.Business.Models.Base;
using HVS.Api.Core.Entities;

namespace HVS.Api.Core.Business.Models.Roles
{
    public class RoleViewModel : ApiBaseModel
    {
        public RoleViewModel() : base()
        {

        }

        public RoleViewModel(Role role)
        {
            if (role != null)
            {
                Id = role.Id;
                Name = role.Name;
            }
        }
    }
}
