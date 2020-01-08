using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStoreShoppingSys.Model
{

    public class Datas_UserAccount
    {
        public string accountNumber { get; set; }
    }

    public class UserAccount
    {
        public int code { get; set; }
        public Datas_UserAccount datas { get; set; }
        public string message { get; set; }
    }


}
