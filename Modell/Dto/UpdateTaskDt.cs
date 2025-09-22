using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Model.Dto
{
    public  class UpdateTaskDt
    {
        public int TaskId { get; set; }
        public TStatus status { get; set; }
    }
}
