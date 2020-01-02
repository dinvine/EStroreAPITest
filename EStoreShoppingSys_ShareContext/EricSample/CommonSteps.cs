using RestSharp;
using SpecflowAPIAutomation.Base;
using SpecflowAPIAutomation.Utilities;
using SpecflowAPIAutomation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Dynamitey.DynamicObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;
using NUnit.Framework;
using NUnit.Framework.Internal;
using RestSharp.Authenticators;

namespace SpecflowAPIAutomation.Steps
{
    [Binding]
    public class CommonSteps
    {
        //Context injection
        private Settings _settings;
        public CommonSteps(Settings settings)
        {
            _settings = settings;
        }

        [Given(@"I get JWT authentication of user with following credentials")]
        public void GivenIGetJWTAuthenticationOfUserWithFollowingCredentials(Table table)
        {
            _settings.Request = new RestRequest("e-store/authentication/token", Method.POST);

            //_settings.Request.AddParameter("application/x-www-form-urlencoded", $"username=apitest010&password=apitest&browserId=abcdef", ParameterType.RequestBody);

            foreach (var row in table.Rows)
            {

                if (row[1].Equals("username"))
                {
                    _settings.Request.AddParameter(row[0], ScenarioContext.Current.Get<String>("username"));
                }
                else if (row[1].Equals("password"))
                {
                    _settings.Request.AddParameter(row[0], ScenarioContext.Current.Get<String>("password"));
                }
                else
                {
                    _settings.Request.AddParameter(row[0], row[1]);
                }
                
                if (row[0].Equals("browserId"))
                {
                    ScenarioContext.Current.Add(row[0], row[1]);
                }
            }

            //Get access token
            _settings.Response = _settings.RestClient.ExecuteAsyncRequest<Authentications>(_settings.Request).GetAwaiter().GetResult();
            string responseDatas = _settings.Response.DeserializeResponse()["datas"];

            Dictionary<string, string> datas = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseDatas);

            //Get account number
            var accountNumber = datas["accountNumber"];

            //Get authentication token
            var authenticator = new JwtAuthenticator(datas["accessToken"]);
            _settings.RestClient.Authenticator = authenticator;

