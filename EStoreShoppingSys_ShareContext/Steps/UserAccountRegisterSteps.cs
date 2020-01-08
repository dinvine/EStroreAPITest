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
        readonly ScenarioContext context;
        readonly Settings _settings;
        readonly CommonSteps _sharedSteps;

        UserAccountRegisterSteps(ScenarioContext scenarioContext,Settings p_settings)
        {

            context = scenarioContext;

            _settings = p_settings;

            _sharedSteps = new CommonSteps(scenarioContext, p_settings);

            context["browserId"] = "honor";

        }

        [Given(@"register with '(.*)' username and '(.*)' password")]
        [When(@"register with '(.*)' username and '(.*)' password")]
        public void GivenRegisterOnWithUsernameAndPassword(string username, string password)
        {
            _sharedSteps.GivenGenerateTheUsernameAndPasswordAt( username, password);

                _sharedSteps.WhenVisitTheRegisterAPIWithTheUsernameAndPassword();
        }










    }
}
