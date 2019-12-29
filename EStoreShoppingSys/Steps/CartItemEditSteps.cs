
using TechTalk.SpecFlow;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using EStoreShoppingSys.Model;


namespace EStoreShoppingSys.Steps
{
    [Binding]
    public class EditItemsInCartSteps
    {
        readonly ScenarioContext _scenarioContext;
        readonly Settings _settings;
        readonly SharedSteps _sharedSteps;
        public Table addItemTable;

        EditItemsInCartSteps(ScenarioContext scenarioContext, Settings p_settings)
        {

            _scenarioContext = scenarioContext;
            _settings = p_settings;
            _sharedSteps = new SharedSteps(scenarioContext, p_settings);
            _scenarioContext["browserId"] = "honor";

        }






        [Given(@"CARTADDITEM Register And Login And CreateCart")]
        public void GivenCARTADDITEMRegisterAndLoginAndCreateCart()
        {
            _sharedSteps.RegisterLoginAndCreateCart();
        }

        [Given(@"CARTADDITEM add the items in table to cart")]
        public void GivenCARTADDTheItemsInTableToCart(Table table)
        {
            addItemTable = table;
            _sharedSteps.GivenAddTheValidItemsTableToCart(table);
        }

        
        [Then(@"CARTADDITEM  should give  response of '(.*)'")]
        public void ThenCARTADDITEMShouldGiveResponseOf(string p0)
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
            if (p0 == "InvalidItemError")
            {
                _sharedSteps.ThenShouldGetResponseStatusOf("OK");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("code", "0");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("error", "True");
                _sharedSteps.ThenWithItemNamedContainingSubstring("message", "itemid");
            }
            if (p0 == "InvalidCartError")
            {
                _sharedSteps.ThenShouldGetResponseStatusOf("OK");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("code", "0");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("error", "True");
                _sharedSteps.ThenWithItemNamedContainingSubstring("message", "cartid");
            }

        }

        [Then(@"CARTADDITEM items in cart should be same to the table")]
        public void ThenCARTADDITEMItemsInCartShouldSameToTheTable()
        {
            JObject cartInfoJson = _sharedSteps.GetCartInfo();

            Assert.AreEqual(cartInfoJson["datas"]["amountDue"].ToString(), _scenarioContext["cartAmountDue"], "Test fail due to amountDue of Cart is wrong");

            for(int i=0;i< addItemTable.Rows.Count; i++)
            {
                Assert.AreEqual(cartInfoJson["datas"]["items"][i]["itemId"].ToString(), addItemTable.Rows[i]["itemId"], "test fail due to itemid is not equal between table and cartinfo");
                Assert.AreEqual(cartInfoJson["datas"]["items"][i]["quantity"].ToString(), addItemTable.Rows[i]["quantity"], "test fail due to itemid is not equal between table and cartinfo");
            }
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
            _sharedSteps.ThenGetResponseJsonWith2LevelItemEqualTo("datas", "amountDue", "0");
            JObject cartInfoJson = _sharedSteps.GetCartInfo();
            Assert.AreEqual(cartInfoJson["datas"]["amountDue"].ToString(), "0", "Test fail due to  Cart amountDue <>0");
            Assert.IsEmpty(cartInfoJson["datas"]["items"], "Test fail due to cart has items left after all the items deleted");
        }

        [Given(@"CARTADDITEM add the valid items table to cart with invalid credential")]
        public void GivenCARTADDITEMAddTheValidItemsTableToCartWithInvalidCredential(Table table)
        {
            addItemTable = table;
            _scenarioContext["accessToken"] = "Invalid" + _scenarioContext["accessToken"];
            _sharedSteps.GivenAddTheValidItemsTableToCart(table);
            _scenarioContext["accessToken"] =_scenarioContext["accessToken"].ToString().Replace("Invalid","");
        }

        [Given(@"CARTADDITEM add the items in table to cart with invalid cartid")]
        public void GivenCARTADDITEMAddTheItemsInTableToCartWithInvalidCartid(Table table)
        {
            addItemTable = table;
            _scenarioContext["cartId"] = "Invalid" + _scenarioContext["cartId"];
            _sharedSteps.GivenAddTheValidItemsTableToCart(table);
            _scenarioContext["cartId"] = _scenarioContext["cartId"].ToString().Replace("Invalid", "");
        }

        [Given(@"CARTADDITEM delete the valid items table from cart with invalid credential")]
        public void GivenCARTADDITEMDeleteTheValidItemsTableFromCartWithInvalidCredential(Table table)
        { 
            _scenarioContext["accessToken"] = "Invalid" + _scenarioContext["accessToken"];
            _sharedSteps.GivenAddTheValidItemsTableToCart(table);
            _scenarioContext["accessToken"] = _scenarioContext["accessToken"].ToString().Replace("Invalid", "");
        }

        [Given(@"CARTADDITEM delete the items in table to cart")]
        public void GivenCARTADDITEMDeleteTheItemsInTableToCart(Table table)
        {
             
            foreach (var row in table.Rows)
            {
                _sharedSteps.GivenDeleteOneRecordOfItemFromCart(row[0].ToString());
            } 
        }





    }
}
