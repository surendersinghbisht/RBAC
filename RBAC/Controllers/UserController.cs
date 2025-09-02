using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Dto;
using Service.Contract;
using Service.Implementation;



namespace RBAC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
       private readonly IUserService _userService;
        public UserController(IUserService userService) {
            _userService = userService;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("all-users")]
        public async Task<ActionResult<IEnumerable<IdentityUserDto>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();

                if (users == null || !users.Any())
                {
                    return NotFound(new { Message = "No users found" });
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<IdentityUserDto>> GetMyDetails()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "User not authenticated" });

            var user = await _userService.GetMyDetails(userId);

            if (user == null) return NotFound(new { Message = "User not found" });

            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("update-role")]
        public async Task<ActionResult<IdentityUserDto>> UpdateRole(UpdateRoleModel model)
        {
            try
            {
                var result = await _userService.UpdateRole(model);

                if (result == null)
                {
                    return NotFound(new { Message = "User not found or role unchanged" });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error updating role: {ex.Message}" });
            }
        }


    }

}


