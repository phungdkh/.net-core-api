using HVS.Api.Core.Business.Models.Base;
using HVS.Api.Core.Business.Models.Roles;
using HVS.Api.Core.Entities;
using HVS.Api.Core.Entities.Enums;
using System;
using System.Linq;

namespace HVS.Api.Core.Business.Models.Users
{
    public class UserViewDetailModel
    {
        public UserViewDetailModel()
        {

        }

        public UserViewDetailModel(User user) : this()
        {
            if (user != null)
            {
                Id = user.Id;
                Name = user.Name;
                Mobile = user.Mobile;
                Avatar = user.AvatarUrl;
                DateOfBirth = user.DateOfBirth;
                JoinDate = user.CreatedOn;
                Gender = user.Gender;
                Roles = user.UserInRoles != null ? user.UserInRoles.Select(y => new RoleViewModel(y.Role)).ToArray() : null;
            }
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Avatar { get; set; }
        public string Jobs { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? JoinDate { get; set; }
        public UserEnums.Gender Gender { get; set; }
        public RoleViewModel[] Roles { get; set; }
    }
}
