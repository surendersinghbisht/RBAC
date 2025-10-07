using Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.Dto;
using Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class RoleService: IRoleService
    {
        private RoleManager<ApplicationRole> _roleManager;
        private UserManager<IdentityUser> _userManager;
        public RoleService(
            RoleManager<ApplicationRole> roleManager,
            UserManager<IdentityUser> userManager) {
        _roleManager = roleManager;
            _userManager = userManager;
        }

       public async Task<bool> AddNewRole(RoleDt roleDt)
        {
            var existingRole = await _roleManager.FindByNameAsync(roleDt.Name);
            if (existingRole != null)
                return false;

            var newRole = new ApplicationRole
            {
                Name = roleDt.Name,
                IsActive = true,
                ShortDescription = roleDt.ShortDescription,
                CreatedOn = DateTime.UtcNow
            };

         var result =  await _roleManager.CreateAsync(newRole);
            if (!result.Succeeded)
                return false;

            return true;
        }

        public async Task<List<RoleDt>> GetAllActiveRoles()
        {
            var activeRoles = _roleManager.Roles.Where(r => r.IsActive == true).ToList();
            var activeRolesList = new List<RoleDt>();
            foreach (var role in activeRoles) {
                var createdRoleDt = new RoleDt
                {
                    Id = role.Id,
                    IsActive = role.IsActive,
                    Name = role.Name,
                    ShortDescription = role.ShortDescription
                };
                activeRolesList.Add(createdRoleDt);
            }
            return activeRolesList;
        }

        public async Task<List<RoleDt>> GetAllRoles()
        {
            var roles = await _roleManager.Roles
                 .OrderByDescending(r => r.CreatedOn)
                .Select(r => new RoleDt
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsActive = r.IsActive,
                    ShortDescription = r.ShortDescription,
                }).ToListAsync();

            return roles;
        }

        public async Task<bool> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return false;
            }

            var result = await _roleManager.DeleteAsync(role);

            return result.Succeeded;
        }

        public async Task<bool> UpdateRoleActiveStatus(RoleDt roledt)
        {
            var role = await _roleManager.FindByIdAsync(roledt.Id);
            if (role == null) return false;

            role.IsActive = roledt.IsActive;
            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded) return false;

            if (role.IsActive == false)
            {
                var users = await _userManager.GetUsersInRoleAsync(role.Name);

                var defaultRoleExists = await _roleManager.RoleExistsAsync("User");

                if (!defaultRoleExists)
                {
                    await _roleManager.CreateAsync(new ApplicationRole { Name = "User", IsActive = true });

                }

                foreach (var user in users)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);

                    await _userManager.AddToRoleAsync(user, "User");
                }

            }

            return true;
        }


    }
}
