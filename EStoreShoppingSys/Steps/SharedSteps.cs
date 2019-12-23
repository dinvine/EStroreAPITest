using System;
using System.Net;
using TechTalk.SpecFlow;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using NUnit.Framework;
using EStoreShoppingSys.Model;
using System.Configuration;
using RestSharp.Authenticators;



namespace EStoreShoppingSys.Steps
{
    [Binding]
    public  class SharedSteps
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext context;
        private readonly Settings _settings ;
        readonly FunctionsShared funcs ;
        public SharedSteps(ScenarioContext injectedContext, Settings p_settings)
        {
            context = injectedContext;
            _settings = p_settings;
            funcs = new FunctionsShared();
        }

               

        [Given(@"at  '(.*)' generate the '(.*)' username and '(.*)' password ")]
        //generate the 'random' username and 'random' password at  'RegisterEndPoint'
        public void GivenGenerateTheUsernameAndPasswordAt(  string p_endpoint,string p_username, string p_password)
        {

            switch (p_username)
            {
                case "random":
                    context["username"] = funcs.GetRandomString(8);
                    break;
                case "empty":
                    context["username"] = "";
                    break;
                case "existing":
                    {
                        GivenGenerateTheUsernameAndPasswordAt("random", "random", p_endpoint);
                        WhenVisitTheRegisterAPIWithTheUsernameAndPassword(p_endpoint);
                        break;
                    }
                default:
                    context["username"] = p_username;
                    break;

            }
            switch (p_password)
            {
                case "random":
                    context["password"] = funcs.GetRandomString(8);
                    break;
                case "empty":
                    context["password"] = "";
                    break            ;
                default:
                    context["password"] = p_password;
                    break;
            }
        }

        [When(@"visit the register API '(.*)' with the username and password")]
        //visit the register API 'RegisterEndPoint' with the username and password
        public void WhenVisitTheRegisterAPIWithTheUsernameAndPassword(string endPoint)
        {
            context["browserId"] = "honor";
            _settings.MyRestClient = new RestClient(ConfigurationManager.AppSettings["EStoreBaseURL"]);
            _settings.MyRestRequest = new RestRequest(ConfigurationManager.AppSettings[endPoint], Method.POST);
            _settings.MyRestRequest.AddParameter("username", context["username"], ParameterType.GetOrPost);
            _settings.MyRestRequest.AddParameter("password", context["password"], ParameterType.GetOrPost);
            _settings.MyRestResponse = _settings.MyRestClient.Execute(_settings.MyRestRequest);
            //set context["accountNumber"]:
            ThenGetResponseBodyWithIncluding("datas", "accountNumber");
        }

        [Then(@"should get  response  status of (.*)")]
        //should get  response status of OK
        public void ThenShouldGetResponseStatusOf(string p0)
        {
            Dictionary<string, string> headerList = funcs.GetResponseHeaderDict(_settings.MyRestResponse);
            Console.WriteLine(p0);
            Assert.AreEqual("application/json; charset", headerList["Content-Type"], "Test fail due to the Conten-Type in header is not application json");

            Assert.AreEqual(p0, _settings.MyRestResponse.StatusCode.ToString(), "Test fail due to Response StatusCode is not equal to OK");


        }

        [Then(@"get response body with '(.*)'   equal to '(.*)'")]
        //get response body with 'code'   equal to '200'
        public void ThenGetResponseBodyWithEqualTo(string p0, string p1)
        {
            var jObject = JObject.Parse(_settings.MyRestResponse.Content);
            string realValueStr = (string)jObject[p0];
            string expectedValueStr = p1;
            Assert.AreEqual(expectedValueStr, realValueStr, "Test fail due to return value of" + p0 + " in response body is not equal to " + expectedValueStr);
        }


        [Then(@"get response body with \['(.*)'] including '(.*)'")]
        // with 'datas' containing items 'accessToken,tokenType,expiresIn,accountNumber'
        public void ThenGetResponseBodyWithIncluding(string datas, string items)
        {
            var jObject = JObject.Parse(_settings.MyRestResponse.Content);
            if (jObject.ContainsKey(datas))
            {
                foreach (var item in items.Split(','))
                {
                    string realValueStr = jObject[datas][item].ToString();
                    Assert.GreaterOrEqual(realValueStr.Length, 1, "Test fail due to " + datas + "." + item + " returned is shorter than 1");
                    if (!context.ContainsKey(item))
                        context[item] = realValueStr;
                    else
                        context[item + "~Existing"] = realValueStr;
                }
            }
        }

