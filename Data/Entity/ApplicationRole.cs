using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entity
{
    public class ApplicationRole:IdentityRole
    {
        public bool? IsActive { get; set; }
        public string? ShortDescription { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
