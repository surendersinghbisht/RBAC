using Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.Dto;
using Service.Contract;
using Service.Implementation;

namespace RBAC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(
            IRoleService roleService
            ) {
            _roleService = roleService;
        }

        [HttpPost("add-roles")]
        public async Task<IActionResult> AddNewRole(RoleDt roleData)
        {
            if (roleData == null || string.IsNullOrWhiteSpace(roleData.Name))
                return BadRequest("Role data is invalid.");

            var result = await _roleService.AddNewRole(roleData);

            if (result)
                return Ok(new { message = "Role created successfully." });
            else
                return BadRequest(new { message = "Role creation failed or role already exists." });
        }

        [HttpGet("get-all-roles")]

        public async Task<ActionResult<List<RoleDt>>> GetAllRoles()
        {
            var result = await _roleService.GetAllRoles();

            return Ok(result);

        }

        [HttpDelete("delete-role/{id}")]
        public async Task<ActionResult> DeleteRole(string id)
        {
            var result = await _roleService.DeleteRole(id);

            if(result)
                return Ok(new { message = "Role deleted successfully." });
            else
                return BadRequest(new { message = "Role creation failed or role already exists." });
        }
    }
    }
