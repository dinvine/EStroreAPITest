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
        static UserAccount userAccount = null;
        RestClient restClient = null;
        RestRequest restRequest = null;
        IRestResponse restResponse = null;
        UserAccountRegisterSteps()
        {
            userAccount = new UserAccount();
        }
        [Given(@"generate the ""(.*)"" username and ""(.*)"" password")]
        public void GivenGenerateTheUsernameAndPassword(string p0, string p1)
        {
            switch (p0)
            {
                case "random":
                    userAccount.username = FunctionsShared.getRandomString(8);
                    break;
                case "empty":
                    userAccount.username = "";
                    break;
                case "existing":
                    {
                        GivenGenerateTheUsernameAndPassword("random", "random");
                        WhenVisitTheRegisterAPIWithTheUsernameAndPassword();
                        break;
                    }

            }
            switch (p1)
            {
                case "random":
                    userAccount.password = FunctionsShared.getRandomString(8);
                    break;
                case "empty":
                    userAccount.password = "";
                    break;
            }
        }

        [When(@"visit the register API with the username and password")]
        public void WhenVisitTheRegisterAPIWithTheUsernameAndPassword()
        {
            restClient = new RestClient(ConfigurationManager.AppSettings["EStoreBaseURL"]);
            restRequest = new RestRequest("/authentication/register", Method.POST);
            // Content type is not required when adding parameters this way
            // This will also automatically UrlEncode the values
            // restRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            //restRequest.AddParameter("username", usernameStr, "application/x-www-form-urlencoded", ParameterType.RequestBody);
            restRequest.AddParameter("username", userAccount.username, ParameterType.GetOrPost);
            restRequest.AddParameter("password", userAccount.password, ParameterType.GetOrPost);
            restResponse = restClient.Execute(restRequest);
        }


        [Then(@"should get  response  status of (.*)")]
        public void ThenShouldGetResponseStatusOf(string p0)
        {
            Dictionary<string, string> headerList = FunctionsShared.getResponseHeaderDict(restResponse);
            Console.WriteLine(p0);
            Assert.AreEqual("application/json; charset", headerList["Content-Type"], "Test fail due to the Conten-Type in header is not application json");

            Assert.AreEqual(p0, restResponse.StatusCode.ToString(), "Test fail due to Response StatusCode is not equal to OK");


        }

        [Then(@"get response body with ""(.*)""   equal to ""(.*)""")]
        public void ThenGetResponseBodyWithEqualTo(string p0, string p1)
        {
            var jObject = JObject.Parse(restResponse.Content);
            string realValueStr = (string)jObject[p0];
            string expectedValueStr = p1;
            Assert.AreEqual(expectedValueStr, realValueStr, "Test fail due to return value of"+ p0 + " in response body is not equal to "+ expectedValueStr);

        }

        [Then(@"with new account number in datas")]
        public void ThenWithNewAccountNumberInDatas()
        {
            var jObject = JObject.Parse(restResponse.Content);
            string accountNumberStr = (string)jObject["datas"]["accountNumber"];       
            Assert.GreaterOrEqual(8, accountNumberStr.Length, "Test fail due to accountNumber returned is shorter than 8");


        }
        


        [Then(@"with item named ""(.*)""  contains ""(.*)""")]
        public void ThenWithItemNamedContains(string p0, string p1)
        {
            var jObject = JObject.Parse(restResponse.Content);
            string realValueStr = (string)jObject[p0];
            Assert.IsTrue(realValueStr.ToLower().Contains(p1), "Test fail due to item named '" + p0 + "' returned does not contain value of '" + p1 + "'");
        }



    }
}
