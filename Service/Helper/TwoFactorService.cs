//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Service.Helper;
using System;
using System.Linq;
using Data.DbContext;
using Data.Entity;
using Microsoft.EntityFrameworkCore;
using Model.Dto;
using Microsoft.AspNetCore.Identity;
using Contract.ITokenService;

namespace Service.Helper
{
    public class TwoFactorService
    {
        private readonly EmailService _emailService;
        private readonly IdentityDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;

        public TwoFactorService(EmailService emailService, IdentityDbContext context,
            UserManager<IdentityUser> userManager,
            ITokenService tokenService
            )
        {
            _emailService = emailService;
            _context = context;
            _userManager = userManager;
           _tokenService = tokenService;
        }

      
        public string GenerateTwoFactorCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public async Task<bool> SendTwoFactorCodeAsync(string userEmail, string userId)
        {
            Console.WriteLine($"email is {userEmail} and id is {userId}");
            var code = GenerateTwoFactorCode();

            
            var existingTokens = await _context.TwoFaTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            if (existingTokens.Any())
            {
                _context.TwoFaTokens.RemoveRange(existingTokens);
            }

           
            var token = new TwoFaToken
            {
                UserId = userId,
                Token = code,
                ExpiryDate = DateTime.UtcNow.AddMinutes(5)
            };

            await _context.TwoFaTokens.AddAsync(token);
            await _context.SaveChangesAsync();

       
            return await _emailService.SendTwoFactorCodeEmailAsync(userEmail, code);
        }

        public async Task<LoginResponseDto> VerifyTwoFactorCodeAsync(string userId, string inputCode)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new LoginResponseDto
                {
                    Message = "User not found",
                    Success = false
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (string.IsNullOrWhiteSpace(inputCode))
            {
                return new LoginResponseDto
                {
                    Email = user.Email,
                    Roles = roles.ToList(),
                    Message = "2FA code is required",
                    Success = false,
                    TwoFactorRequired = user.TwoFactorEnabled,
                    Token = null,
                    userId = userId
                };
            }

            var token = await _context.TwoFaTokens
                .FirstOrDefaultAsync(t => t.UserId == userId && t.Token == inputCode);

            if (token == null || token.ExpiryDate < DateTime.UtcNow)
            {
                return new LoginResponseDto
                {
                    Email = user.Email,
                    Roles = roles.ToList(),
                    Message = token == null ? "Invalid 2FA code" : "2FA code expired",
                    Success = false,
                    TwoFactorRequired = user.TwoFactorEnabled,
                    Token = null,
                    userId = userId
                };
            }

          
            var jwtToken = _tokenService.GenerateJwtToken(user, roles);

      
            _context.TwoFaTokens.Remove(token);
            await _context.SaveChangesAsync();

            return new LoginResponseDto
            {
                Email = user.Email,
                Roles = roles.ToList(),
                Message = "Login successful",
                Success = true,
                TwoFactorRequired = user.TwoFactorEnabled,
                Token = jwtToken,
                userId = userId
            };
        }

    }
}
