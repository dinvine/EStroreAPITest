using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStoreShoppingSys.Model
{
    class TransactionInfo
    {
        public int TransactionNumber { get; set; }
        public string AccountNumber { get; set; }
        public double AmountDue { get; set; }
        public List<ProductInfo> Items { get; set; }
    }
}
