using System;
using TechTalk.SpecFlow;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using EStoreShoppingSys.Model;


namespace EStoreShoppingSys.Steps
{
    [Binding]
    public class EditItemsInCartSteps
    {
        readonly ScenarioContext context;
        Settings _settings;
        readonly CommonSteps _sharedSteps;
        public Table addItemTable;

        EditItemsInCartSteps(ScenarioContext scenarioContext, Settings p_settings)
        {

            context = scenarioContext;
            _settings = p_settings;
            _sharedSteps = new CommonSteps(scenarioContext, p_settings);
            context["browserId"] = "honor";

        }

        [When(@"CARTADDITEM delete all the items from cart")]
        public void WhenCARTADDITEMDeleteAllTheItemsFromCart()
        { 
            foreach(var row in addItemTable.Rows)
            {
                _sharedSteps.GivenDeleteOneRecordOfItemFromCart(row[0]);
            }
        }

        [Then(@"CARTADDITEM  cart should be empty")]
        public void ThenCARTADDITEMCartShouldBeEmpty()
        {
            _settings = (Settings)context["RestSettings"];
            JObject cartInfoJson = JObject.Parse(_settings.MyRestResponse.Content);

            _sharedSteps.ThenGetResponseJsonWith2LevelItemEqualTo("datas", "amountDue", "0");

            Assert.AreEqual(cartInfoJson["datas"]["amountDue"].ToString(), "0", "Test fail due to  Cart amountDue <>0");
            Assert.IsEmpty(cartInfoJson["datas"]["items"], "Test fail due to cart has items left after all the items deleted");
        }

        [Given(@"CARTADDITEM add the valid items table to cart with invalid credential")]
        public void GivenCARTADDITEMAddTheValidItemsTableToCartWithInvalidCredential(Table table)
        {
            addItemTable = table;
            context["accessToken"] = "Invalid" + context["accessToken"];
            _sharedSteps.GivenAddTheValidItemsTableToCart(table);
            context["accessToken"] =context["accessToken"].ToString().Replace("Invalid","");
        }

        [Given(@"CARTADDITEM add the items in table to cart with invalid cartid")]
        public void GivenCARTADDITEMAddTheItemsInTableToCartWithInvalidCartid(Table table)
        {
            addItemTable = table;
            context["cartId"] = "Invalid" + context["cartId"];
            _sharedSteps.GivenAddTheValidItemsTableToCart(table);
            context["cartId"] = context["cartId"].ToString().Replace("Invalid", "");
        }

        [When(@"CARTADDITEM delete the valid items table from cart with invalid credential")]
        [Given(@"CARTADDITEM delete the valid items table from cart with invalid credential")]
        public void GivenCARTADDITEMDeleteTheValidItemsTableFromCartWithInvalidCredential(Table table)
        { 
            context["accessToken"] = "Invalid" + context["accessToken"];
            _sharedSteps.GivenAddTheValidItemsTableToCart(table);
            context["accessToken"] = context["accessToken"].ToString().Replace("Invalid", "");
        }


        [When(@"CARTADDITEM delete the items in table from cart")]
        [Given(@"CARTADDITEM delete the items in table from cart")]
        public void GivenCARTADDITEMDeleteTheItemsInTableToCart(Table table)
        {       
 

            foreach (var row in table.Rows)
            {
                _sharedSteps.GivenDeleteOneRecordOfItemFromCart(row[0].Trim());
            }

        }





    }
}
