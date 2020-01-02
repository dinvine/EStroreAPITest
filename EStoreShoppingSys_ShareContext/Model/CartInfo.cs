using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStoreShoppingSys.Model
{
    class CartInfo
    {
        public int CartId { get; set; }
        public string AccountNumber { get; set; }
        public double AmountDue { get; set; }
        public List<ProductInfo> Items { get; set; }
    }
    class ProductInfo
    {
        public int ItemId { get; set; }
        public string ItemClass { get; set; }
        public string ItemName { get; set; }
        public string ItemPrice { get; set; }
        public string ItemDescription { get; set; }
    }
}
