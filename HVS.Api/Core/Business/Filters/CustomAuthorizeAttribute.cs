using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HVS.Api.Core.Common.Constants;
using System;
using System.Net;
using System.Linq;
using HVS.Api.Core.Common.Helpers;
using System.Collections.Generic;

namespace HVS.Api.Core.Business.Filters
{
    /// <summary>
    /// 
    /// </summary>
	public class CustomAuthorizeAttribute : ActionFilterAttribute
    {
        //private readonly UserHelpers _userHelpers = IoCHelper.GetInstance<UserHelpers>();
        public string Roles { get; set; }
        /// <summary>
        /// Override OnActionExecuting to check Access Token and Role
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var jwtHelper = (IJwtHelper)context.HttpContext.RequestServices.GetService(typeof(IJwtHelper));
            var accessToken = context.HttpContext.Request.Headers["x-access-token"].ToString();
            var jwtPayload = jwtHelper.ValidateToken(accessToken);

            if (jwtPayload == null)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new JsonResult(MessageConstants.INVALID_ACCESS_TOKEN);
            }
            else if (!string.IsNullOrEmpty(Roles))
            {
                bool isUserInRole = IsUserInRole(Roles, jwtPayload.RoleIds);
                if (!isUserInRole)
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Result = new JsonResult("Unauthorized request");
                }
            }
            base.OnActionExecuting(context);
        }

        private bool IsUserInRole(string allowRoles, List<Guid> currentRoleIds)
        {
            if (currentRoleIds == null || currentRoleIds.Count <= 0)
            {
                return false;
            }

            foreach (var role in allowRoles.Split(','))
            {
                var roleId = Guid.Parse(role);
                return currentRoleIds.Any(x => x == roleId);
            }
            return false;
        }
    }
}
