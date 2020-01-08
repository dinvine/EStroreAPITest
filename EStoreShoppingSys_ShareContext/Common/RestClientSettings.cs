using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using TechTalk.SpecFlow;
namespace EStoreShoppingSys.Model
{
    public class Settings
    {
        public RestClient MyRestClient { get; set; }
        public RestRequest MyRestRequest { get; set; }
        public IRestResponse MyRestResponse { get; set; }

    }
    /*
    public static class StaticSettings{
        public static Settings settingStatic;
        public static ScenarioContext contextStaic;
    }
    */
    public class RequestParams
    {
        public Dictionary<string, string> Headers;
        public Dictionary<string, string> Parameters;
        public Dictionary<string, string> QueryParameters;
        public RequestParams()
        {
            Headers = new Dictionary<string, string>();
            Parameters = new Dictionary<string, string>();
            QueryParameters = new Dictionary<string, string>();
        }
    }
}
