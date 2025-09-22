using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Model.Dto
{
    public class AddTaskDto
    {
        public int? TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AssignedById { get; set; }
        public string AssignedToId { get; set; }
        public TStatus Status { get; set; }
        public string? AssignedTouserName { get; set; }
        public string? AssignedByuserName { get; set; }
        public DateTime createdOn { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; } = DateTime.UtcNow;
    }
}





