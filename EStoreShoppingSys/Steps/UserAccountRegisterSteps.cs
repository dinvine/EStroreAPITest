using System;
using TechTalk.SpecFlow;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using NUnit.Framework;
using EStoreShoppingSys.Model;
using System.Configuration;

namespace EStoreShoppingSys.Steps
{
    [Binding]
    public class UserAccountRegisterSteps
    {
        UserAccount UserAccount { get; set; }
        RestClient RestClient { get; set; }
        RestRequest RestRequest { get; set; }
        IRestResponse RestResponse { get; set; }
        string EndPointStr { get; set; }
        UserAccountRegisterSteps()
        {
            UserAccount = null;
            RestClient = null;
             RestRequest = null;
             RestResponse = null;
             EndPointStr = "";
        }
        [Given(@"generate the ""(.*)"" username and ""(.*)"" password")]
        public void GivenGenerateTheUsernameAndPassword(string p0, string p1)
        {
            switch (p0)
            {
                case "random":
                    UserAccount.Username = FunctionsShared.GetRandomString(8);
                    break;
                case "empty":
                    UserAccount.Username = "";
                    break;
                case "existing":
                    {
                        GivenGenerateTheUsernameAndPassword("random", "random");
                        WhenVisitTheRegisterAPIWithTheUsernameAndPassword(EndPointStr);
                        break;
                    }
                default:
                        UserAccount.Username = p0;
                        break;

            }
            switch (p1)
            {
                case "random":
                    UserAccount.Password = FunctionsShared.GetRandomString(8);
                    break;
                case "empty":
                    UserAccount.Password = "";
                    break;
                default:
                        UserAccount.Username = p0;
                        break;
            }
        }


        [When(@"visit the register API ""(.*)"" with the username and password")]
        public void WhenVisitTheRegisterAPIWithTheUsernameAndPassword(string endPoint)
        {
            this.EndPointStr = endPoint;
            RestClient = new RestClient(ConfigurationManager.AppSettings["EStoreBaseURL"]);
            RestRequest = new RestRequest(EndPointStr, Method.POST);
            // Content type is not required when adding parameters this way
            // This will also automatically UrlEncode the values
            // restRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            //restRequest.AddParameter("username", usernameStr, "application/x-www-form-urlencoded", ParameterType.RequestBody);
            RestRequest.AddParameter("username", UserAccount.Username, ParameterType.GetOrPost);
            RestRequest.AddParameter("password", UserAccount.Password, ParameterType.GetOrPost);
            RestRequest.AddParameter("browserid", UserAccount.Password, ParameterType.GetOrPost);
            RestResponse = RestClient.Execute(RestRequest);
        }



        [Then(@"should get  response  status of (.*)")]
        public void ThenShouldGetResponseStatusOf(string p0)
        {
            Dictionary<string, string> headerList = FunctionsShared.GetResponseHeaderDict(RestResponse);
            Console.WriteLine(p0);
            Assert.AreEqual("application/json; charset", headerList["Content-Type"], "Test fail due to the Conten-Type in header is not application json");

            Assert.AreEqual(p0, RestResponse.StatusCode.ToString(), "Test fail due to Response StatusCode is not equal to OK");


        }

        [Then(@"get response body with ""(.*)""   equal to ""(.*)""")]
        public void ThenGetResponseBodyWithEqualTo(string p0, string p1)
        {
            var jObject = JObject.Parse(RestResponse.Content);
            string realValueStr = (string)jObject[p0];
            string expectedValueStr = p1;
            Assert.AreEqual(expectedValueStr, realValueStr, "Test fail due to return value of"+ p0 + " in response body is not equal to "+ expectedValueStr);

        }

        [Then(@"with new account number in datas")]
        public void ThenWithNewAccountNumberInDatas()
        {
            var jObject = JObject.Parse(RestResponse.Content);
            string accountNumberStr = (string)jObject["datas"]["accountNumber"];       
            Assert.GreaterOrEqual(8, accountNumberStr.Length, "Test fail due to accountNumber returned is shorter than 8");
            UserAccount.AccountNumber = accountNumberStr;
        }
        


        [Then(@"with item named ""(.*)""  contains ""(.*)""")]
        public void ThenWithItemNamedContains(string p0, string p1)
        {
            var jObject = JObject.Parse(RestResponse.Content);
            string realValueStr = (string)jObject[p0];
            Assert.IsTrue(realValueStr.ToLower().Contains(p1), "Test fail due to item named '" + p0 + "' returned does not contain value of '" + p1 + "'");
        }



    }
}
