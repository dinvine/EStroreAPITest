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
    public class UserLoginSteps  
    {
        readonly ScenarioContext context;
        readonly Settings _settings;
        readonly CommonSteps _sharedSteps;

        UserLoginSteps(ScenarioContext scenarioContext, Settings p_settings)
        {

            context = scenarioContext;
            _settings = p_settings;
            _sharedSteps = new CommonSteps(scenarioContext, p_settings);
            context["browserId"] = "honor";

        }

        [Given(@"get the existing username and password on '(.*)'")]
        public void GivenGetTheExistingUsernameAndPassword(string endpoint)
        {
            _sharedSteps.GivenGenerateTheUsernameAndPasswordAt( "random", "random");
            _sharedSteps.WhenVisitTheRegisterAPIWithTheUsernameAndPassword();
        }



        [Given(@"get the unregisted username and password")]
        public void GivenGetTheUnregistedUsernameAndPasswordOn()
        {
            _sharedSteps.GivenGenerateTheUsernameAndPasswordAt( "Unexisting_9[@]#~", "random");
        }
       

        [Then(@"TokenAPI should give json with '(.*)' containing items '(.*)'")]
        public void ThenTokenAPIShouldGiveJsonWithP0ContainingItems(string datas, string items)
        {
            _sharedSteps.ThenGetResponseBodyWithIncluding(datas, items);
        }

        [Given(@"register login and  get valid token")]
        [When(@"register login and  get valid token")]
        public void GivenRegisterOnWithUsernameAndPassword()
        {
            _sharedSteps.GivenGenerateTheUsernameAndPasswordAt("random", "random");
            _sharedSteps.WhenVisitTheRegisterAPIWithTheUsernameAndPassword();
            _sharedSteps.GivenGetTokenAtEndpoint("valid");
        }


    }
}