        [Then(@"ThenSetValuesInResponseBodyToContext")]
        // set  items 'accessToken,tokenType,expiresIn,accountNumber'  in  "datas" into context
        // if datas=""   then  set items into context
        public void ThenSetValuesInResponseBodyToContext(string datas, string items)
        {
            
            var jObject = JObject.Parse(_settings.MyRestResponse.Content);
            if (datas!="")
            {
                Assert.IsTrue(jObject.ContainsKey(datas), "Test fail due to " + datas + " does not exists in the   response body .");
                foreach (var item in items.Split(','))
                {
                    Assert.IsNotNull(jObject.SelectToken(datas+"."+item), "Test fail due to " + item + " does not existes in the " + datas + " returned .");
                    string realValueStr = (string)jObject[datas][item];
                        Assert.GreaterOrEqual(realValueStr.Length, 1, "Test fail due to " + datas + "." + item + " returned is shorter than 1");
                        if (!context.ContainsKey(item))
                            context[item] = realValueStr;
                        else
                            context[item + "~Existing"] = realValueStr;                    
                }
            }
            else
            {
                foreach (var item in items.Split(','))
                {
                    Assert.IsTrue(jObject.ContainsKey(item), "Test fail due to " + item + " does not existes in the " + datas + " returned .");

                    string realValueStr = (string)jObject[datas][item];
                        Assert.GreaterOrEqual(realValueStr.Length, 1, "Test fail due to " + datas + "." + item + " returned is shorter than 1");
                        if (!context.ContainsKey(item))
                            context[item] = realValueStr;
                        else
                            context[item + "~Existing"] = realValueStr;
                }
            }
        }

        [Then(@"with item named '(.*)'  containing substring '(.*)'")]
        public void ThenWithItemNamedContainingSubstring(string p0, string p1)
        {
            var jObject = JObject.Parse(_settings.MyRestResponse.Content);
            string realValueStr = (string)jObject[p0];
            Assert.IsTrue(realValueStr.ToLower().Contains(p1), "Test fail due to item named '" + p0 + "' returned does not contain value of '" + p1 + "'");

        }

        [When(@"visit the token API '(.*)' with the  username and password and browserid")]
        public void WhenVisitTheTokenAPIWithTheUsernameAndPasswordAndBrowserid(string p0)
        {

        }



        [Given(@"get token at endpoint '(.*)' with '(.*)' credential")]
        public void GivenGetTokenAtEndpoint(string endpoint,string isOrNotValid)
        {

            if (isOrNotValid=="valid")
                {
                _settings.MyRestClient = new RestClient(ConfigurationManager.AppSettings["EStoreBaseURL"]);
                _settings.MyRestRequest = new RestRequest(ConfigurationManager.AppSettings[endpoint], Method.POST);
                _settings.MyRestRequest.AddParameter("username", context["username"], ParameterType.GetOrPost);
                _settings.MyRestRequest.AddParameter("password", context["password"], ParameterType.GetOrPost);
                _settings.MyRestRequest.AddParameter("browserid", context["browserId"], ParameterType.GetOrPost);
                _settings.MyRestResponse = _settings.MyRestClient.Execute(_settings.MyRestRequest);
                
                // _settings.MyRestResponse.Cookies
                ThenShouldGetResponseStatusOf("OK");
                ThenGetResponseBodyWithEqualTo("code", "200");
                ThenWithItemNamedContainingSubstring("message", "success");
                ThenGetResponseBodyWithIncluding("datas", "accessToken,tokenType,expiresIn,accountNumber");
                Assert.AreEqual(context["accountNumber"], context["accountNumber~Existing"], "Test fail due to accountNumber in response body is not equal to the one get in registration");
                
                int cookiecount=1;
                foreach (var cookie in _settings.MyRestResponse.Cookies)
                {
                    if (cookie.Name.StartsWith("koa:sess"))
                    {
                        context["cookie_token_" + cookiecount.ToString()] = cookie.Name + "#" + cookie.Value + "#" + cookie.Path + "#" + cookie.Domain;
                        cookiecount += 1;
                    }
                }


            }
            if (isOrNotValid == "invalid")
            {
                _settings.MyRestClient = new RestClient(ConfigurationManager.AppSettings["EStoreBaseURL"]);
                _settings.MyRestRequest = new RestRequest(ConfigurationManager.AppSettings[endpoint], Method.POST);
                _settings.MyRestRequest.AddParameter("username", "Invalid_u.d#H", ParameterType.GetOrPost);
                _settings.MyRestRequest.AddParameter("password", context["password"], ParameterType.GetOrPost);
                _settings.MyRestRequest.AddParameter("browserid", context["browserId"], ParameterType.GetOrPost);
                _settings.MyRestResponse = _settings.MyRestClient.Execute(_settings.MyRestRequest);
                ThenShouldGetResponseStatusOf("OK");
                ThenGetResponseBodyWithEqualTo("code", "0");
                ThenGetResponseBodyWithEqualTo("error", "True");
                ThenWithItemNamedContainingSubstring("message", "username");
            }

        }


