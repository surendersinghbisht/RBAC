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
        private readonly IWebHostEnvironment _env;
        public UserController(IUserService userService
            , IWebHostEnvironment env
            ) {
            _userService = userService;
            _env = env;
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
        [HttpPost("UpdateUserDetails")]
        public async Task<IActionResult> UpdateUserDetails([FromForm] UserDetailsDto dto)
        {
            if (dto == null)
                return BadRequest("No data provided.");

            var result = await _userService.UpdateUserDetailsAsync(dto);

            if (!result)
                return BadRequest("Failed to update user details.");

            return Ok(new { message = "User details updated successfully." });
        }




        [HttpGet("get-user-details/{userId}")]
        public async Task<ActionResult<UserDetailsDto>> GetUserDetailsAsync(string userId)
        {
            var details = await _userService.GetUserDetails(userId);
            if (details == null)
                return NotFound("User not found");

            return Ok(details);
        }
    }


    }


