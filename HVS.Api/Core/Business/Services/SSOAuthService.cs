using HVS.Api.Core.Business.Models.Users;
using HVS.Api.Core.Common.Utilities;
using HVS.Api.Core.DataAccess.Repositories.Base;
using HVS.Api.Core.Entities;
using System.Linq;
using System.Threading.Tasks;
using HVS.Api.Core.Common.Helpers;

namespace HVS.Api.Core.Business.Services
{
    public interface ISSOAuthService
    {
        Task<ResponseModel> LoginAsync(UserLoginModel userLoginModel);

        ResponseModel VerifyTokenAsync(string token);
    }

    public class SSOAuthService : ISSOAuthService
    {
        private readonly IUserService _userService;
        private readonly IJwtHelper _jwtHelper;

        public SSOAuthService(IUserService userService, IJwtHelper jwtHelper)
        {
            _userService = userService;
            _jwtHelper = jwtHelper;
        }

        public async Task<ResponseModel> LoginAsync(UserLoginModel userLoginModel)
        {
            var user = await _userService.GetByMobileAsync(userLoginModel.Mobile);
            if (user != null)
            {
                var result = PasswordUtilities.ValidatePass(user.Password, userLoginModel.Password, user.PasswordSalt);
                if (result)
                {
                    if (!user.Verified)
                    {
                        var userViewModel = new UserViewModel(user);
                        return new ResponseModel()
                        {
                            StatusCode = System.Net.HttpStatusCode.OK,
                            Data = userViewModel
                        };
                    }
                    else
                    {
                        var jwtPayload = new JwtPayload()
                        {
                            UserId = user.Id,
                            Mobile = user.Mobile,
                            Name = user.Name,
                            RoleIds = user.UserInRoles != null ? user.UserInRoles.Select(x => x.RoleId).ToList() : null
                        };

                        var token = _jwtHelper.GenerateToken(jwtPayload);

                        return new ResponseModel()
                        {
                            StatusCode = System.Net.HttpStatusCode.OK,
                            Data = token
                        };
                    }
                }
                else
                {
                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Số điện thoại hoặc mật khẩu không đúng. Vui lòng thử lại!"// TODO: multi language
                    };
                }
            }
            else
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Số điện thoại chưa được đăng kí!"// TODO: multi language
                };
            }
        }

        public ResponseModel VerifyTokenAsync(string token)
        {
            var jwtPayload = _jwtHelper.ValidateToken(token);

            if (jwtPayload == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Unauthorized request"
                };
            }
            else
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = jwtPayload
                };
            }
        }
    }
}
