using TechTalk.SpecFlow;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using EStoreShoppingSys.Model;

namespace EStoreShoppingSys.Steps
{
    [Binding]
    public class UserAccountDeleteSteps
    {
        readonly ScenarioContext _scenarioContext;
        readonly Settings _settings;
        readonly SharedSteps _sharedSteps;
        public Table addItemTable;

        UserAccountDeleteSteps(ScenarioContext scenarioContext, Settings p_settings)
        {

            _scenarioContext = scenarioContext;
            _settings = p_settings;
            _sharedSteps = new SharedSteps(scenarioContext, p_settings);
            _scenarioContext["browserId"] = "honor";

        }
        [Given(@"AccountDelete Register And Login And CreateCart")]
        public void GivenAccountDeleteRegisterAndLoginAndCreateCart()
        {
            _sharedSteps.RegisterLoginAndCreateCart();
        }
        
        [When(@"AccountDelete delete the account number")]
        public void WhenAccountDeleteDeleteTheAccountNumber()
        {
            _sharedSteps.GivenDeleteAccount();
        }
        
        [When(@"AccountDelete delete the account number with invalid token")]
        public void WhenAccountDeleteDeleteTheAccountNumberWithInvalidToken()
        {
            _scenarioContext["accessToken"] = "Invalid" + _scenarioContext["accessToken"];
            _sharedSteps.GivenDeleteAccount();
            _scenarioContext["accessToken"] = _scenarioContext["accessToken"].ToString().Replace("Invalid", "");
        }
        
        [When(@"AccountDelete delete the account number with invalid account_number")]
        public void WhenAccountDeleteDeleteTheAccountNumberWithInvalidAccount_Number()
        {
            _scenarioContext["accountNumber"] = "Invalid" + _scenarioContext["accountNumber"];
            _sharedSteps.GivenDeleteAccount();
            _scenarioContext["accountNumber"] = _scenarioContext["accountNumber"].ToString().Replace("Invalid", "");

        }

        [Then(@"AccountDelete  should give  response of '(.*)'")]
        public void ThenAccountDeleteShouldGiveResponseOf(string p0)
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
            if (p0 == "AccountNumberError")
            {
                _sharedSteps.ThenShouldGetResponseStatusOf("OK");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("code", "0");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("error", "True");
                _sharedSteps.ThenWithItemNamedContainingSubstring("message", "account number");
            }
        }
        
        [Then(@"AccountDelete should give json with '(.*)' containing items '(.*)'")]
        public void ThenAccountDeleteShouldGiveJsonWithContainingItems(string datas, string items)
        {
            _sharedSteps.ThenGetResponseBodyWithIncluding(datas, items);
            Assert.AreEqual(_scenarioContext["accountNumber"], _scenarioContext["accountNumber~Existing"], "Test fail due to accountNumber in response body is not equal to the original one");
        }
    }
}
