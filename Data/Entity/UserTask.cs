
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Data.Entity
{
    public class UserTask
    {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string AssignedById { get; set; }
        public virtual IdentityUser AssignedBy { get; set; }
        public string AssignedToId { get; set; }
        public virtual IdentityUser AssignedTo { get; set; }
        public TStatus Status { get; set; } = TStatus.Pending;
        public DateTime createdOn { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; } = DateTime.UtcNow;


    }
}

//public enum TStatus
//{
//    Done,
//    Pending,
//    Working
//}

