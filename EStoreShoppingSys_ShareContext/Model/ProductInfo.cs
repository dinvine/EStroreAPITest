using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStoreShoppingSys.Model
{


    public class ProductInfo
    {
        public int id { get; set; }
        public string @class { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public string description { get; set; }
    }

    public class ProductList
    {
        public int code { get; set; }
        public List<ProductInfo> datas { get; set; }
        public string message { get; set; }
    }

}
