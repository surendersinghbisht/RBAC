using Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contract
{
    public interface IRoleService
    {
        Task<bool> AddNewRole(RoleDt roleDt);
        Task<List<RoleDt>> GetAllRoles();
        Task<bool> DeleteRole(string id);

        Task<bool> UpdateRoleActiveStatus(RoleDt roledt);
        Task<List<RoleDt>> GetAllActiveRoles();
    }
}
