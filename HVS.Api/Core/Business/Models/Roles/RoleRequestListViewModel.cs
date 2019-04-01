using HVS.Api.Core.Business.Models.Base;

namespace HVS.Api.Core.Business.Models.Roles
{
    public class RoleRequestListViewModel : RequestListViewModel
    {
        public RoleRequestListViewModel() : base()
        {

        }

        public string Query { get; set; }
        public bool? IsActive { get; set; }
    }
}
