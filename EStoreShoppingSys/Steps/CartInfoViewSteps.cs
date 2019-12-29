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
    public class CartInfoViewSteps
    {
        readonly ScenarioContext _scenarioContext;
        readonly Settings _settings;
        readonly SharedSteps _sharedSteps;
        public Table addItemTable;

        CartInfoViewSteps(ScenarioContext scenarioContext, Settings p_settings)
        {

            _scenarioContext = scenarioContext;
            _settings = p_settings;
            _sharedSteps = new SharedSteps(scenarioContext, p_settings);
            _scenarioContext["browserId"] = "honor";

        }
        [Given(@"CartInfo Register And Login And CreateCart")]
        public void GivenCartInfoRegisterAndLoginAndCreateCart()
        {
            _sharedSteps.RegisterLoginAndCreateCart();
        }
        
        [When(@"CartInfo visit the cart info API with valid credential")]
        public void WhenCartInfoVisitTheCartInfoAPIWithValidCredential()
        {
            _sharedSteps.GetCartInfo();
        }
        
        [When(@"CartInfo visit the cart info API with invalid credential")]
        public void WhenCartInfoVisitTheCartInfoAPIWithInvalidCredential()
        {

            _scenarioContext["accessToken"] = "Invalid" + _scenarioContext["accessToken"];
            _sharedSteps.GetCartInfo();
            _scenarioContext["accessToken"] = _scenarioContext["accessToken"].ToString().Replace("Invalid", "");

        }
        
        [When(@"CartInfo visit the cart info API with invalid accountNumber")]
        public void WhenCartInfoVisitTheCartInfoAPIWithInvalidCartid()
        {
            _scenarioContext["accountNumber"] = "Invalid" + _scenarioContext["accountNumber"];
            _sharedSteps.GetCartInfo();
            _scenarioContext["accountNumber"] = _scenarioContext["accountNumber"].ToString().Replace("Invalid", "");
        }
        
        [Then(@"CartInfo should give  response of '(.*)'")]
        public void ThenCartInfoShouldGiveResponseOf(string p0)
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
            if (p0 == "accountNumberError")
            {
                _sharedSteps.ThenShouldGetResponseStatusOf("OK");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("code", "0");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("error", "True");
                _sharedSteps.ThenWithItemNamedContainingSubstring("message", "accountNumber");
            }
            if (p0 == "CartIdError")
            {
                _sharedSteps.ThenShouldGetResponseStatusOf("OK");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("code", "0");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("error", "True");
                _sharedSteps.ThenWithItemNamedContainingSubstring("message", "cartid");
            }
        }
    }
}
