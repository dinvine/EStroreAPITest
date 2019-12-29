using TechTalk.SpecFlow;
//using NUnit.Framework;
using EStoreShoppingSys.Model;

namespace EStoreShoppingSys.Steps
{
    [Binding]
    public sealed class PostConditionSteps
    {
        readonly ScenarioContext _scenarioContext;
        readonly Settings _settings;
        readonly SharedSteps _sharedSteps;

        PostConditionSteps(ScenarioContext scenarioContext, Settings p_settings)
        {

            _scenarioContext = scenarioContext;
            _settings = p_settings;
            _sharedSteps = new SharedSteps(scenarioContext, p_settings);
            _scenarioContext["browserId"] = "honor";

        }

        [AfterScenario()]
        public void ScenarioTearDown()
        {
            _sharedSteps.GivenDeleteAccount();
            if (_scenarioContext.ContainsKey("accountNumber"))
            {
                _sharedSteps.ThenShouldGetResponseStatusOf("OK");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("code", "200");
                _sharedSteps.ThenWithItemNamedContainingSubstring("message", "success");
            }
        }
    }
}
