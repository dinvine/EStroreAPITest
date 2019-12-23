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

        [Given(@"get the valid token on '(.*)'")]
        public void GivenGetTheValidTokenOn(string endpoint)
        {
            _sharedSteps.GivenGenerateTheUsernameAndPasswordAt("RegisterEndPoint", "random", "random");
            _sharedSteps.WhenVisitTheRegisterAPIWithTheUsernameAndPassword("RegisterEndPoint");
            _sharedSteps.GivenGetTokenAtEndpoint(endpoint, "valid");
        }
        
        [Given(@"get the invalid token on '(.*)'")]
        public void GivenGetTheInvalidTokenOn(string endpoint)
        {
            _sharedSteps.GivenGenerateTheUsernameAndPasswordAt("RegisterEndPoint", "random", "random");
            _sharedSteps.WhenVisitTheRegisterAPIWithTheUsernameAndPassword("RegisterEndPoint");
            _sharedSteps.GivenGetTokenAtEndpoint(endpoint, "valid");
            _scenarioContext["accessToken"] = "FakeToken_1.3#0;";
        }


        [Given(@"create cart at '(.*)' with the  token")]
        [When(@"create cart at '(.*)' with the  token")]
        public void WhenVisitTheCartAddAPIWithTheToken(string p0)
        {

            _sharedSteps.GivenCreateCartAtCartCreateEndPoint();
        }
        
        [When(@"delete cart at CartDeleteEndPoint with the  token")]
        public void WhenVisitTheCartDeleteAPIWithTheToken()
        {
            _sharedSteps.GivenDeleteCart();
        }
        
        [Then(@"Cart  should give  response of '(.*)'")]
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

        }

        [Then(@"CartAdd should give json with '(.*)' containing items '(.*)'")]
        public void ThenCartAddShouldGiveJsonWithContainingItems(string datas, string items)
        {
            _sharedSteps.ThenGetResponseBodyWithIncluding(datas, items);
            Assert.AreEqual(_scenarioContext["accountNumber"], _scenarioContext["accountNumber~Existing"], "Test fail due to accountNumber in response body is not equal to the one get in registration");

        }

        [Then(@"CartDelete should give json with '(.*)' containing items '(.*)'")]
        public void ThenCartDeleteShouldGiveJsonWithContainingItems(string datas, string items)
        {
            // ScenarioContext.Current.Pending();
            _sharedSteps.ThenGetResponseBodyWithIncluding(datas, items);

                Assert.AreEqual(_scenarioContext["accountNumber"], _scenarioContext["accountNumber~Existing"], "Test fail due to accountNumber in response body is not equal to original one");
            
  //              Assert.AreEqual(_scenarioContext["cartId"], _scenarioContext["cartId~Existing"], "Test fail due to cartId in response body is not equal to original one");

        }

        [When(@"delete cart at '(.*)' with the invalid token")]
        public void WhenDeleteCartAtWithTheInvalidToken(string p0)
        {
         //   ScenarioContext.Current.Pending();
        }

    }
}
