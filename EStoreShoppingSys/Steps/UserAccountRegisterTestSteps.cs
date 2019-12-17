using System;
using TechTalk.SpecFlow;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using NUnit.Framework;
using EStoreShoppingSys.Model;
using System.Configuration;

namespace EStoreShoppingSys.test.steps
{
    [Binding]
    public class UserAccountRegisterSteps
    {
        private static UserAccount userAccount;
        RestClient restClient = null;
        RestRequest restRequest = null;
        IRestResponse restResponse = null;


        [Given(@"visitor has the username and password prepared")]
        public void GivenVisitorHasTheUsernameAndPasswordPrepared()
        {
            userAccount.username = FunctionsShared.getRandomString(8);
            userAccount.password = FunctionsShared.getRandomString(8); 
        }
        


        [Given(@"visitor has not provide the password")]
        public void GivenVisitorHasNotProvideTheUsernameOrPassword()
        {
            
            userAccount.username = FunctionsShared.getRandomString(8); 
            userAccount.password = "";
        }
        
        
        
        [When(@"visit the register API with the existing username and password")]
        public void WhenVisitTheRegisterAPIWithTheExistingUsernameAndPassword()
        {

        }
        
        [When(@"visit the register API with the empty username or password")]
        public void WhenVisitTheRegisterAPIWithTheEmptyUsernameOrPassword()
        {

        }
        
        [Then(@"vistor should get the response of success and new account number\.")]
        public void ThenVistorShouldGetTheResponseOfSuccessAndNewAccountNumber_()
        {

            Dictionary<string, string> headerList = new Dictionary<string, string>();
            string[] keyPairs = null;
            foreach (var item in restResponse.Headers)
            {
                keyPairs = item.ToString().Split('=');
                headerList.Add(keyPairs[0], keyPairs[1]);
                Console.WriteLine(keyPairs[0] + ">>>>>>>>>" + keyPairs[1]);
            }

            Assert.AreEqual("application/json", headerList["Content-Type"], "Test fail due to the Conten-Type in header is not application json");

            Assert.AreEqual("200 OK", restResponse.StatusCode, "Test fail due to Response StatusCode is not equal to 200 OK");
            //Console.WriteLine(restResponse.StatusCode);

            var jObject = JObject.Parse(restResponse.Content);
            string returnCode = (string)jObject["code"];
            string accountNumberStr = (string)jObject["datas"]["accountNumber"];
            string messageStr= (string)jObject["message"];
            Assert.AreEqual("200", returnCode, "Test fail due to returen code in response body is not equal to 200");
            Assert.GreaterOrEqual(8, accountNumberStr.Length, "Test fail due to accountNumber returned is shorter than 8");
            Assert.IsTrue(messageStr.ToLower().Contains("success"), "Test fail due to message returned does not contain success");
            messageStr= (string)jObject["message"];

    }
        
        [Then(@"vistor should get the response of fail: erro=true;  code=(.*) \.")]
        public void ThenVistorShouldGetTheResponseOfFailErroTrueCode_(int p0)
        {
 
        }


    }
}
