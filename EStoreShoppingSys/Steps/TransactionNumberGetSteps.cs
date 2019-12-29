using TechTalk.SpecFlow;
using EStoreShoppingSys.Model;

namespace EStoreShoppingSys.Steps
{
    [Binding]
    public class TransactionNumberGetSteps
    {
        readonly ScenarioContext _scenarioContext;
        readonly Settings _settings;
        readonly SharedSteps _sharedSteps;
        public Table addItemTable;

        TransactionNumberGetSteps(ScenarioContext scenarioContext, Settings p_settings)
        {

            _scenarioContext = scenarioContext;
            _settings = p_settings;
            _sharedSteps = new SharedSteps(scenarioContext, p_settings);
            _scenarioContext["browserId"] = "honor";

        }
        [Given(@"TransactionNumber Register And Login And CreateCart")]
        public void GivenTransactionNumberRegisterAndLoginAndCreateCart()
        {
            _sharedSteps.RegisterLoginAndCreateCart();
        }
        
        [When(@"TransactionNumber visit the transaction number API with valid credential")]
        public void WhenTransactionNumberVisitTheTransactionNumberAPIWithValidCredential()
        {
            _sharedSteps.GetTransactionNumber();
        }
        
        [When(@"TransactionNumber visit the cart info API with invalid credential")]
        public void WhenTransactionNumberVisitTheCartInfoAPIWithInvalidCredential()
        {
            _scenarioContext["accessToken"] = "Invalid" + _scenarioContext["accessToken"];
            _sharedSteps.GetTransactionNumber();
            _scenarioContext["accessToken"] = _scenarioContext["accessToken"].ToString().Replace("Invalid", "");
        }
        
        [Then(@"TransactionNumber should give  response of '(.*)'")]
        public void ThenTransactionNumberShouldGiveResponseOf(string p0)
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
        }

        [Then(@"TransactionNumber should give json with '(.*)' containing items '(.*)'")]
        public void ThenTransactionNumberShouldGiveJsonWithContainingItems(string datas, string items)
        {
            _sharedSteps.ThenGetResponseBodyWithIncluding(datas, items);
            if (_scenarioContext.ContainsKey("transactionNumber~Existing"))
                {
                _scenarioContext["transactionNumber"] = _scenarioContext["transactionNumber~Existing"];
                _scenarioContext.Remove("transactionNumber~Existing");
                }
            
        }


    }
}
