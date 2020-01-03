using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStoreShoppingSys.Model
{
    public class CartInfo
    {
        public int code { get; set; }
        public Datas_CartInfo datas { get; set; }
        public string message { get; set; }
    }

    public class Datas_CartInfo
    {
        public int cartId { get; set; }

        public int accountNumber { get; set; }
        public int amountDue { get; set; }

        public items itemDatas { get; set; }
    }

    public class items
    {
        public int itemId { get; set; }
        public string itemName { get; set; }

        public int quantity { get; set; }
        public int price { get; set; }
    }
}
