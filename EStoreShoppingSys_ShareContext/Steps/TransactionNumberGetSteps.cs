using TechTalk.SpecFlow;
using EStoreShoppingSys.Model;

namespace EStoreShoppingSys.Steps
{
    [Binding]
    public class TransactionNumberGetSteps
    {
        readonly ScenarioContext context;
        readonly Settings _settings;
        readonly CommonSteps _sharedSteps;
        public Table addItemTable;

        TransactionNumberGetSteps(ScenarioContext scenarioContext, Settings p_settings)
        {

            context = scenarioContext;
            _settings = p_settings;
            _sharedSteps = new CommonSteps(scenarioContext, p_settings);
            context["browserId"] = "honor";

        }
        
        
        [When(@"TransactionNumber get with invalid credential")]
        public void WhenTransactionNumberVisitTheCartInfoAPIWithInvalidCredential()
        {
            context["accessToken"] = "Invalid" + context["accessToken"];
            _sharedSteps.GetTransactionNumber();
            context["accessToken"] = context["accessToken"].ToString().Replace("Invalid", "");
        }
        

        [Then(@"TransactionNumber should give json with '(.*)' containing items '(.*)'")]
        public void ThenTransactionNumberShouldGiveJsonWithContainingItems(string datas, string items)
        {
            _sharedSteps.ThenGetResponseBodyWithIncluding(datas, items);
            if (context.ContainsKey("transactionNumber~Existing"))
                {
                context["transactionNumber"] = context["transactionNumber~Existing"];
                context.Remove("transactionNumber~Existing");
                }
            
        }


    }
}
