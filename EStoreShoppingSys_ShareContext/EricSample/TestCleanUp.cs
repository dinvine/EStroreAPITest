using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Io.Cucumber.Messages;
using TechTalk.SpecFlow;
using SpecflowAPIAutomation.Base;
using SpecflowAPIAutomation.Steps;

namespace SpecflowAPIAutomation.Hooks
{
    [Binding]
    public class TestCleanUp
    {
        private CommonSteps _common;

        private AuthenticationSteps _auth;
        //Context injection
        private Settings _settings;
        public TestCleanUp(Settings settings)
        {
            _settings = settings;
            _common = new CommonSteps(_settings);
        }

        [AfterScenario("CreateCart")]
        public void DeleteCart()
        {
            _common.GivenIPerformADELETEOperationWithParameter("DeleteCart", "/e-store/cart/delete", "cartId");
            _common.ThenIShouldHaveTheFollowingResponseWithStatusCode("OK");
        }

        [When(@"I delete the above user account")]
        [AfterScenario("RegisterUser")]
        public void DeleteAccount()
        {
            var value = ScenarioContext.Current.Get<string>("accountNumber");
            ScenarioContext.Current.Remove("account_number");
            ScenarioContext.Current.Add("account_number", value);

            _common.GivenIPerformADELETEOperationWithParameter("DeleteAccount", "/e-store/authentication/delete", "account_number");
        }
    }
}