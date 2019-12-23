using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecflowAPIAutomation.Model
{
    public class SalesData
    {
        public int code { get; set; }
        public List<salesDatas> datas { get; set; }
        public string message { get; set; }
    }

    public class salesDatas
    {
        public int id { get; set; }

        public string Class { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public string description { get; set; }

    }
}
