using Data.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entity
{
    public class UserNotification
    {
        public int Id { get; set; }

        public string UserId { get; set; }  
        public int AnnouncementId { get; set; }  

        public bool IsRead { get; set; } = false;      
        public DateTime? SeenAt { get; set; } = null; 

    
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public virtual Announcements Announcement { get; set; }
      
    }

}
