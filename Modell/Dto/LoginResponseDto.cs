using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto
{
    public class LoginResponseDto
    {
        public string userId {  get; set; } 
        public string? Email { get; set; }
        public string? Token { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
        public bool? TwoFactorRequired { get; set; } = false;
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
