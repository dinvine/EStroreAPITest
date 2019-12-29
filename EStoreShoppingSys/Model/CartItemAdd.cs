using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStoreShoppingSys.Model
{
    
    public class Datas_CartItemAdd
    {
        public int cartId { get; set; }
        public string accountNumber { get; set; }
        public double amountDue { get; set; }
    }

    public class CartItemAdd
    {
        public int code { get; set; }
        public Datas_CartItemAdd datas { get; set; }
        public string message { get; set; }
    }
}
