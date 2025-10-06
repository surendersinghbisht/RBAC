using Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contract
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto registerdt);
        Task<LoginResponseDto> LoginAsync(LoginDto loginDt);
        Task<bool> EnableTwoFactorAuthenticationAsync(string email, bool enable);
        //Task<LoginResponseDto> ValidateTwoFactorCode(TwoFactorDto dto);
        Task<bool> ForgetPassword(ForgetPassword forgetPassword);
    }
}
