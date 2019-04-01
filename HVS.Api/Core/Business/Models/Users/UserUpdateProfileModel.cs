using HVS.Api.Core.Entities;
using HVS.Api.Core.Entities.Enums;
using System;

namespace HVS.Api.Core.Business.Models.Users
{
    public class UserUpdateProfileModel
    {
        public string Name { get; set; }

        public string Mobile { get; set; }

        public string Avatar { get; set; }

        public string Jobs { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public UserEnums.Gender Gender { get; set; }

        public string IdentityNumber { get; set; }

        public string Nationality { get; set; }

        public Guid? BankingId { get; set; }

        public string BankingNumber { get; set; }

        public string[] InvestTypes { get; set; }

        public string[] InvestExchanges { get; set; }


        public User GetUserFromModel(User user)
        {
            user.Name = Name;
            user.Mobile = Mobile;
            user.AvatarUrl = Avatar;
            user.DateOfBirth = DateOfBirth;
            user.Gender = Gender;

            return user;
        }
    }
}
