using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStoreShoppingSys.Model
{
 

    public class Datas_TransactionNumber
    {
        public int transactionNumber { get; set; }
    }

    public class TransactionNumber
    {
        public int code { get; set; }
        public Datas_TransactionNumber datas { get; set; }
        public string message { get; set; }
    }

}
