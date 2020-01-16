using TechTalk.SpecFlow;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using EStoreShoppingSys.Model;

namespace EStoreShoppingSys.Steps
{
    [Binding]
    public class UserAccountDeleteSteps
    {
        readonly ScenarioContext context;
        readonly Settings _settings;
        readonly CommonSteps _sharedSteps;
        public Table addItemTable;

        UserAccountDeleteSteps(ScenarioContext scenarioContext, Settings p_settings)
        {

            context = scenarioContext;
            _settings = p_settings;
            _sharedSteps = new CommonSteps(scenarioContext, p_settings);
            context["browserId"] = "honor";

        }
        [Given(@"AccountDelete Register And Login And CreateCart")]
        public void GivenAccountDeleteRegisterAndLoginAndCreateCart()
        {
            _sharedSteps.RegisterLoginAndCreateCart();
        }
        

        
        [When(@"AccountDelete delete the account number with invalid token")]
        public void WhenAccountDeleteDeleteTheAccountNumberWithInvalidToken()
        {
            context["accessToken"] = "Invalid" + context["accessToken"];
            _sharedSteps.GivenDeleteAccount();
            context["accessToken"] = context["accessToken"].ToString().Replace("Invalid", "");
        }
        
        [When(@"AccountDelete delete the account number with invalid account_number")]
        public void WhenAccountDeleteDeleteTheAccountNumberWithInvalidAccount_Number()
        {
            context["accountNumber"] = "Invalid" + context["accountNumber"];
            _sharedSteps.GivenDeleteAccount();
            context["accountNumber"] = context["accountNumber"].ToString().Replace("Invalid", "");

        }

        
        [Then(@"AccountDelete should give json with '(.*)' containing items '(.*)'")]
        public void ThenAccountDeleteShouldGiveJsonWithContainingItems(string datas, string items)
        {
            _sharedSteps.ThenAddItemInResponseBodyToScenarioContext(datas, items);
            Assert.AreEqual(context["accountNumber"], context["accountNumber~Existing"], "Test fail due to accountNumber in response body is not equal to the original one");
        }
    }
}
