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
    public class CreateAndDeleteCartSteps
    {
        readonly ScenarioContext _scenarioContext;
        readonly Settings _settings;
        readonly SharedSteps _sharedSteps;

        CreateAndDeleteCartSteps(ScenarioContext scenarioContext, Settings p_settings)
        {

            _scenarioContext = scenarioContext;
            _settings = p_settings;
            _sharedSteps = new SharedSteps(scenarioContext, p_settings);
            _scenarioContext["browserId"] = "honor";

        }



        [Given(@"CART get the valid token on '(.*)'")]
        public void GivenCartGetTheValidTokenOn(string endpoint)
        {
            _sharedSteps.GivenGenerateTheUsernameAndPasswordAt("RegisterEndPoint", "random", "random");
            _sharedSteps.WhenVisitTheRegisterAPIWithTheUsernameAndPassword("RegisterEndPoint");
            _sharedSteps.GivenGetTokenAtEndpoint("valid");
        }
        

        [Given(@"CART create with the  token")]
        [When(@"CART create with the  token")]
        public void WhenVisitTheCartAddAPIWithTheToken()
        {

            _sharedSteps.GivenCreateCartAtCartCreateEndPoint();
        }


        [When(@"CART create  with the  invalid token")]
        public void WhenCARTCreateAtWithTheInvalidToken()
        {
            _scenarioContext["accessToken"] += "####";
            _sharedSteps.GivenCreateCartAtCartCreateEndPoint();
            _scenarioContext["accessToken"] = _scenarioContext["accessToken"].ToString().Replace("####", "");
        }





        [When(@"CART delete  with the  token")]
        public void WhenVisitTheCartDeleteAPIWithTheToken()
        {
            _sharedSteps.GivenDeleteCart();
        }

        [When(@"CART delete  with the invalid token")]
        public void WhenCARTDeleteAtWithTheInvalidToken()
        {

            _scenarioContext["accessToken"] += "invalidToken";
            _sharedSteps.GivenDeleteCart();
            _scenarioContext["accessToken"] = _scenarioContext["accessToken"].ToString().Replace("invalidToken", "");
        }


        [Then(@"CART  should give  response of '(.*)'")]
        public void ThenCartShouldGiveResponseOf(string p0)
        {
            if (p0 == "OK")
            {
                _sharedSteps.ThenShouldGetResponseStatusOf("OK");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("code", "200");
                _sharedSteps.ThenWithItemNamedContainingSubstring("message", "success");
            }
            if (p0 == "TokenError")
            {
                _sharedSteps.ThenShouldGetResponseStatusOf("OK");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("code", "0");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("error", "True");
                _sharedSteps.ThenWithItemNamedContainingSubstring("message", "没有登录");
            }
            if (p0 == "DuplicateCartError")
            {
                _sharedSteps.ThenShouldGetResponseStatusOf("OK");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("code", "0");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("error", "True");
                _sharedSteps.ThenWithItemNamedContainingSubstring("message", "repeated");
            }         
        }

        [Then(@"CART should give json with '(.*)' containing items '(.*)'")]
        public void ThenCartShouldGiveJsonWithContainingItems(string datas, string items)
        {
            _sharedSteps.ThenGetResponseBodyWithIncluding(datas, items);
            Assert.AreEqual(_scenarioContext["accountNumber"], _scenarioContext["accountNumber~Existing"], "Test fail due to accountNumber in response body is not equal to the one get in registration");
            Assert.AreEqual(_scenarioContext["cartId"], _scenarioContext["cartId~Existing"], "Test fail due to cartId in response body is not equal to the one get in registration");
        }


    }
}
