using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entity
{
    public class TwoFaToken
    {
   
            public int Id { get; set; }
            public string UserId { get; set; }
            public string Token { get; set; }
            public DateTime ExpiryDate { get; set; }
        }
    }
