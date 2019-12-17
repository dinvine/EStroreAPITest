using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStoreShoppingSys.Model
{
    class TransactionInfo
    {
        public int transactionNumber { get; set; }
        public string accountNumber { get; set; }
        public double amountDue { get; set; }
        public List<ProductInfo> items { get; set; }
    }
}
