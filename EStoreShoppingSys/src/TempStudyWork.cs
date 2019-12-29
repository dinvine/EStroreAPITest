using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using EStoreShoppingSys.Model;

namespace EStoreShoppingSys.src
{
    class TempStudyWork
    {

        static void Main(string[] args)
        {
            Console.WriteLine("hello");
            List<ProductInfo> itemsList = new List<ProductInfo>();
            itemsList.Add(new ProductInfo { ItemId=1,ItemName="2", ItemPrice ="5", ItemDescription ="ok" });
            itemsList.Add(new ProductInfo { ItemId = 2, ItemName = "ItemName2", ItemPrice = "15", ItemDescription = "ok2" });

            Console.WriteLine(itemsList.ToString());
        }
    }
}
