using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto
{
    public class RoleDt
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }

        public bool? IsActive { get; set; }
    }
}
