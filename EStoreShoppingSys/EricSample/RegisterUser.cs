using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecflowAPIAutomation.Model
{
    public class RegisterUser
    {
        public int code { get; set; }
        public userDatas datas { get; set; }
        public string message { get; set; }
    }

    public class userDatas
    {
        public string accountNumber { get; set; }
    }
}
