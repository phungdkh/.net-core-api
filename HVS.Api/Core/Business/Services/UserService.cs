using System;
using System.Linq;
using HVS.Api.Core.Business.Models.Users;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using HVS.Api.Core.Business.Models.Base;
using System.Collections.Generic;
using HVS.Api.Core.DataAccess.Repositories.Base;
using HVS.Api.Core.Entities;
using HVS.Api.Core.Common.Utilities;
using HVS.Api.Core.Common.Constants;
using HVS.Api.Core.Business.Models;
using System.Threading.Tasks;
using HVS.Api.Core.Common.Helpers;
using HVS.Api.Core.Common.Reflections;

namespace HVS.Api.Core.Business.Services
{
    public interface IUserService
    {
        Task<PagedList<UserViewModel>> ListUserAsync(UserRequestListViewModel userRequestListViewModel);

        Task<ResponseModel> RegisterAsync(UserRegisterModel userRegisterModel);

        Task<ResponseModel> UpdateProfileAsync(Guid id, UserUpdateProfileModel userUpdateProfileModel);

        Task<ResponseModel> DeleteUserAsync(Guid id);

        Task<User> GetByMobileAsync(string email);

        Task<User> GetByIdAsync(Guid? id);

        Task<ResponseModel> VerifyCodeAsync(Guid userId, string code);

        Task<ResponseModel> ResendCodeAsync(Guid userId);
    }

    public class UserService : IUserService
    {
        #region Fields

        private readonly IRepository<User> _userRepository;
        private readonly ILogger _logger;
        private readonly IOptions<AppSettings> _appSetting;
        private readonly IJwtHelper _jwtHelper;

        #endregion

        #region Constructor

        public UserService(IRepository<User> userRepository, ILogger<UserService> logger,
            IOptions<AppSettings> appSetting, IJwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _appSetting = appSetting;
            _jwtHelper = jwtHelper;
        }

        #endregion

        #region Base Methods

        #endregion

        #region Private Methods

        private IQueryable<User> GetAll()
        {
            return _userRepository.GetAll()
                        .Include(x => x.UserInRoles)
                            .ThenInclude(user => user.Role)
                    .Where(x => !x.RecordDeleted);
        }

        private List<string> GetAllPropertyNameOfUserViewModel()
        {
            var userViewModel = new UserViewModel();
            var type = userViewModel.GetType();

            return ReflectionUtilities.GetAllPropertyNamesOfType(type);
        }

        #endregion

        #region Other Methods

        public async Task<PagedList<UserViewModel>> ListUserAsync(UserRequestListViewModel userRequestListViewModel)
        {
            var list = await GetAll()
            .Where(x => (!userRequestListViewModel.IsActive.HasValue || x.RecordActive == userRequestListViewModel.IsActive)
                && (string.IsNullOrEmpty(userRequestListViewModel.Query)
                    || (x.Name.Contains(userRequestListViewModel.Query)
                    || (x.Email.Contains(userRequestListViewModel.Query)
                    ))))
                .Select(x => new UserViewModel(x)).ToListAsync();

            var userViewModelProperties = GetAllPropertyNameOfUserViewModel();
            var requestPropertyName = !string.IsNullOrEmpty(userRequestListViewModel.SortName) ? userRequestListViewModel.SortName.ToLower() : string.Empty;
            string matchedPropertyName = string.Empty;

            foreach (var userViewModelProperty in userViewModelProperties)
            {
                var lowerTypeViewModelProperty = userViewModelProperty.ToLower();
                if (lowerTypeViewModelProperty.Equals(requestPropertyName))
                {
                    matchedPropertyName = userViewModelProperty;
                    break;
                }
            }

            if (string.IsNullOrEmpty(matchedPropertyName))
            {
                matchedPropertyName = "Name";
            }

            var type = typeof(UserViewModel);
            var sortProperty = type.GetProperty(matchedPropertyName);

            list = userRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();

            return new PagedList<UserViewModel>(list, userRequestListViewModel.Offset ?? CommonConstants.Config.DEFAULT_SKIP, userRequestListViewModel.Limit ?? CommonConstants.Config.DEFAULT_TAKE);
        }

        public async Task<ResponseModel> RegisterAsync(UserRegisterModel userRegisterModel)
        {
            var user = await _userRepository.FetchFirstAsync(x => x.Mobile == userRegisterModel.Mobile);
            if (user != null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Số điện thoại đã được đăng kí. Sử dụng chức năng quên mật khẩu để lấy lại!"// TODO: multi language
                };
            }
            else
            {
                user = AutoMapper.Mapper.Map<User>(userRegisterModel);
                userRegisterModel.Password.GeneratePassword(out string saltKey, out string hashPass);

                user.Password = hashPass;
                user.PasswordSalt = saltKey;

                var verifyCode = SMSHelper.GenerateVerifyNumber();
                user.VerifyCode = verifyCode;

                SMSHelper.SendMessage(verifyCode, user.Mobile);

                return await _userRepository.InsertAsync(user);
            }
        }

        public async Task<ResponseModel> UpdateProfileAsync(Guid id, UserUpdateProfileModel userUpdateProfileModel)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "User không tồn tại trong hệ thống. Vui lòng kiểm tra lại!"
                };
            }
            else
            {
                user = userUpdateProfileModel.GetUserFromModel(user);
                return await _userRepository.UpdateAsync(user);
            }
        }

        public async Task<ResponseModel> VerifyCodeAsync(Guid userId, string code)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "User không tồn tại trong hệ thống. Vui lòng kiểm tra lại!"
                };
            }
            else
            {
                if (user.VerifyCode != code)
                {
                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Mã xác nhận không hợp lệ!"
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
        }

        public async Task<ResponseModel> ResendCodeAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "User không tồn tại trong hệ thống. Vui lòng kiểm tra lại!"
                };
            }
            else
            {
                var verifyCode = SMSHelper.GenerateVerifyNumber();
                user.VerifyCode = verifyCode;
                await _userRepository.UpdateAsync(user);

                SMSHelper.SendMessage(verifyCode, user.Mobile);

                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Đã gửi mã xác nhận tới số điện thoại đăng ký!"
                };
            }
        }

        public async Task<ResponseModel> DeleteUserAsync(Guid id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        public async Task<User> GetByMobileAsync(string mobile)
        {
            return await _userRepository.FetchFirstAsync(x => x.Mobile == mobile);
        }

        public async Task<User> GetByIdAsync(Guid? id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        #endregion
    }
}
