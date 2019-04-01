using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HVS.Api.Core.Common.Constants
{
    public static class ErrorConstants
    {
        public class Common
        {
            public const string InternalServerError = "Internal server error.";
        }
        public class User
        {
            public const string UserNotFound = "User was not found.";
            public const string WrongUserNameOrPassword = "Your Username or Password is incorrect.";
            public const string AddUserFail = "Add user to database fail.";
            public const string UnAuthorized = "Unauthorized request";
            public const string InvalidAccessToken = "Access token is invalid.";
            public const string UserDoesNotHaveRole = "The user does not have the role.";
        }
    }
}
