using Data.Entity;
using Data.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.Dto;
using Service.Contract;

namespace Service.Implementation
{
    public class UserService: IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UserService(UserManager<IdentityUser> userManager,
            RoleManager<ApplicationRole> roleManager
            ) {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IEnumerable<IdentityUserDto>> GetAllUsersAsync()
        {
            try
            {
               


                var users = await _userManager.Users.ToListAsync();
                var allRoles = _roleManager.Roles.Where(r => r.Name != "Admin");

                var userDtoList = new List<IdentityUserDto>();
                foreach (var user in users) {
                    var roles = await _userManager.GetRolesAsync(user);
                    if(!roles.Contains("Admin"))
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
    };
        }


