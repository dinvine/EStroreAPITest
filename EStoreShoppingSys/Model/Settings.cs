using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace EStoreShoppingSys.Model
{
    public class Settings
    {
        public RestClient MyRestClient { get; set; }
        public RestRequest MyRestRequest { get; set; }
        public IRestResponse MyRestResponse { get; set; }

    }
}
