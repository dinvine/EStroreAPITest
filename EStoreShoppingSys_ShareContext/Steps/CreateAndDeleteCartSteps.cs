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
        readonly ScenarioContext context;
        readonly Settings _settings;
        readonly CommonSteps _sharedSteps;

        CreateAndDeleteCartSteps(ScenarioContext scenarioContext, Settings p_settings)
        {

            context = scenarioContext;
            _settings = p_settings;
            _sharedSteps = new CommonSteps(scenarioContext, p_settings);
            context["browserId"] = "honor";

        }



        [When(@"CART create  with the  invalid token")]
        public void WhenCARTCreateAtWithTheInvalidToken()
        {
            context["accessToken"] += "####";
            _sharedSteps.GivenCreateCartAtCartCreateEndPoint();
            context["accessToken"] = context["accessToken"].ToString().Replace("####", "");
        }





        [When(@"CART delete  with the invalid token")]
        public void WhenCARTDeleteAtWithTheInvalidToken()
        {

            context["accessToken"] += "invalidToken";
            _sharedSteps.GivenDeleteCart();
            context["accessToken"] = context["accessToken"].ToString().Replace("invalidToken", "");
        }

    }
}
