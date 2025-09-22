using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto
{
    public class EditAnnouncementDto
    {
        public int AnnouncementId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
