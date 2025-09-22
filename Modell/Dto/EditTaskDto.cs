using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Model.Dto
{
    public class EditTaskDto
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AssignedToId { get; set; }
        public DateTime DueDate { get; set; } = DateTime.UtcNow;
    }
}
