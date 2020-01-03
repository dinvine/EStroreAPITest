using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStoreShoppingSys.Model
{


    public class Datas_UserLogin
    {
        public string accessToken { get; set; }
        public string tokenType { get; set; }
        public int expiresIn { get; set; }
        public string accountNumber { get; set; }
    }

    public class UserLogin
    {
        public int code { get; set; }
        public Datas_UserLogin datas { get; set; }
        public string message { get; set; }
    }
}
