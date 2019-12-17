using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStoreShoppingSys.Model
{
    class UserLogin
    {
        public string token { get; set; }
        public string loginTime { get; set; }
        public string expiration { get; set; }
    }
}
