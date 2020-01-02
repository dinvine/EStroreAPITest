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
        readonly ScenarioContext context;
        readonly Settings _settings;
        readonly CommonSteps _sharedSteps;
        public Table addItemTable;

        ProductInfoViewSteps(ScenarioContext scenarioContext, Settings p_settings)
        {

            context = scenarioContext;
            _settings = p_settings;
            _sharedSteps = new CommonSteps(scenarioContext, p_settings);
            context["browserId"] = "honor";

        }
        

        
        [When(@"ProductInfo list get with invalid credential")]
        public void WhenProductInfoVisitTheCartInfoAPIWithInvalidCredential()
        {
            context["accessToken"] = "Invalid" + context["accessToken"];
            _sharedSteps.GetProductInfoList();
            context["accessToken"] = context["accessToken"].ToString().Replace("Invalid", "");
        }
        

    }
}
