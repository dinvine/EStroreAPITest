using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStoreShoppingSys.Model
{
    public class CartDelete
    {
        public int code { get; set; }
        public Datas_CartDelete datas { get; set; }
        public string message { get; set; }
    }

    public class Datas_CartDelete
    {
        public int cartId { get; set; }

        public int accountNumber { get; set; }

    }
}
