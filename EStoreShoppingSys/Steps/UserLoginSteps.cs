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
            _sharedSteps.GivenGenerateTheUsernameAndPasswordAt(endpoint, "random", "random");
        }



        [When(@"visit the token API '(.*)' with the  username and password and browserid")]
        public void WhenVisitTheTokenAPIWithTheUsernameAndPasswordAndBrowserid(string p0)
        {
            _settings.MyRestClient = new RestClient(ConfigurationManager.AppSettings["EStoreBaseURL"]);
            _settings.MyRestRequest = new RestRequest(ConfigurationManager.AppSettings[p0], Method.POST);
            _settings.MyRestRequest.AddParameter("username", _scenarioContext["username"], ParameterType.GetOrPost);
            _settings.MyRestRequest.AddParameter("password", _scenarioContext["password"], ParameterType.GetOrPost);
            _settings.MyRestRequest.AddParameter("browserid", _scenarioContext["browserId"], ParameterType.GetOrPost);
            _settings.MyRestResponse = _settings.MyRestClient.Execute(_settings.MyRestRequest);
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
