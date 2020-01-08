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

        public string accountNumber { get; set; }
        public double amountDue { get; set; }
        public List<Item> items { get; set; }
    }

    public class Item
    {
        public string itemId { get; set; }
        public string itemName { get; set; }
        public int quantity { get; set; }
        public string price { get; set; }
    }

}
