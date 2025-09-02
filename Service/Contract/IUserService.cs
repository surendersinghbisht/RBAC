using Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contract
{
    public interface IUserService
    {
        Task<IEnumerable<IdentityUserDto>> GetAllUsersAsync();
        Task<IdentityUserDto> GetMyDetails(string id);
        Task<IdentityUserDto> UpdateRole(UpdateRoleModel role);
    }
}
