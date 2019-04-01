using HVS.Api.Core.Business.Models.Users;
using HVS.Api.Core.Business.Services;
using HVS.Api.Core.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HVS.Api.Core.Common.Helpers;

namespace HVS.Api.Controllers
{
    [Route("api/sso")]
    [EnableCors("CorsPolicy")]
    public class SSOController : Controller
    {
        private readonly ISSOAuthService _ssoService;
        private readonly IUserService _userService;
        private readonly IJwtHelper _jwtHelper;

        public SSOController(ISSOAuthService ssoService, IUserService userService, IJwtHelper jwtHelper)
        {
            _ssoService = ssoService;
            _userService = userService;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel userRegisterModel)
        {
            var responseModel = await _userService.RegisterAsync(userRegisterModel);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var user = (User)responseModel.Data;
                return Ok(new UserViewModel(user));
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel userLoginModel)
        {
            var responseModel = await _ssoService.LoginAsync(userLoginModel);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel.Data);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }

        [HttpGet("verify-token/{token}")]
        public IActionResult VerifyToken(string token)
        {
            var responseModel = _ssoService.VerifyTokenAsync(token);
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
}