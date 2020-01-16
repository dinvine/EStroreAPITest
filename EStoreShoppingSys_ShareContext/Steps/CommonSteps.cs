using System;
using System.Net;
using TechTalk.SpecFlow;
using Newtonsoft.Json;
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
    public  class CommonSteps
    {
        readonly  ScenarioContext context;
        private  Settings _settings ;
        public CommonSteps(ScenarioContext injectedContext, Settings p_settings)
        {
            context = injectedContext;
            _settings = p_settings;
            context["browserId"] = "honor";
        }

        /// <summary>
        /// asyc execute request and validate the response json string with schema model
        /// </summary>
        /// <param name="baseUrlEStoreBaseURL">URI</param>
        /// <param name="method">Get Post Delete</param>
        /// <param name="endPointURL"> EndPoint </param>
        /// <param name="requestParams">params for request</param>
        [Given(@"Request  in URLEncodedForm and validate the response")]
        public void RequestInURLEncodedFromAndValidateTheResponse(string baseUrlEStoreBaseURL, Method method, string endPointURL, RequestParams requestParams)
        {
            _settings.MyRestClient = new RestClient(ConfigurationManager.AppSettings[baseUrlEStoreBaseURL]);
            if (context.ContainsKey("accessToken"))
            {
                var authenticator = new JwtAuthenticator(context["accessToken"].ToString());
                _settings.MyRestClient.Authenticator = authenticator;
            }
            if (context.ContainsKey("cookie_token_1") && context.ContainsKey("cookie_token_2"))
            {
                CookieContainer cookiecon = new CookieContainer();
                string[] items = context["cookie_token_1"].ToString().Split('#');
                cookiecon.Add(new Cookie(items[0], items[1], items[2], items[3]));
                items = context["cookie_token_2"].ToString().Split('#');
                cookiecon.Add(new Cookie(items[0], items[1], items[2], items[3]));
                _settings.MyRestClient.CookieContainer = cookiecon;
            }

            //create cookiecon to save cookies in previous response
            _settings.MyRestRequest = new RestRequest(ConfigurationManager.AppSettings[endPointURL], method);
            foreach (KeyValuePair<string, string> kvPairs in requestParams.Headers)
            {
                _settings.MyRestRequest.AddHeader(kvPairs.Key, kvPairs.Value);
            }

            foreach (KeyValuePair<string, string> kvPairs in requestParams.Parameters)
            {
                _settings.MyRestRequest.AddParameter(kvPairs.Key, kvPairs.Value);
            }

            foreach (KeyValuePair<string, string> kvPairs in requestParams.QueryParameters)
            {
                _settings.MyRestRequest.AddQueryParameter(kvPairs.Key, kvPairs.Value);
            }

            _settings.MyRestResponse = _settings.MyRestClient.ExecuteAsyncRequest(_settings.MyRestRequest).GetAwaiter().GetResult();
            context["RestSettings"] = _settings;



        }


        [Then(@"should get  response  status of (.*)")]
        //should get  response status of OK
        public void ThenShouldGetResponseStatusOf(string p0)
        {
            Dictionary<string, string> headerList = FunctionsShared.GetResponseHeaderDict(_settings.MyRestResponse);
            //Console.WriteLine(p0);
            Assert.AreEqual("application/json; charset", headerList["Content-Type"], "Test fail due to the Conten-Type in header is not application json");
            Assert.AreEqual(p0, _settings.MyRestResponse.StatusCode.ToString(), "Test fail due to Response StatusCode is not equal to OK");
        }

        [Then(@"get response body with '(.*)'   equal to '(.*)'")]
        //get response body with 'code'   equal to '200'
        public Boolean ThenGetResponseBodyWithEqualTo(string p0, string p1)
        {
            var jObject = JObject.Parse(_settings.MyRestResponse.Content);
            string realValueStr = (string)jObject[p0];
            string expectedValueStr = p1;
            Assert.AreEqual(expectedValueStr, realValueStr, "Test fail due to return value of" + p0 + " in response body is not equal to " + expectedValueStr);
            if(realValueStr == expectedValueStr)
                return true;
            else 
                return false;
        }



        [Then(@"get response body with ['(.*)']['(.*)']   equal to '(.*)'")]
        //get response body with e.g. [datas][account_number]   equal to '1234567'
        public void ThenGetResponseJsonWith2LevelItemEqualTo(string datas, string item, string expectedValue)
        {
            var jObject = JObject.Parse(_settings.MyRestResponse.Content);
            string realValueStr = (string)jObject[datas][item];
            Assert.AreEqual(expectedValue, realValueStr, "Test fail due to return value of [" + datas + "][" + item + "] in response body is not equal to " + expectedValue);
        }

        [Then(@"add  item:  \['(.*)'] \['(.*)'] in response body to scenario context")]
        //add  item in response body e.g. [datas][account_number]   equal to '1234567'
        public void ThenAddItemInResponseBodyToScenarioContext(string datas, string items)
        {
            var jObject = JObject.Parse(_settings.MyRestResponse.Content);

            Assert.IsTrue(jObject.ContainsKey(datas), "Test fail due to [" + datas + "] not existing in the response");
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

        /// <summary>
        /// should get response comform with model named ResponseSchema
        /// </summary>
        /// <param name="ResponseSchema"> the name of model Schema</param>
        [Then(@"should get response comform with model '(.*)'")]
        public void ThenShouldGetResponseComformWithModel(string ResponseSchema)
        {
            Boolean jsonFormatIsValid = FunctionsShared.CompareJsonWithSchema(_settings.MyRestResponse.Content, ResponseSchema);
            Assert.IsTrue(jsonFormatIsValid, "Test failed due to response content did not conform the schema ：" + ResponseSchema);
        }

        [Then(@"ThenSetValuesInResponseBodyToContext")]
        // set  items 'accessToken,tokenType,expiresIn,accountNumber'  in  "datas" into context
        // if datas=""   then  set items into context
        public void ThenSetValuesInResponseBodyToContext(string datas, string items)
        {

            var jObject = JObject.Parse(_settings.MyRestResponse.Content);
            if (datas != "")
            {
                Assert.IsTrue(jObject.ContainsKey(datas), "Test fail due to " + datas + " does not exists in the   response body .");
                foreach (var item in items.Split(','))
                {
                    Assert.IsNotNull(jObject.SelectToken(datas + "." + item), "Test fail due to " + item + " does not existes in the " + datas + " returned .");
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

        /// <summary>
        /// verify whether the string p0 contains p1
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        [Then(@"with item named '(.*)'  containing substring '(.*)'")]
        public void ThenWithItemNamedContainingSubstring(string p0, string p1)
        {
            var jObject = JObject.Parse(_settings.MyRestResponse.Content);
            string realValueStr = (string)jObject[p0];
            Console.WriteLine(realValueStr);
            Assert.IsTrue(realValueStr.ToLower().Contains(p1), "Test fail due to item named '" + p0 + "' returned does not contain value of '" + p1 + "'");

        }

        /// <summary>
        /// verify  the value of string p0 keeps constant by compare the value of context[p0] and context[p0+ "~Existing"]
        /// </summary>
        /// <param name="p0"></param>
        [Then(@"should keep value constant for keys '(.*)'")]
        public void ThenShouldKeepValueConstantForKeys(string p0)
        {

            foreach (var item in p0.Split(','))
            {
                Assert.AreEqual(context[item], context[item + "~Existing"], "Test fail due to " + item + " in response body is not equal to the one get in registration");
            }

        }


        /// <summary>
        /// verify the response status , code , and message in response body
        /// </summary>
        /// <param name="p0">response pattern</param>
        [Then(@"should get  response of '(.*)'")]
        public void ThenRegisterShouldGetResponseOf(string p0)
        {
            if (p0 == "OK")
            {
                ThenShouldGetResponseStatusOf("OK");
                ThenGetResponseBodyWithEqualTo("code", "200");
                ThenWithItemNamedContainingSubstring("message", "success");
            }
            else
            {
                ThenShouldGetResponseStatusOf("OK");
                ThenGetResponseBodyWithEqualTo("code", "0");
                ThenGetResponseBodyWithEqualTo("error", "True");
                string msgSubString;
                switch(p0)
                {
                    case "RegisteredError":
                        msgSubString = "registered";
                        break;
                    case "UsernameError":
                        msgSubString = "username";
                        break;
                    case "PasswordError":
                        msgSubString = "password";
                        break;
                    case "CredentialError":
                        msgSubString = "没有登录";
                        break;
                    case "DuplicateCartError":
                        msgSubString = "repeated";
                        break;
                    case "CartIdError":
                        msgSubString = "cartid";
                        break;
                    case "AccountNumberError":
                        msgSubString = "account number";
                        break;
                    case "InvalidItemError":
                        msgSubString = "itemid";
                        break;
                    case "InvalidCartError":
                        msgSubString = "cartid";
                        break;
                    case "InvalidTransactionNumberError":
                        msgSubString = "unexpected";
                        break;
                    case "unexpected":
                        msgSubString = "unexpected";
                        break;
                        
                    default:
                        msgSubString = "undefined message code "+p0;
                        break;
                }
                ThenWithItemNamedContainingSubstring("message", msgSubString);
            }
           
        }

        [Given(@"at  '(.*)' generate the '(.*)' username and '(.*)' password ")]
        //e.g. generate the 'random' username and 'random' password at  'RegisterEndPoint'
        //e.g. generate the 'exsiting' username and 'exsiting' password at  'RegisterEndPoint'
        
        public void GivenGenerateTheUsernameAndPasswordAt(  string p_username, string p_password)
        {

            switch (p_username)
            {
                case "random":
                    context["username"] = FunctionsShared.GetRandomString(8);
                    break;
                case "empty":
                    context["username"] = "";
                    break;
                case "existing":
                    {
                        GivenGenerateTheUsernameAndPasswordAt( "random", "random");
                        WhenVisitTheRegisterAPIWithTheUsernameAndPassword();
                        break;
                    }
                default:
                    context["username"] = p_username;
                    break;

            }
            switch (p_password)
            {
                case "random":
                    if(p_username!="existing")
                    context["password"] = FunctionsShared.GetRandomString(8);
                    break;
                case "empty":
                    context["password"] = "";
                    break            ;
                case "existing":
                    break;
                default:
                    context["password"] = p_password;
                    break;
            }
        }

        //visit the register API e.g.'RegisterEndPoint' with the username and password
        public void WhenVisitTheRegisterAPIWithTheUsernameAndPassword()
        {
            string baseUrlEStoreBaseURL = "EStoreBaseURL";
            string endPointURL = "RegisterEndPoint";
            RequestParams requestParams = new RequestParams();
            requestParams.Parameters.Add("username", context["username"].ToString());
            requestParams.Parameters.Add("password", context["password"].ToString());
            //RequestInURLEncodedForm(baseUrlEStoreBaseURL, Method.POST, endPointURL, requestParams);

                RequestInURLEncodedFromAndValidateTheResponse(baseUrlEStoreBaseURL, Method.POST, endPointURL, requestParams);


            var jObject = JObject.Parse(_settings.MyRestResponse.Content);
            if (jObject["code"].ToString() == "200")
                ThenAddItemInResponseBodyToScenarioContext("datas", "accountNumber");
        }

        [When(@"get token with '(.*)' credential")]
        [Given(@"get token  with '(.*)' credential")]
        public void GivenGetTokenAtEndpoint(string isOrNotValid)
        {

            if (isOrNotValid=="valid")
                {
                string baseUrlEStoreBaseURL = "EStoreBaseURL";
                string endPointURL = "TokenEndPoint";
                RequestParams requestParams = new RequestParams();
                requestParams.Parameters.Add("username", context["username"].ToString());
                requestParams.Parameters.Add("password", context["password"].ToString());
                requestParams.Parameters.Add("browserid", context["browserId"].ToString());
                
                RequestInURLEncodedFromAndValidateTheResponse(baseUrlEStoreBaseURL, Method.POST, endPointURL, requestParams);

                // _settings.MyRestResponse.Cookies
                ThenShouldGetResponseStatusOf("OK");
                ThenGetResponseBodyWithEqualTo("code", "200");
                ThenWithItemNamedContainingSubstring("message", "success");
                ThenAddItemInResponseBodyToScenarioContext("datas", "accessToken,tokenType,expiresIn,accountNumber");
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
                string baseUrlEStoreBaseURL = "EStoreBaseURL";
                string endPointURL = "TokenEndPoint";
                RequestParams requestParams = new RequestParams();
                requestParams.Parameters.Add("username", context["username"].ToString());
                requestParams.Parameters.Add("password", context["password"].ToString());
                requestParams.Parameters.Add("browserid", context["browserId"].ToString());
                RequestInURLEncodedFromAndValidateTheResponse(baseUrlEStoreBaseURL, Method.POST, endPointURL, requestParams);

                ThenShouldGetResponseStatusOf("OK");
                ThenGetResponseBodyWithEqualTo("code", "0");
                ThenGetResponseBodyWithEqualTo("error", "True");
                ThenWithItemNamedContainingSubstring("message", "username");
            }

        }


        [Given(@"CART create with the  token")]
        [When(@"CART create with the  token")]
         public void GivenCreateCartAtCartCreateEndPoint()
        {
            string baseUrlEStoreBaseURL = "EStoreBaseURL";
            string endPointURL = "CartCreateEndPoint";
            RequestParams requestParams = new RequestParams();
            //headers
            requestParams.Headers.Add("browserId", context["browserId"].ToString());
            requestParams.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //parameters
            requestParams.Parameters.Add("accountNumber", context["accountNumber"].ToString());
            //QueryParameters
            //requestParams.QueryParameters.Add(-------------);
            //            RequestInURLEncodedForm(baseUrlEStoreBaseURL, Method.POST, endPointURL, requestParams);

                RequestInURLEncodedFromAndValidateTheResponse(baseUrlEStoreBaseURL, Method.POST, endPointURL, requestParams);

            var jObject = JObject.Parse(_settings.MyRestResponse.Content);
            if (jObject["code"].ToString() == "200")
            {
                ThenAddItemInResponseBodyToScenarioContext("datas", "cartId,amountDue,accountNumber");
                Assert.AreEqual(context["accountNumber"], context["accountNumber~Existing"], "Test fail due to accountNumber in response body is not equal to the one get in registration");
            }
        }

        [Given(@"Register And Login And Create Cart")]
        public void RegisterLoginAndCreateCart()
        {
            GivenGenerateTheUsernameAndPasswordAt("random", "random");
            Console.WriteLine(context["username"]);
            WhenVisitTheRegisterAPIWithTheUsernameAndPassword();
            Console.WriteLine(_settings.MyRestResponse.Content);
            GivenGetTokenAtEndpoint("valid");
            Console.WriteLine(_settings.MyRestResponse.Content);
            GivenCreateCartAtCartCreateEndPoint();
            Console.WriteLine(_settings.MyRestResponse.Content);

        }
        [Given(@"add the valid items table to cart")]
        public void GivenAddTheValidItemsTableToCart(Table table)
        {
            Double amountDue = 0;
            foreach(var row in table.Rows)
            {
                GivenAddOneRecordOfItemToCart(row[0].Trim(), row[1].Trim());
                amountDue += Double.Parse(row[3].Trim());
            }
            amountDue = Math.Round(amountDue, 2);
            context["cartAmountDue"] = amountDue.ToString();
        }

        [Given(@"add one record of  item to cart")]
        public void GivenAddOneRecordOfItemToCart(string itemId, string quantity)
        {
            string baseUrlEStoreBaseURL = "EStoreBaseURL";
            string endPointURL = "CartAddItemEndPoint";
            RequestParams requestParams = new RequestParams();
            //headers
            requestParams.Headers.Add("browserId", context["browserId"].ToString());
            requestParams.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //parameters
            requestParams.Parameters.Add("cartId", context["cartId"].ToString());
            requestParams.Parameters.Add("itemId", itemId.Trim());
            if (!Int32.TryParse(quantity, out int itemQty)) { itemQty = -1; }
            Assert.AreNotEqual(itemQty, -1, "test fail due to itemQty input is not an integer");
            requestParams.Parameters.Add("quantity", itemQty.ToString());
            //QueryParameters
            //requestParams.QueryParameters.Add(-------------);
            //RequestInURLEncodedForm(baseUrlEStoreBaseURL, Method.POST, endPointURL, requestParams);


                RequestInURLEncodedFromAndValidateTheResponse(baseUrlEStoreBaseURL, Method.POST, endPointURL, requestParams);


            var jObject = JObject.Parse(_settings.MyRestResponse.Content);
            if (jObject["code"].ToString() == "200")
            {
                ThenAddItemInResponseBodyToScenarioContext("datas", "cartId,amountDue,accountNumber");
                Assert.AreEqual(context["accountNumber"], context["accountNumber~Existing"], "Test fail due to accountNumber in response body is not equal to the one get in registration");
                Assert.AreEqual(context["cartId"], context["cartId~Existing"], "Test fail due to cartId in response body is not equal to the one get in registration");
            }
        }


        [Given(@"delete one record of  item from cart")]
        public void GivenDeleteOneRecordOfItemFromCart(string itemId)
        {
            string baseUrlEStoreBaseURL = "EStoreBaseURL";
            string endPointURL = "CartDeleteItemEndPoint";
            RequestParams requestParams = new RequestParams();
            //headers
            requestParams.Headers.Add("browserId", context["browserId"].ToString());
            requestParams.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //parameters
            requestParams.Parameters.Add("cartId", context["cartId"].ToString());
            requestParams.Parameters.Add("itemId", itemId.Trim());
            //QueryParameters
            //requestParams.QueryParameters.Add(-------------);

            //RequestInURLEncodedForm(baseUrlEStoreBaseURL, Method.DELETE, endPointURL, requestParams);

                RequestInURLEncodedFromAndValidateTheResponse(baseUrlEStoreBaseURL, Method.DELETE, endPointURL, requestParams);
 

            var jObject = JObject.Parse(_settings.MyRestResponse.Content);
            if (jObject["code"].ToString() == "200")
            {
                ThenAddItemInResponseBodyToScenarioContext("datas", "cartId,amountDue,accountNumber");
                Assert.AreEqual(context["accountNumber"], context["accountNumber~Existing"], "Test fail due to accountNumber in response body is not equal to the one get in registration");
                Assert.AreEqual(context["cartId"], context["cartId~Existing"], "Test fail due to cartId in response body is not equal to the one get in registration");
            }

        }

        [When(@"CART delete  with the  token")]
        [Given(@"CART delete  with the  token")]


        public void GivenDeleteCart()
        {
            if (!context.ContainsKey("cartId")) return;

            string baseUrlEStoreBaseURL = "EStoreBaseURL";
            string endPointURL = "CartDeleteEndPoint";
            RequestParams requestParams = new RequestParams();
            //headers
            requestParams.Headers.Add("browserId", context["browserId"].ToString());
            requestParams.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //parameters
            requestParams.Parameters.Add("cartId", context["cartId"].ToString());
            //QueryParameters
            //requestParams.QueryParameters.Add(-------------);
            // RequestInURLEncodedForm(baseUrlEStoreBaseURL, Method.DELETE, endPointURL, requestParams);
                RequestInURLEncodedFromAndValidateTheResponse(baseUrlEStoreBaseURL, Method.DELETE, endPointURL, requestParams);


            //           var jObject = JObject.Parse(_settings.MyRestResponse.Content);
            //           if (jObject["code"].ToString() == "200"&& jObject["datas"]["cartId"].ToString()== context["cartId"].ToString())
            //               context.Remove("cartId");
            //               ThenGetResponseBodyWithIncluding("datas", "cartId,accountNumber");
            //            Assert.AreEqual(context["accountNumber"], context["accountNumber~Existing"], "Test fail due to accountNumber in response body is not equal to the one get in registration");
            //            Assert.AreEqual(context["cartId"], context["cartId~Existing"], "Test fail due to cartId in response body is not equal to the one get in registration");
        }

        [When(@"delete  account")]
        [Given(@"delete  account")]


        public void GivenDeleteAccount()
        {
            if (!context.ContainsKey("accountNumber")) return;
            //create cookiecon to save cookies in previous response
            if (!context.ContainsKey("accessToken"))
            {
                GivenGetTokenAtEndpoint("valid");
            }
            string baseUrlEStoreBaseURL = "EStoreBaseURL";
            string endPointURL = "AccountDeleteEndPoint";
            RequestParams requestParams = new RequestParams();
            //headers
            requestParams.Headers.Add("browserId", context["browserId"].ToString());
            requestParams.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //parameters
            //requestParams.Parameters.Add(-------------);
            //QueryParameters
            requestParams.QueryParameters.Add("account_number", context["accountNumber"].ToString());
            //RequestInURLEncodedForm(baseUrlEStoreBaseURL, Method.DELETE, endPointURL, requestParams);
            
                RequestInURLEncodedFromAndValidateTheResponse(baseUrlEStoreBaseURL, Method.DELETE, endPointURL, requestParams);
                
            var jObject = JObject.Parse(_settings.MyRestResponse.Content);
            if (jObject["code"].ToString() == "200")
            {
                context["accountNumber_deleted"] = context["accountNumber"];
                context.Remove("accountNumber");
            }
        }


        [When(@"get product info list")]

        public void GetProductInfoList()
        {
            string baseUrlEStoreBaseURL = "EStoreBaseURL";
            string endPointURL = "ProductInfoEndPoint";
            RequestParams requestParams = new RequestParams();
            //headers
            requestParams.Headers.Add("browserId", context["browserId"].ToString());
            requestParams.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //RequestInURLEncodedForm(baseUrlEStoreBaseURL, Method.GET, endPointURL, requestParams);

                RequestInURLEncodedFromAndValidateTheResponse(baseUrlEStoreBaseURL, Method.GET, endPointURL, requestParams);

        }

        [When(@"get transaction number")]
        [Given(@"get transaction number")]

        public void GetTransactionNumber()
        {
            string baseUrlEStoreBaseURL = "EStoreBaseURL";
            string endPointURL = "TransactionNumberEndPoint";
            RequestParams requestParams = new RequestParams();
            //headers
            requestParams.Headers.Add("browserId", context["browserId"].ToString());
            requestParams.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //RequestInURLEncodedForm(baseUrlEStoreBaseURL, Method.GET, endPointURL, requestParams);
                RequestInURLEncodedFromAndValidateTheResponse(baseUrlEStoreBaseURL, Method.GET, endPointURL, requestParams);

            var jObject = JObject.Parse(_settings.MyRestResponse.Content);
            if (jObject["code"].ToString() == "200")
            {
                ThenAddItemInResponseBodyToScenarioContext("datas", "transactionNumber");
                if (context.ContainsKey("transactionNumber~Existing"))
                {
                    context["transactionNumber"] = context["transactionNumber~Existing"];
                    context.Remove("transactionNumber~Existing");
                }

            }
            


        }

        [When(@"save the items table to transaction")]
        public void GivenSaveTheItemsTableToTransaction(Table table)
        {
            Double amountDue = 0;

            string itemsString = "[";
            foreach (TableRow itemRow in table.Rows)
            {
                itemsString += "{";
                itemsString += "\"" + (table.Header as string[])[0] + "\":\"" + itemRow[0] + "\",";
                itemsString += "\"" + (table.Header as string[])[1] + "\":\"" + itemRow[1] + "\",";
                itemsString += "\"" + (table.Header as string[])[2] + "\":\"" + itemRow[2] + "\",";
                itemsString += "\"" + (table.Header as string[])[3] + "\":\"" + itemRow[3] + "\"},";
                amountDue += Double.Parse(itemRow[4].Trim());
            }
            itemsString = itemsString.Substring(0, itemsString.Length - 1)+"]";
            amountDue = Math.Round(amountDue, 2);
            context["transactionAmountDue"] = amountDue.ToString();


            string baseUrlEStoreBaseURL = "EStoreBaseURL";
            string endPointURL = "TransactionSaveEndPoint";
            RequestParams requestParams = new RequestParams();
            //headers
            requestParams.Headers.Add("browserId", context["browserId"].ToString());
            requestParams.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //parameters
            requestParams.Parameters.Add("transactionNumber", context["transactionNumber"].ToString());
            requestParams.Parameters.Add("accountNumber", context["accountNumber"].ToString());
            requestParams.Parameters.Add("totalAmountDue", context["transactionAmountDue"].ToString());
             requestParams.Parameters.Add("items", itemsString);
            //RequestInURLEncodedForm(baseUrlEStoreBaseURL, Method.POST, endPointURL, requestParams);
                RequestInURLEncodedFromAndValidateTheResponse(baseUrlEStoreBaseURL, Method.POST, endPointURL, requestParams);



        }

    }
}