            //Creating a cookie container to store all cookie for future requests
            CookieContainer cookiecon = new CookieContainer();
            //var cookie = _settings.Response.Cookies.FirstOrDefault();
            foreach (var cookie in _settings.Response.Cookies)
            {
                cookiecon.Add(new Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain)); //this adds every cookie in the previous response.
            }
            //cookiecon.Add(new Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain));
            _settings.RestClient.CookieContainer = cookiecon;
        }

        [Then(@"I should have the response with status code (.*)")]
        public void ThenIShouldHaveTheFollowingResponseWithStatusCode(string statusCode)
        {
            Assert.That(_settings.Response.StatusCode.ToString(), Is.EqualTo(statusCode), $"The statusCode is not matching");
        }

        [Given(@"I should save the (.*) from the response")]
        [Then(@"I should save the (.*) from the response")]
        public void ThenIShouldSaveFromTheResponse(string key)
        {
            var value = _settings.Response.GetNestedResponseObject("datas", key);
            ScenarioContext.Current.Add(key, value);
        }

        [Given(@"I perform a GET operation for (.*)")]
        public void GivenIPerformGETOperationFor(string url)
        {
            _settings.Request = new RestRequest(url, Method.GET);

            //_settings.Response = _settings.RestClient.ExecuteAsyncRequest<CreateCart>(_settings.Request).GetAwaiter().GetResult();
        }

        [Given(@"I perform GET operation for (.*) with invalid auth")]
        public void GivenIPerformGETOperationWithInvalidAuth(string returnObjType)
        {
            _settings.Request.AddHeader("browserId", ScenarioContext.Current.Get<string>("browserId"));

            var invalidAuth = new JwtAuthenticator("qerqerqwerdssfasdfadfasfdasdftrewthy"); ;
            _settings.RestClient.Authenticator = invalidAuth;

            switch (returnObjType)
            {
                case "SalesData":
                    _settings.Response = _settings.RestClient.ExecuteAsyncRequest<SalesData>(_settings.Request).GetAwaiter().GetResult();
                    break;
                case "CartInfo":
                    _settings.Response = _settings.RestClient.ExecuteAsyncRequest<CartInfo>(_settings.Request).GetAwaiter().GetResult();
                    break;
                case "TransactionNumber":
                    _settings.Response = _settings.RestClient.ExecuteAsyncRequest<TransactionNumber>(_settings.Request).GetAwaiter().GetResult();
                    break;
            }

        }

        [Given(@"I perform operation for GET (.*)")]
        public void GivenIPerformOperationForGETOperation(string returnObjType)
        {
            _settings.Request.AddHeader("browserId", ScenarioContext.Current.Get<string>("browserId"));
            switch (returnObjType)
            {
                case "SalesData":
                    _settings.Response = _settings.RestClient.ExecuteAsyncRequest<SalesData>(_settings.Request).GetAwaiter().GetResult();
                    break;
                case "CartInfo":
                    _settings.Response = _settings.RestClient.ExecuteAsyncRequest<CartInfo>(_settings.Request).GetAwaiter().GetResult();
                    break;
                case "TransactionNumber":
                    _settings.Response = _settings.RestClient.ExecuteAsyncRequest<TransactionNumber>(_settings.Request).GetAwaiter().GetResult();
                    break;
            }

        }

        [Given(@"I perform a (.*) POST operation for (.*) with body")]
        public void GivenIPerformAPOSTOperationWithBody(string returnObjType, string url, Table table)
        {
            _settings.Request = new RestRequest(url, Method.POST);

            foreach (var row in table.Rows)
            {
                if (string.Equals(row[1], "username") || string.Equals(row[1], "password"))
                {
                    var rand = new Random();
                    int num = rand.Next(100000);
                    _settings.Request.AddParameter(row[0], row[1] + num);
                    ScenarioContext.Current.Add(row[0], row[1] + num);
                }
                else if (string.Equals(row[1], "accountNumber"))
                {
                    var accountNumber = ScenarioContext.Current.Get<String>("accountNumber");
                    _settings.Request.AddHeader("browserId", ScenarioContext.Current.Get<string>("browserId"));
                    _settings.Request.AddParameter(row[0], accountNumber);
                }
                else
                {
                    _settings.Request.AddHeader("browserId", ScenarioContext.Current.Get<string>("browserId"));
                    _settings.Request.AddParameter(row[0], row[1]);
                }

                //_settings.Request.AddParameter("application/x-www-form-urlencoded", $"accountNumber=10010", ParameterType.RequestBody);
            }

            switch (returnObjType)
            {
                case "CreateCart":
                    _settings.Response = _settings.RestClient.ExecuteAsyncRequest<CreateCart>(_settings.Request).GetAwaiter().GetResult();
                    break;
                case "TransactionSave":
                    //string itemJSON = "";
                    //_settings.Request.AddParameter("application/json",  new {itemId = 1, itemName = "Apple Watch Series 4", quantity = 1, price = 550.0 }, ParameterType.RequestBody);
                    JObject jObjectbody = new JObject();
                    jObjectbody.Add("transactionNumber", 1);
                    _settings.Request.AddParameter("items",  new {itemId = 1, itemName = "Apple Watch Series 4", quantity = 1, price = 550.0 }, ParameterType.RequestBody);
                    _settings.Response = _settings.RestClient.ExecuteAsyncRequest<TransactionSave>(_settings.Request).GetAwaiter().GetResult();
                    break;
                case "RegisterUser":
                    _settings.Response = _settings.RestClient.ExecuteAsyncRequest<RegisterUser>(_settings.Request).GetAwaiter().GetResult();
                    break;
                    //case "AddItem":
                    //    _settings.Response = _settings.RestClient.ExecuteAsyncRequest<AddItem>(_settings.Request).GetAwaiter().GetResult();
                    //    break;
            }

        }

        [Given(@"I perform a (.*) POST operation for (.*) with invalid auth")]
        public void GivenIPerformAPOSTOperationWithinvalidauth(string returnObjType, string url, Table table)
        {
            _settings.Request = new RestRequest(url, Method.POST);

            var invalidAuth = new JwtAuthenticator("qerqerqwerdssfasdfadfasfdasdftrewthy"); ;
            _settings.RestClient.Authenticator = invalidAuth;

            foreach (var row in table.Rows)
            {
                if (string.Equals(row[1], "username") || string.Equals(row[1], "password"))
                {
                    var rand = new Random();
                    int num = rand.Next(100000);
                    _settings.Request.AddParameter(row[0], row[1] + num);
                    ScenarioContext.Current.Add(row[0], row[1] + num);
                }
                else if (string.Equals(row[1], "accountNumber"))
                {
                    var accountNumber = ScenarioContext.Current.Get<String>("accountNumber");
                    _settings.Request.AddHeader("browserId", ScenarioContext.Current.Get<string>("browserId"));
                    _settings.Request.AddParameter(row[0], accountNumber);
                }
                else
                {
                    _settings.Request.AddHeader("browserId", ScenarioContext.Current.Get<string>("browserId"));
                    _settings.Request.AddParameter(row[0], row[1]);
                }

                //_settings.Request.AddParameter("application/x-www-form-urlencoded", $"accountNumber=10010", ParameterType.RequestBody);
            }

            switch (returnObjType)
            {
                case "CreateCart":
                    _settings.Response = _settings.RestClient.ExecuteAsyncRequest<CreateCart>(_settings.Request).GetAwaiter().GetResult();
                    break;
                case "TransactionSave":
                    //string itemJSON = "";
                    //_settings.Request.AddParameter("application/json",  new {itemId = 1, itemName = "Apple Watch Series 4", quantity = 1, price = 550.0 }, ParameterType.RequestBody);
                    JObject jObjectbody = new JObject();
                    jObjectbody.Add("transactionNumber", 1);
                    _settings.Request.AddParameter("items", new { itemId = 1, itemName = "Apple Watch Series 4", quantity = 1, price = 550.0 }, ParameterType.RequestBody);
                    _settings.Response = _settings.RestClient.ExecuteAsyncRequest<TransactionSave>(_settings.Request).GetAwaiter().GetResult();
                    break;
                case "RegisterUser":
                    _settings.Response = _settings.RestClient.ExecuteAsyncRequest<RegisterUser>(_settings.Request).GetAwaiter().GetResult();
                    break;
                    //case "AddItem":
                    //    _settings.Response = _settings.RestClient.ExecuteAsyncRequest<AddItem>(_settings.Request).GetAwaiter().GetResult();
                    //    break;
            }

        }


        [Given(@"I perform a (.*) POST operation for (.*) with JSON body")]
        public void GivenIPerformAPOSTOperationWithJSONBody(string returnObjType, string url, Table table)
        {
            _settings.Request = new RestRequest(url, Method.POST);

            _settings.Request.AddHeader("browserId", ScenarioContext.Current.Get<string>("browserId"));

            JObject jObjectbody = new JObject();

            var value = ScenarioContext.Current.Get<string>("transactionNumber");
            jObjectbody.Add("transactionNumber", value);

            foreach (var row in table.Rows)
            {
                jObjectbody.Add(row[0], row[1]);
            }

            var items = new JArray() as dynamic;
            dynamic item = new JObject();
            item.Add("itemId", 1);
            item.Add("itemName", "Apple Watch Series 4");
            item.Add("quantity", 1);
            item.Add("price", 550);

            items.Add(item);

            //JObject itemsbody = new JObject();
            //itemsbody.Add("itemId", 1);
            //itemsbody.Add("itemName", "Apple Watch Series 4");
            //itemsbody.Add("quantity", 1);
            //itemsbody.Add("price", 550);

            //List<JObject> items = new List<JObject>();
            //items.Add(itemsbody);



            jObjectbody.Add("items", items);

            _settings.Request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            _settings.Response = _settings.RestClient.ExecuteAsyncRequest<TransactionSave>(_settings.Request).GetAwaiter().GetResult();


        }


        [Given(@"I perform a (.*) POST operation for (.*) with (.*) as the parameter and body")]
        public void GivenIPerformAPOSTOperationWithBody(string returnObjType, string url, string key, Table table)
        {
            _settings.Request = new RestRequest(url, Method.POST);

            _settings.Request.AddHeader("browserId", ScenarioContext.Current.Get<string>("browserId"));

            var value = ScenarioContext.Current.Get<string>(key);
            _settings.Request.AddParameter(key, value);

            foreach (var row in table.Rows)
            {
                _settings.Request.AddParameter(row[0], row[1]);
            }

            switch (returnObjType)
            {
                //case "CreateCart":
                //    _settings.Response = _settings.RestClient.ExecuteAsyncRequest<CreateCart>(_settings.Request).GetAwaiter().GetResult();
                //    break;
                case "AddItem":
                    _settings.Response = _settings.RestClient.ExecuteAsyncRequest<AddItem>(_settings.Request).GetAwaiter().GetResult();
                    break;
            }

        }

        [Given(@"I perform a (.*) DELETE operation for (.*)  with (.*) as the paramete and body")]
        public void GivenIPerformADELETEOperationWithParameteAndBody(string returnObjType, string url, string key, Table table)
        {
            _settings.Request = new RestRequest(url, Method.DELETE);

            _settings.Request.AddHeader("browserId", ScenarioContext.Current.Get<string>("browserId"));

            var value = ScenarioContext.Current.Get<string>(key);
            _settings.Request.AddParameter(key, value);

            foreach (var row in table.Rows)
            {
                _settings.Request.AddParameter(row[0], row[1]);
            }

            _settings.Response = _settings.RestClient.ExecuteAsyncRequest<AddItem>(_settings.Request).GetAwaiter().GetResult();

        }

        [Given(@"I perform a (.*) DELETE operation for (.*) with (.*) as the parameter")]
        public void GivenIPerformADELETEOperationWithParameter(string returnObjType, string url, string key)
        {
            _settings.Request = new RestRequest(url, Method.DELETE);
            
            var value = ScenarioContext.Current.Get<string>(key);
            _settings.Request.AddParameter(key, value);


            switch (returnObjType)
            {
                case "DeleteCart":
                    _settings.Request.AddHeader("browserId", ScenarioContext.Current.Get<string>("browserId"));
                    _settings.Response = _settings.RestClient.ExecuteAsyncRequest<DeleteCart>(_settings.Request).GetAwaiter().GetResult();
                    break;
                case "DeleteAccount":
                    _settings.Response = _settings.RestClient.ExecuteAsyncRequest<DeleteAccount>(_settings.Request).GetAwaiter().GetResult();
                    break;


            }
        }
    }
}
