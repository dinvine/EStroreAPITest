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
    public class CartInfoViewSteps
    {
        readonly ScenarioContext context;
        readonly Settings _settings;
        readonly CommonSteps _sharedSteps;
        public Table addItemTable;

        CartInfoViewSteps(ScenarioContext scenarioContext, Settings p_settings)
        {

            context = scenarioContext;
            _settings = p_settings;
            _sharedSteps = new CommonSteps(scenarioContext, p_settings);
            context["browserId"] = "honor";

        }

        [When(@"Cartinfo get products included")]
        [Then(@"Cartinfo get products included")]

        public void GetCartInfo()
        {
            string baseUrlEStoreBaseURL = "EStoreBaseURL";
            string endPointURL = "CartInfoEndPoint";
            RequestParams requestParams = new RequestParams();
            //headers
            requestParams.Headers.Add("browserId", context["browserId"].ToString());
            requestParams.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //parameters
            //requestParams.Parameters.Add(-------------);
            //QueryParameters
            requestParams.QueryParameters.Add("account_number", context["accountNumber"].ToString());

                 _sharedSteps.RequestInURLEncodedFromAndValidateTheResponse(baseUrlEStoreBaseURL, Method.GET, endPointURL, requestParams);
                //_sharedSteps.RequestInURLEncodedFromAndValidateTheResponse2<CartInfo>(baseUrlEStoreBaseURL, Method.GET, endPointURL, requestParams);
               // _sharedSteps.RequestInURLEncodedFromAndValidateTheResponse2<ResponseError>(baseUrlEStoreBaseURL, Method.GET, endPointURL, requestParams);
            
            context["settings"] = _settings;

        }

        [When(@"CartInfo visit the cart info API with invalid credential")]
        public void WhenCartInfoVisitTheCartInfoAPIWithInvalidCredential()
        {

            context["accessToken"] = "Invalid" + context["accessToken"];
            GetCartInfo();
            context["accessToken"] = context["accessToken"].ToString().Replace("Invalid", "");

        }
        
        [When(@"CartInfo visit the cart info API with invalid accountNumber")]
        public void WhenCartInfoVisitTheCartInfoAPIWithInvalidCartid()
        {
            context["accountNumber"] = "Invalid" + context["accountNumber"];
            GetCartInfo();
            context["accountNumber"] = context["accountNumber"].ToString().Replace("Invalid", "");
        }

        [Then(@"CartInfo  cart should be empty")]
        public void ThenCARTShouldBeEmpty()
        {
            
            GetCartInfo();
            JObject cartInfoJson = JObject.Parse(_settings.MyRestResponse.Content);
            _sharedSteps.ThenGetResponseJsonWith2LevelItemEqualTo("datas", "amountDue", "0");
            Assert.AreEqual(cartInfoJson["datas"]["amountDue"].ToString(), "0", "Test fail due to  Cart amountDue <>0");
            Assert.IsEmpty(cartInfoJson["datas"]["items"], "Test fail due to cart has items left after all the items deleted");
        }

        [Then(@"CartInfo items in cart should be same to the table")]
        public void ThenCARTADDITEMItemsInCartShouldSameToTheTable(Table table)
        {

            Double amountDue = 0;
            GetCartInfo();
            JObject cartInfoJson = JObject.Parse(_settings.MyRestResponse.Content);

            for (int i = 0; i < table.Rows.Count; i++)
            {
                amountDue += Double.Parse(table.Rows[i][3].Trim());
                Assert.AreEqual(cartInfoJson["datas"]["items"][i]["itemId"].ToString(), table.Rows[i]["itemId"], "test fail due to itemid is not equal between table and cartinfo");
                Assert.AreEqual(cartInfoJson["datas"]["items"][i]["quantity"].ToString(), table.Rows[i]["quantity"], "test fail due to itemid is not equal between table and cartinfo");
            }
            amountDue = Math.Round(amountDue, 2);
            context["cartAmountDue"] = amountDue.ToString();
            Assert.AreEqual(context["cartAmountDue"], cartInfoJson["datas"]["amountDue"].ToString(), "Test fail due to amountDue of Cart is wrong");

        }

    }
}
