using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStoreShoppingSys.Model
{
    public class ResponseError
    {
        public int code { get; set; }
        public bool error { get; set; }
        public string message { get; set; }
    }
}
