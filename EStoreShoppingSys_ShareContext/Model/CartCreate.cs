using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStoreShoppingSys.Model
{
    public class Datas_CartCreate
    {
        public int cartId { get; set; }
        public string accountNumber { get; set; }
        public int amountDue { get; set; }
    }

    public class CartCreate
    {
        public int code { get; set; }
        public Datas_CartCreate datas { get; set; }
        public string message { get; set; }
    }
}
