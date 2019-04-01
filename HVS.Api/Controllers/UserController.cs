using Microsoft.AspNetCore.Mvc;
using HVS.Api.Core.Business.Services;
using System;
using Microsoft.AspNetCore.Cors;
using HVS.Api.Core.Business.Models.Users;
using HVS.Api.Core.Business.Filters;
using System.Threading.Tasks;

namespace HVS.Api.Controllers
{
    [Route("api/users")]
    [EnableCors("CorsPolicy")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public UserController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(UserRequestListViewModel userRequestListViewModel)
        {
            var users = await _userService.ListUserAsync(userRequestListViewModel);
            return Ok(users);
        }

        [HttpPut("{id}")]
        [CustomAuthorize]
        public async Task<IActionResult> Put(Guid id, [FromBody] UserUpdateProfileModel userUpdateProfileModel)
        {
            var responseModel = await _userService.UpdateProfileAsync(id, userUpdateProfileModel);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound("User không tồn tại trong hệ thống. Vui lòng kiểm tra lại!");
            }
            else
            {
                if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Ok(responseModel.Data);
                }
                else
                {
                    return BadRequest(new { Message = responseModel.Message });
                }
            }
        }

        [HttpDelete("{id}")]
        [CustomAuthorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var responseModel = await _userService.DeleteUserAsync(id);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel.Data);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }

        #region Other Methods

        [HttpGet("check-existing-mobile")]
        public async Task<IActionResult> ValidateExistMobile([FromBody] string mobile)
        {
            var user = await _userService.GetByMobileAsync(mobile);
            return Ok(user != null);
        }

        [HttpPost("verify-code/{userId}")]
        public async Task<IActionResult> VerifyCode(Guid userId, [FromBody] VerifyCodeModel verifyCodeModel)
        {
            var responseModel = await _userService.VerifyCodeAsync(userId, verifyCodeModel.Code);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel.Data);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }

        [HttpGet("resend-code/{userId}")]
        public async Task<IActionResult> ResendCode(Guid userId)
        {
            var responseModel = await _userService.ResendCodeAsync(userId);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel.Message);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }

        #endregion
    }
}
