using TechTalk.SpecFlow;
//using NUnit.Framework;
using EStoreShoppingSys.Model;
using Gurock.TestRail;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace EStoreShoppingSys.Steps
{
    [Binding]
    public sealed class PostConditionSteps
    {
        readonly ScenarioContext context;
        readonly Settings _settings;
        readonly CommonSteps _sharedSteps;

        PostConditionSteps(ScenarioContext scenarioContext, Settings p_settings)
        {

            context = scenarioContext;
            _settings = p_settings;
            _sharedSteps = new CommonSteps(scenarioContext, p_settings);
            context["browserId"] = "honor";

        }

        [AfterScenario()]
        public void ScenarioTearDown()
        {
            _sharedSteps.GivenDeleteAccount();
            if (context.ContainsKey("accountNumber"))
            {
                _sharedSteps.ThenShouldGetResponseStatusOf("OK");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("code", "200");
                _sharedSteps.ThenWithItemNamedContainingSubstring("message", "success");
            }

            APIClient client = new APIClient("https://dinvine.testrail.io/");
            client.User = "dinvine@qq.com"; //user e-mail
            client.Password = "8pLWKCUv7.3UUWKNZB3h"; //user password
            Dictionary<string, object> testResult = new Dictionary<string, object>();

            if (null != context.TestError)
            {

                testResult["status_id"] = "5"; //failed;

                testResult["comment"] = context.TestError.ToString();
            }

            else

            {
                testResult["status_id"] = "1"; //passed               
            }
            string strScenarioTitle = context.ScenarioInfo.Title;
            string strTestRailCaseId = strScenarioTitle.Trim().Split(' ')[0];
            strTestRailCaseId = strTestRailCaseId.Substring(1, strTestRailCaseId.Length - 1);
            if (int.Parse(strTestRailCaseId) <= 500)
            {
                string strTestRailAddResultURL = @"add_result/" + strTestRailCaseId;
                client.SendPost(strTestRailAddResultURL, testResult); //hardcoded test id.

            }
        }
    }
}