        [Given(@"create cart at CartCreateEndPoint")]
        public void GivenCreateCartAtCartCreateEndPoint()
        {
            //create cookiecon to save cookies in previous response




                _settings.MyRestClient = new RestClient(ConfigurationManager.AppSettings["EStoreBaseURL"]);
                var authenticator = new JwtAuthenticator(context["accessToken"].ToString());
                _settings.MyRestClient.Authenticator = authenticator;
                CookieContainer cookiecon = new CookieContainer();
                string[] items = context["cookie_token_1"].ToString().Split('#');
                cookiecon.Add(new Cookie(items[0], items[1], items[2], items[3]));
                items = context["cookie_token_2"].ToString().Split('#');
                cookiecon.Add(new Cookie(items[0], items[1], items[2], items[3]));
                _settings.MyRestClient.CookieContainer = cookiecon;

                _settings.MyRestRequest = new RestRequest(ConfigurationManager.AppSettings["CartCreateEndPoint"], Method.POST); 
                _settings.MyRestRequest.AddHeader("browserId", context["browserId"].ToString());
                //_settings.MyRestRequest.AddHeader("Authorization", "Bearer "+ context["accessToken"]);
                _settings.MyRestRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                _settings.MyRestRequest.AddParameter("accountNumber", context["accountNumber"]);

            //execute request
            _settings.MyRestResponse = _settings.MyRestClient.Execute(_settings.MyRestRequest);
                ThenGetResponseBodyWithIncluding("datas", "cartId,amountDue,accountNumber");
                Assert.AreEqual(context["accountNumber"], context["accountNumber~Existing"], "Test fail due to accountNumber in response body is not equal to the one get in registration");    
        }

        [Given(@"delete cart at CartCreateEndPoint")]
        public void GivenDeleteCart()
        {
            //create cookiecon to save cookies in previous response
            
            /*
            foreach (var cookie in _settings.MyRestClient.CookieContainer.)
            {
                cookiecon.Add(new Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain));
            }
            
            foreach (var cookie in _settings.MyRestResponse.Cookies)
                cookiecon.Add(new Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain));
            var authenticator = new JwtAuthenticator(context["accessToken"].ToString());
            */
            _settings.MyRestClient = new RestClient(ConfigurationManager.AppSettings["EStoreBaseURL"]);
            var authenticator = new JwtAuthenticator(context["accessToken"].ToString());
            _settings.MyRestClient.Authenticator = authenticator;
            CookieContainer cookiecon = new CookieContainer();
            string[] items = context["cookie_token_1"].ToString().Split('#');
            cookiecon.Add(new Cookie(items[0], items[1], items[2], items[3]));
            items = context["cookie_token_2"].ToString().Split('#');
            cookiecon.Add(new Cookie(items[0], items[1], items[2], items[3]));
            _settings.MyRestClient.CookieContainer = cookiecon;



            _settings.MyRestRequest = new RestRequest(ConfigurationManager.AppSettings["CartDeleteEndPoint"]+ "?cartId={cartId_url}", Method.DELETE);
            _settings.MyRestRequest.AddHeader("browserId", context["browserId"].ToString());
            _settings.MyRestRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            _settings.MyRestRequest.AddParameter("cartId_url", context["cartId"],ParameterType.UrlSegment);


            //execute request
            _settings.MyRestResponse = _settings.MyRestClient.Execute(_settings.MyRestRequest);

            ThenGetResponseBodyWithIncluding("datas", "cartId,accountNumber");
 
        }



    }
}
