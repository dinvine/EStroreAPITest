using System;
using TechTalk.SpecFlow;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using NUnit.Framework;

namespace EStoreShoppingSys.test.steps
{
    [Binding]
    public class UserAccountRegisterSteps
    {
        RestClient restClient = null;
        RestRequest restRequest = null;
        IRestResponse restResponse = null;
        String usernameStr = "";
        String passwordStr = "";

        [Given(@"visitor has the username and password prepared")]
        public void GivenVisitorHasTheUsernameAndPasswordPrepared()
        {
            Random r = new Random();
            usernameStr = "";
            passwordStr = "";
            string  charNumStr = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";
            for(int i = 0; i < 8; i++)
            {
                usernameStr += charNumStr[r.Next(charNumStr.Length)];
                passwordStr += charNumStr[r.Next(charNumStr.Length)];
            }
            //  ScenarioContext.Current.Pending();
            
        }
        
        [Given(@"visitor has the existing username")]
        public void GivenVisitorHasTheExistingUsername()
        {
         //   ScenarioContext.Current.Pending();
        }
        
        [Given(@"visitor has not provide the password")]
        public void GivenVisitorHasNotProvideTheUsernameOrPassword()
        {
            //    ScenarioContext.Current.Pending();
            Random r = new Random();
            usernameStr = "";
            passwordStr = "";
            string charNumStr = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";
            for (int i = 0; i < 8; i++)
            {
                usernameStr += charNumStr[r.Next(charNumStr.Length)];                
            }

        }
        
        [When(@"visit the register API with the username and password")]
        public void WhenVisitTheRegisterAPIWithTheUsernameAndPassword()
        {
            // ScenarioContext.Current.Pending();
            restClient = new RestClient("http://106.15.238.71:7413/e-store");
            restRequest = new RestRequest("/authentication/register", Method.POST);
            // Content type is not required when adding parameters this way
            // This will also automatically UrlEncode the values
            // restRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            //restRequest.AddParameter("username", usernameStr, "application/x-www-form-urlencoded", ParameterType.RequestBody);
            
           // restRequest.AddParameter("password", passwordStr, "application/x-www-form-urlencoded", ParameterType.RequestBody);

            restRequest.AddParameter("username", usernameStr, ParameterType.GetOrPost);
            restRequest.AddParameter("password", passwordStr, ParameterType.GetOrPost);
            restResponse = restClient.Execute(restRequest);           
        }
        
        [When(@"visit the register API with the existing username and password")]
        public void WhenVisitTheRegisterAPIWithTheExistingUsernameAndPassword()
        {
          //  ScenarioContext.Current.Pending();
        }
        
        [When(@"visit the register API with the empty username or password")]
        public void WhenVisitTheRegisterAPIWithTheEmptyUsernameOrPassword()
        {
          //  ScenarioContext.Current.Pending();
        }
        
        [Then(@"vistor should get the response of success and new account number\.")]
        public void ThenVistorShouldGetTheResponseOfSuccessAndNewAccountNumber_()
        {
            //  ScenarioContext.Current.Pending();
            //Header
            Dictionary<string, string> headerList = new Dictionary<string, string>();
            string[] keyPairs = null;
            foreach (var item in restResponse.Headers)
            {
                keyPairs = item.ToString().Split('=');
                headerList.Add(keyPairs[0], keyPairs[1]);
                Console.WriteLine(keyPairs[0] + ">>>>>>>>>" + keyPairs[1]);
            }

            Assert.AreEqual("application/json", headerList["Content-Type"], "Test fail due to the Conten-Type in header is not application json");


            //response status 
            Assert.AreEqual("200 OK", restResponse.StatusCode, "Test fail due to Response StatusCode is not equal to 200 OK");
            //            Console.WriteLine(restResponse.StatusCode);

            var jObject = JObject.Parse(restResponse.Content);
            string returnCode = (string)jObject["code"];
            string accountNumberStr = (string)jObject["datas"]["accountNumber"];
            string messageStr= (string)jObject["message"];
            Assert.AreEqual("200", returnCode, "Test fail due to returen code in response body is not equal to 200");
            Assert.GreaterOrEqual(8, accountNumberStr.Length, "Test fail due to accountNumber returned is shorter than 8");
            Assert.IsTrue(messageStr.ToLower().Contains("success"), "Test fail due to message returned does not contain success");
            messageStr= (string)jObject["message"];

           /*
            String temperatureStr = jObject.GetValue("Temperature").ToString();
            string[] temperatureStrArray = temperatureStr.Split(' ');

            Console.WriteLine(float.Parse(temperatureStrArray[0]));
            Assert.IsTrue(Math.Abs(float.Parse(temperatureStrArray[0])) < 100, "Test fail  due to the temperature is not in the range of 100");
        */
    }
        
        [Then(@"vistor should get the response of fail: erro=true;  code=(.*) \.")]
        public void ThenVistorShouldGetTheResponseOfFailErroTrueCode_(int p0)
        {
          //  ScenarioContext.Current.Pending();
        }
    }
}
