using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entity
{
    public class UserDetails
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public DateOnly? Dob { get; set; }
        public string? ProfilePicUrl { get; set; } = null;

        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }

    }
}
