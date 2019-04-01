using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HVS.Api.Core.Business.Models;
using HVS.Api.Core.Business.Models.Base;
using HVS.Api.Core.Business.Models.Roles;
using HVS.Api.Core.Common.Constants;
using HVS.Api.Core.Common.Reflections;
using HVS.Api.Core.DataAccess.Repositories;
using HVS.Api.Core.DataAccess.Repositories.Base;
using HVS.Api.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HVS.Api.Core.Business.Services
{
    public interface IRoleService
    {
        Task<PagedList<RoleViewModel>> ListRoleAsync(RoleRequestListViewModel userRequestListViewModel);
        Task<RoleViewModel> GetRoleByNameAsync(string name);
    }

    public class RoleService : IRoleService
    {
        #region Fields

        private readonly IRepository<Role> _roleRepository;
        private readonly ILogger _logger;
        private readonly IOptions<AppSettings> _appSetting;

        #endregion

        #region Constructor

        public RoleService(IRepository<Role> roleRepository, ILogger<RoleService> logger, IOptions<AppSettings> appSetting
            )
        {
            _roleRepository = roleRepository;
            _logger = logger;
            _appSetting = appSetting;
        }

        #endregion

        public async Task<PagedList<RoleViewModel>> ListRoleAsync(RoleRequestListViewModel roleRequestListViewModel)
        {
            var list = await GetAll().Where(x => (string.IsNullOrEmpty(roleRequestListViewModel.Query)
                        || (x.Name.Contains(roleRequestListViewModel.Query)))
                    ).Select(x => new RoleViewModel(x)).ToListAsync();

            var roleViewModelProperties = GetAllPropertyNameOfViewModel();
            var requestPropertyName = !string.IsNullOrEmpty(roleRequestListViewModel.SortName) ? roleRequestListViewModel.SortName.ToLower() : string.Empty;
            string matchedPropertyName = string.Empty;

            foreach (var roleViewModelProperty in roleViewModelProperties)
            {
                var lowerTypeViewModelProperty = roleViewModelProperty.ToLower();
                if (lowerTypeViewModelProperty.Equals(requestPropertyName))
                {
                    matchedPropertyName = roleViewModelProperty;
                    break;
                }
            }

            if (string.IsNullOrEmpty(matchedPropertyName))
            {
                matchedPropertyName = "Name";
            }

            var type = typeof(RoleViewModel);
            var sortProperty = type.GetProperty(matchedPropertyName);

            list = roleRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();

            return new PagedList<RoleViewModel>(list, roleRequestListViewModel.Offset ?? CommonConstants.Config.DEFAULT_SKIP, roleRequestListViewModel.Limit ?? CommonConstants.Config.DEFAULT_TAKE);
        }

        public async Task<RoleViewModel> GetRoleByNameAsync(string name)
        {
            var role = await _roleRepository.FetchFirstAsync(x => x.Name == name);
            if (role == null)
            {
                return null;
            }
            else
            {
                return new RoleViewModel(role);
            }
        }

        #region Private Methods

        public IQueryable<Role> GetAll()
        {
            return _roleRepository.GetAll().Where(x => !x.RecordDeleted);
        }

        private List<string> GetAllPropertyNameOfViewModel()
        {
            var userViewModel = new RoleViewModel();
            var type = userViewModel.GetType();

            return ReflectionUtilities.GetAllPropertyNamesOfType(type);
        }

        #endregion
    }
}
