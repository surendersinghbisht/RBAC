using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Model.Dto;
using Service.Contract;
using Service.Helper;

namespace RBAC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TwoFactorService _twoFactorService;
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService, TwoFactorService twoFactorService) { 
        _authService = authService;
            _twoFactorService = twoFactorService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerdt)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            try
            {
                var result = await _authService.RegisterAsync(registerdt);

                if (result.Contains("Error") || result.Contains("already"))
                    return BadRequest(new { message = result });

                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }


        [HttpPost("login")]

        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDt)
        {
            try
            {
                var loggedIn = await _authService.LoginAsync(loginDt);
                    return Ok(loggedIn);

            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new LoginResponseDto
                {
                    Success = false,
                    Message = $"Server error: {ex.Message}",
                    Token = null
                });
            }
        }

        [HttpPost("enable-2fa")]
        public async Task<ActionResult<bool>> EnableTwoFactor(string email, bool enable)
        {
            try
            {
                var result = await _authService.EnableTwoFactorAuthenticationAsync(email, enable);

                if (!result)
                {
                    return BadRequest($"Failed to {(enable ? "enable" : "disable")} two-factor authentication.");
                }

                return Ok(result); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating two-factor authentication.");
            }
        }


        [HttpPost("verify-2fa")]
        public async Task<ActionResult<LoginResponseDto>> VerifyTwoFA(OtpRequestModel model)
        {
            var result = await _twoFactorService.VerifyTwoFactorCodeAsync(model.UserId, model.Code);

            if (!result.Success)
                return BadRequest(result);  

            return Ok(result);  
        }


    }
}
