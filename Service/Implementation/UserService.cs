using Data.DbContext;
using Data.Entity;
using Data.Migrations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using Model.Dto;
using Service.Contract;

namespace Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IdentityDbContext _identityDbContext;
        private readonly string _webRootPath;
        public UserService(UserManager<IdentityUser> userManager,
            RoleManager<ApplicationRole> roleManager,
         IdentityDbContext identityDbContext,
         IWebHostEnvironment env
            )
        {
            _identityDbContext = identityDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _webRootPath = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }
        public async Task<IEnumerable<IdentityUserDto>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var allRoles = _roleManager.Roles.Where(r => r.Name != "Admin");

                var userDtoList = new List<IdentityUserDto>();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (!roles.Contains("Admin"))
                    {
                        userDtoList.Add(new IdentityUserDto
                        {
                            Email = user.Email,
                            Id = user.Id,
                            Role = roles.FirstOrDefault(),
                            UserName = user.UserName,
                        });
                    }
                }

                return userDtoList;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching users: {ex.Message}", ex);
            }
        }

        public async Task<IdentityUserDto> GetMyDetails(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return new IdentityUserDto
            {
                Email = user.Email,
                Id = user.Id,
                UserName = user.UserName
            };
        }

        public async Task<IdentityUserDto> UpdateRole(UpdateRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return null;

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Contains(model.NewRole))
                return new IdentityUserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = model.NewRole
                };

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                throw new Exception($"Failed to remove roles: {string.Join(", ", removeResult.Errors.Select(e => e.Description))}");
            }

            var addResult = await _userManager.AddToRoleAsync(user, model.NewRole);
            if (!addResult.Succeeded)
            {
                throw new Exception($"Failed to add role: {string.Join(", ", addResult.Errors.Select(e => e.Description))}");
            }

            return new IdentityUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = model.NewRole
            };
        }

        public async Task<bool> UpdateUserDetailsAsync(UserDetailsDto dto)
        {
            if (string.IsNullOrEmpty(dto.IdentityUserId))
                return false;

            var details = await _identityDbContext.userDetails
                .FirstOrDefaultAsync(u => u.IdentityUserId == dto.IdentityUserId);

            if (details == null) return false;

            if (!string.IsNullOrEmpty(dto.FirstName)) details.FirstName = dto.FirstName;
            if (!string.IsNullOrEmpty(dto.LastName)) details.LastName = dto.LastName;
            if (dto.Dob.HasValue) details.Dob = dto.Dob;

            if (dto.ProfilePic != null && dto.ProfilePic.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webRootPath, "uploads", "profile-pics");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{dto.ProfilePic.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ProfilePic.CopyToAsync(stream);
                }

                details.ProfilePicUrl = $"/uploads/profile-pics/{fileName}";
            }

            _identityDbContext.userDetails.Update(details);
            await _identityDbContext.SaveChangesAsync();

            return true;
        }



        public async Task<UserDetailsDto> GetUserDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return null;

            var userDetails = await _identityDbContext.userDetails
                .FirstOrDefaultAsync(u => u.IdentityUserId == id);

            if (userDetails == null)
                return null;

            return new UserDetailsDto
            {
                Id = userDetails?.Id ?? 0,
                IdentityUserId = user?.Id ?? string.Empty,
                FirstName = userDetails?.FirstName,
                LastName = userDetails?.LastName,
                Dob = userDetails?.Dob ?? default,
                ProfilePicUrl = userDetails?.ProfilePicUrl
            };
        }
    }
    }

    
        



