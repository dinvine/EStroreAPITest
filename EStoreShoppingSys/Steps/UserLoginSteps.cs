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
        readonly ScenarioContext _scenarioContext;
        readonly Settings _settings;
        readonly SharedSteps _sharedSteps;

        UserLoginSteps(ScenarioContext scenarioContext, Settings p_settings)
        {

            _scenarioContext = scenarioContext;
            _settings = p_settings;
            _sharedSteps = new SharedSteps(scenarioContext, p_settings);
            _scenarioContext["browserId"] = "honor";

        }

        [Given(@"get the existing username and password on '(.*)'")]
        public void GivenGetTheExistingUsernameAndPassword(string endpoint)
        {
            _sharedSteps.GivenGenerateTheUsernameAndPasswordAt(endpoint, "random", "random");
            _sharedSteps.WhenVisitTheRegisterAPIWithTheUsernameAndPassword(endpoint);
        }



        [Given(@"get the unregisted username and password on '(.*)'")]
        public void GivenGetTheUnregistedUsernameAndPasswordOn(string endpoint)
        {
            _sharedSteps.GivenGenerateTheUsernameAndPasswordAt(endpoint, "Unexisting_9[@]#~", "random");
        }


        [When(@"visit the token API '(.*)' with the  '(.*)' credential")]
        public void WhenVisitTheTokenAPIWithTheCredential(string endpoint, string credentialIsValid)
        {
            _sharedSteps.GivenGetTokenAtEndpoint(credentialIsValid);
        }
        
    [Then(@"TokenAPI  should give  response of '(.*)'")]
    public void ThenTokenAPIShouldGiveResponseOf(string p0)
    {
            if (p0 == "OK")
            {
                _sharedSteps.ThenShouldGetResponseStatusOf("OK");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("code", "200");
                _sharedSteps.ThenWithItemNamedContainingSubstring("message", "success");
            }
            if (p0 == "CredentialError")
            {
                _sharedSteps.ThenShouldGetResponseStatusOf("OK");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("code", "0");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("error", "True");
                _sharedSteps.ThenWithItemNamedContainingSubstring("message", "username");
            }
        }
        

        [Then(@"TokenAPI should give json with '(.*)' containing items '(.*)'")]
        public void ThenTokenAPIShouldGiveJsonWithP0ContainingItems(string datas, string items)
        {
            _sharedSteps.ThenGetResponseBodyWithIncluding(datas, items);
            Assert.AreEqual(_scenarioContext["accountNumber"], _scenarioContext["accountNumber~Existing"], "Test fail due to accountNumber in response body is not equal to the one get in registration");
        }
    }
}
