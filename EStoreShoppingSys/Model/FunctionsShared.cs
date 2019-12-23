using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace EStoreShoppingSys.Model
{
    public  class FunctionsShared
    {
        public string GetRandomString(int length)
        {
            Random r = new Random();
            string rStr = "";            
            string charNumStr = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";
            for (int i = 0; i < length; i++)
            {
                rStr += charNumStr[r.Next(charNumStr.Length)];
            }
            return rStr;
        }

        public  Dictionary<string, string> GetResponseHeaderDict(IRestResponse restResponse) {

            Dictionary<string, string> headerList = new Dictionary<string, string>();
            string[] keyPairs;
            foreach (var item in restResponse.Headers)
            {
                keyPairs = item.ToString().Split('=');
                headerList.Add(keyPairs[0], keyPairs[1]);
                Console.WriteLine(keyPairs[0] + ">>>>>>>>>" + keyPairs[1]);
            }
            return headerList;
        }

    }
}
