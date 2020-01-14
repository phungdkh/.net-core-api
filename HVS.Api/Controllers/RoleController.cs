namespace HVS.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using HVS.Api.Core.Business.Models.Roles;
    using HVS.Api.Core.Business.Services;
    using System.Threading.Tasks;
    using HVS.Api.Core.Business.Filters;

    [Route("api/roles")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;

        }

        [HttpGet]
        public async Task<IActionResult> Get(RoleRequestListViewModel roleRequestListViewModel)
        {
            var roles = await _roleService.ListRoleAsync(roleRequestListViewModel);
            return Ok(roles);
        }

        [HttpGet("by-name")]
        public async Task<IActionResult> GetByName(string name)
        {
            var responseModel = await _roleService.GetRoleByNameAsync(name);
            return new CustomActionResult(responseModel);
        }
    }
}