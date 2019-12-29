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
    public class ProductInfoViewSteps
    {
        readonly ScenarioContext _scenarioContext;
        readonly Settings _settings;
        readonly SharedSteps _sharedSteps;
        public Table addItemTable;

        ProductInfoViewSteps(ScenarioContext scenarioContext, Settings p_settings)
        {

            _scenarioContext = scenarioContext;
            _settings = p_settings;
            _sharedSteps = new SharedSteps(scenarioContext, p_settings);
            _scenarioContext["browserId"] = "honor";

        }

        [Given(@"ProductInfo Register And Login And CreateCart")]
        public void GivenProductInfoRegisterAndLoginAndCreateCart()
        {
            _sharedSteps.RegisterLoginAndCreateCart();
        }
        
        [When(@"ProductInfo visit the cart info API with valid credential")]
        public void WhenProductInfoVisitTheCartInfoAPIWithValidCredential()
        {
            _sharedSteps.GetProductInfoList();
        }
        
        [When(@"ProductInfo visit the cart info API with invalid credential")]
        public void WhenProductInfoVisitTheCartInfoAPIWithInvalidCredential()
        {
            _scenarioContext["accessToken"] = "Invalid" + _scenarioContext["accessToken"];
            _sharedSteps.GetProductInfoList();
            _scenarioContext["accessToken"] = _scenarioContext["accessToken"].ToString().Replace("Invalid", "");
        }
        
        [Then(@"ProductInfo should give  response of '(.*)'")]
        public void ThenProductInfoShouldGiveResponseOf(string p0)
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
    }
}
