using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto
{
    public class TwoFactorDto
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
}
