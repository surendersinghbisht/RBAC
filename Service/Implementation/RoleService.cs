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
    public class RoleService:IRoleService
    {
        private RoleManager<ApplicationRole> _roleManager;
        public RoleService(
            RoleManager<ApplicationRole> roleManager) {
        _roleManager = roleManager;
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

    }
}
