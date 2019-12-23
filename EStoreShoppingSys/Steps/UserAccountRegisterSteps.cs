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
        readonly ScenarioContext _scenarioContext;
        readonly Settings _settings;
        readonly SharedSteps _sharedSteps;

        UserAccountRegisterSteps(ScenarioContext scenarioContext,Settings p_settings)
        {

            _scenarioContext = scenarioContext;

            _settings = p_settings;

            _sharedSteps = new SharedSteps(scenarioContext, p_settings);

        }
        [When(@"register on '(.*)' with '(.*)' username and '(.*)' password")]
        public void GivenRegisterOnWithUsernameAndPassword(string endPoint, string username, string password)
        {
            _sharedSteps.GivenGenerateTheUsernameAndPasswordAt(endPoint, username, password);
            _sharedSteps.WhenVisitTheRegisterAPIWithTheUsernameAndPassword(endPoint);
        }

        [Then(@"register should get  response of '(.*)'")]
        public void ThenRegisterShouldGetResponseOf(string p0)
        {
            if(p0=="OK")
            {
                _sharedSteps.ThenShouldGetResponseStatusOf("OK");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("code", "200");
                _sharedSteps.ThenWithItemNamedContainingSubstring("message", "success");
            }
            if (p0 == "RegisteredError")
            {
                _sharedSteps.ThenShouldGetResponseStatusOf("OK");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("code", "0");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("error", "True");
                _sharedSteps.ThenWithItemNamedContainingSubstring("message", "registered");
            }
            if (p0 == "UsernameError")
            {
                _sharedSteps.ThenShouldGetResponseStatusOf("OK");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("code", "0");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("error", "True");
                _sharedSteps.ThenWithItemNamedContainingSubstring("message", "username");
            }
            if (p0 == "PasswordError")
            {
                _sharedSteps.ThenShouldGetResponseStatusOf("OK");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("code", "0");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("error", "True");
                _sharedSteps.ThenWithItemNamedContainingSubstring("message", "password");
            }


        }

        [Then(@"register should get  \['(.*)'] including '(.*)'")]
        public void ThenRegisterShouldGetIncluding(string p0, string p1)
        {            
            _sharedSteps.ThenGetResponseBodyWithIncluding(p0, p1);
        }

        






    }
}
