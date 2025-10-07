using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto
{
    public class UserDetailsDto
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public DateOnly? Dob { get; set; }
        public IFormFile? ProfilePic { get; set; } = null;
        public string? ProfilePicUrl { get; set; }

        public string IdentityUserId { get; set; }

        

    }
}
