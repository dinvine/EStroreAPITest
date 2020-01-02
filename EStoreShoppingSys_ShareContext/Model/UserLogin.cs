using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStoreShoppingSys.Model
{
    class UserLogin:UserAccount
    {
        public string BrowserId{ get; set; }
        public string AccessToken { get; set; }
        public string LoginTime { get; set; }
        public string Expiration { get; set; }

        public string TokenType { get; set; }
    }
}
