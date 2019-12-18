using System;
using TechTalk.SpecFlow;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using NUnit.Framework;
using EStoreShoppingSys.Model;
using System.Configuration;

namespace EStoreShoppingSys
{


    [Binding]
    public class UserLoginSteps  
    {
        UserLogin UserLogin { get; set; }
        RestClient RestClient { get; set; }
        RestRequest RestRequest { get; set; }
        IRestResponse RestResponse { get; set; }
        string EndPointStr { get; set; }
        UserLoginSteps()
        {
            UserLogin = new UserLogin { BrowserId="honor"};
            RestClient = null;
            RestRequest = null;
            RestResponse = null;
            EndPointStr = "";
        }
        [Given(@"UserLogin generate the ""(.*)"" username and ""(.*)"" password")]
        public void GivenUserLoginGenerateTheUsernameAndPassword(string p0, string p1)
        {

            switch (p0)
            {
                case "random":
                    UserLogin.Username = FunctionsShared.GetRandomString(8);
                    break;
                case "empty":
                    UserLogin.Username = "";
                    break;
                case "existing":                    
                        GivenUserLoginGenerateTheUsernameAndPassword("random", "random");
                        WhenUserLoginVisitTheRegisterAPIWithTheUsernameAndPassword("/authentication/register");
                        break;
                    
                default: 
                        UserLogin.Username = p0;
                        break;
                    

            }
            switch (p1)
            {
                case "random":
                    UserLogin.Password = FunctionsShared.GetRandomString(8);
                    break;
                case "empty":
                    UserLogin.Password = "";
                    break;
                default:                    
                        UserLogin.Password = p1;
                        break;
                    
            }
        }

        [When(@"UserLogin visit the register API ""(.*)"" with the username and password")]
        public void WhenUserLoginVisitTheRegisterAPIWithTheUsernameAndPassword(string p0)
        {
            this.EndPointStr = p0;
            RestClient = new RestClient(ConfigurationManager.AppSettings["EStoreBaseURL"]);
            RestRequest = new RestRequest(p0, Method.POST);
            RestRequest.AddParameter("username", UserLogin.Username, ParameterType.GetOrPost);
            RestRequest.AddParameter("password", UserLogin.Password, ParameterType.GetOrPost);
            RestResponse = RestClient.Execute(RestRequest);

            var jObject = JObject.Parse(RestResponse.Content);
            string accountNumberStr = (string)jObject["datas"]["accountNumber"];
            UserLogin.AccountNumber = accountNumberStr;

        }

        [When(@"visit the token API ""(.*)"" with the  username and password and browserid")]
        public void WhenVisitTheTokenAPIWithTheUsernameAndPasswordAndBrowserid(string p0)
        {
            this.EndPointStr = p0;
            RestClient = new RestClient(ConfigurationManager.AppSettings["EStoreBaseURL"]);
            RestRequest = new RestRequest(p0, Method.POST);
            RestRequest.AddParameter("username", UserLogin.Username, ParameterType.GetOrPost);
            RestRequest.AddParameter("password", UserLogin.Password, ParameterType.GetOrPost);
            RestRequest.AddParameter("browserid", UserLogin.BrowserId, ParameterType.GetOrPost);
            RestResponse = RestClient.Execute(RestRequest);
        }
        
        [Then(@"TokenAPI should return response  status of ""(.*)""")]
        public void ThenTokenAPIShouldReturnResponseStatusOfOK(string statusStr)
        {
            Dictionary<string, string> headerList = FunctionsShared.GetResponseHeaderDict(RestResponse);
            Console.WriteLine(statusStr);
            Assert.AreEqual("application/json; charset", headerList["Content-Type"], "Test fail due to the Conten-Type in header is not application json");
            Assert.AreEqual(statusStr, RestResponse.StatusCode.ToString(), "Test fail due to Response StatusCode is not equal to OK");

        }


        [Then(@"TokenAPI should return json with ""(.*)"" equal to ""(.*)""")]
        public void ThenTokenAPIShouldReturnJsonWithEqualTo(string p0, string p1)
        {

            var jObject = JObject.Parse(RestResponse.Content);
            string realValueStr = (string)jObject[p0];
            string expectedValueStr = p1.ToString();
            Assert.AreEqual(expectedValueStr, realValueStr, "Test fail due to return value of" + p0 + " in response body is not equal to " + expectedValueStr);

        }


        [Then(@"TokenAPI should return json with ""(.*)"" containing items '(.*)'")]
        public void ThenTokenAPIShouldReturnJsonWithP0ContainingItemsP1(string p0, string p1)
        {           
            var jObject = JObject.Parse(RestResponse.Content);
            foreach (var item in p1.Split(','))
            {                
                string realValueStr = (string)jObject[p0][item];
                Assert.GreaterOrEqual( realValueStr.Length, 1,"Test fail due to "+ p0 + "."+ item + " returned is shorter than 1");
                if (item == "accessToken") UserLogin.AccessToken = realValueStr;
                if (item == "tokenType") UserLogin.TokenType = realValueStr;
                if (item == "expiresIn") UserLogin.Expiration = realValueStr;
                if (item == "accountNumber")
                {
                    Assert.AreEqual(UserLogin.AccountNumber, realValueStr, "Test fail due to accountNumber in response body is not equal to the one get in registration");
                }
            } 
        }

        [Then(@"And TokenAPI should return json with ""(.*)"" containing substring ""(.*)""")]
        public void ThenAndTokenAPIShouldReturnJsonWithContainingSubstring(string p0, string p1)
        {
            var jObject = JObject.Parse(RestResponse.Content);
            string realValueStr = (string)jObject[p0];
            Assert.IsTrue(realValueStr.ToLower().Contains(p1), "Test fail due to item named '" + p0 + "' returned does not contain value of '" + p1 + "'");


        }
    }
}
