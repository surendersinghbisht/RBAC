using Contract.ITokenService;
using Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Model.Dto;
using Service.Contract;
using Service.Helper;

namespace Service.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly TwoFactorService _twoFactorService;
        public AuthService(UserManager<IdentityUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration config,
            ITokenService tokenService,
            TwoFactorService twoFactorService
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _tokenService = tokenService;
            _twoFactorService = twoFactorService;
        }

        public async Task<string> RegisterAsync(RegisterDto registerDt)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(registerDt.Email);
                if (existingUser != null)
                {
                    return "Email already exists";
                }
                
                var user = new IdentityUser
                {
                    UserName = registerDt.UserName,
                    Email = registerDt.Email,
                    EmailConfirmed = true
                };


                var result = await _userManager.CreateAsync(user, registerDt.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return $"Error creating user: {errors}";
                }


                await _userManager.AddToRoleAsync(user, "User");

                return "User registered successfully!";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDt)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(loginDt.Email);
                if (existingUser == null)
                {
                    return new LoginResponseDto
                    {
                        Email = loginDt.Email,
                        Message = "User Not Found",
                        Success = false,
                    };
                }

                var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser, loginDt.Password);
                if (!isPasswordValid)
                {
                    return new LoginResponseDto
                    {
                        Email = loginDt.Email,
                        Message = "Invalid Password",
                        Success = false,
                    };
                }

                
                var roles = await _userManager.GetRolesAsync(existingUser);
                if (await _userManager.GetTwoFactorEnabledAsync(existingUser))
                {
                    var otp = _twoFactorService.GenerateTwoFactorCode();   
                    await _twoFactorService.SendTwoFactorCodeAsync(existingUser.Email, existingUser.Id);
                   
                    return new LoginResponseDto
                    {
                        Email = existingUser.Email,
                        Success = false,
                        Message = "2FA required",
                        TwoFactorRequired = true,
                        userId = existingUser.Id,
                        Roles = roles
                    };
                }

                
                var token = _tokenService.GenerateJwtToken(existingUser, roles);

                return new LoginResponseDto
                {
                    userId = existingUser.Id,
                    Email = loginDt.Email,
                    Success = true,
                    Message = "Login successful",
                    Token = token,
                    Roles = roles
                };
            }
            catch (Exception ex)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Token = null
                };
            }
        }


        public async Task<bool> EnableTwoFactorAuthenticationAsync(string email, bool enable)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return false;
                }
                var result = await _userManager.SetTwoFactorEnabledAsync(user, enable);

                return result.Succeeded;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ForgetPassword(ForgetPassword forgetPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgetPassword.Email);
            if (user == null)
                return false;

            if(forgetPassword.NewPassword != forgetPassword.ConfirmPassword)
            {
                return false;
            }
            // Generate a password reset token
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Reset the password using the token
            var result = await _userManager.ResetPasswordAsync(user, resetToken, forgetPassword.ConfirmPassword);

            return result.Succeeded;
        }

        //public async Task<LoginResponseDto> ValidateTwoFactorCode(TwoFactorDto dto)
        //{
        //    var user = await _userManager.FindByEmailAsync(dto.Email);
        //    if (user == null)
        //    {
        //        return new LoginResponseDto
        //        {
        //            Email = dto.Email,
        //            Success = false,
        //            Message = "User not found",
        //            Token = null
        //        };
        //    }

        //    if (!_cache.TryGetValue($"2fa_{user.Id}", out string savedCode))
        //    {
        //        return new LoginResponseDto
        //        {
        //            Email = dto.Email,
        //            Success = false,
        //            Message = "OTP expired or not found",
        //            Token = null
        //        };
        //    }

        //    if (savedCode != dto.Otp)
        //    {
        //        return new LoginResponseDto
        //        {
        //            Email = dto.Email,
        //            Success = false,
        //            Message = "Invalid OTP",
        //            Token = null
        //        };
        //    }

        //    _cache.Remove($"2fa_{user.Id}");

        //    var roles = await _userManager.GetRolesAsync(user);
        //    var token = _tokenService.GenerateJwtToken(user, roles);

        //    return new LoginResponseDto
        //    {
        //        Email = user.Email,
        //        Success = true,
        //        Message = "Login successful",
        //        Token = token
        //    };
        //}
    }

}
