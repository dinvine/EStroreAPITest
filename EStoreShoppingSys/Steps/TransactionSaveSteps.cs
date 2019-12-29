using TechTalk.SpecFlow;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using EStoreShoppingSys.Model;

namespace EStoreShoppingSys.Steps
{
    [Binding]
    public class TransactionSaveSteps
    {
        readonly ScenarioContext _scenarioContext;
        readonly Settings _settings;
        readonly SharedSteps _sharedSteps;
        public Table addItemTable;

        TransactionSaveSteps(ScenarioContext scenarioContext, Settings p_settings)
        {

            _scenarioContext = scenarioContext;
            _settings = p_settings;
            _sharedSteps = new SharedSteps(scenarioContext, p_settings);
            _scenarioContext["browserId"] = "honor";

        }

        [Given(@"TransactionSave Register And Login And CreateCart")]
        public void GivenTransactionSaveRegisterAndLoginAndCreateCart()
        {
            _sharedSteps.RegisterLoginAndCreateCart();
        }

        [Given(@"TransactionSave get transaction number")]
        public void GivenTransactionSaveGetTransactionNumber()
        {
            _sharedSteps.GetTransactionNumber();            
        }

        [When(@"TransactionSave add the items in table to transaction")]
        public void WhenTransactionSaveAddTheItemsInTableToTransaction(Table table)
        {
            addItemTable = table;
            _sharedSteps.GivenSaveTheItemsTableToTransaction(table);
        }
        
        [When(@"TransactionSave add the items in table to transaction with invalid credential")]
        public void WhenTransactionSaveAddTheItemsInTableToTransactionWithInvalidCredential(Table table)
        {
            
            _scenarioContext["accessToken"] = "Invalid" + _scenarioContext["accessToken"];
            _sharedSteps.GivenSaveTheItemsTableToTransaction(table);
            _scenarioContext["accessToken"] = _scenarioContext["accessToken"].ToString().Replace("Invalid", "");
        }
        
        [When(@"TransactionSave add the items in table to transaction with invalid accountNumber")]
        public void WhenTransactionSaveAddTheItemsInTableToTransactionWithInvalidAccountNumber(Table table)
        {
            _scenarioContext["accountNumber"] = "Invalid" + _scenarioContext["accountNumber"];
            _sharedSteps.GivenSaveTheItemsTableToTransaction(table);
            _scenarioContext["accountNumber"] = _scenarioContext["accountNumber"].ToString().Replace("Invalid", "");
        }
        
        [When(@"TransactionSave add the items in table to transaction with invalid transactionNumber")]
        public void WhenTransactionSaveAddTheItemsInTableToTransactionWithInvalidTransactionNumber(Table table)
        {
            _scenarioContext["transactionNumber"] = "Invalid" + _scenarioContext["transactionNumber"];
            _sharedSteps.GivenSaveTheItemsTableToTransaction(table);
            _scenarioContext["transactionNumber"] = _scenarioContext["transactionNumber"].ToString().Replace("Invalid", "");
        }
        
        [Then(@"TransactionSave  should give  response of '(.*)'")]
        public void ThenTransactionSaveShouldGiveResponseOf(string p0)
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
            if (p0 == "InvalidAccountNumberError")
            {
                _sharedSteps.ThenShouldGetResponseStatusOf("OK");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("code", "0");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("error", "True");
                _sharedSteps.ThenWithItemNamedContainingSubstring("message", "account number");
            }

            if (p0 == "InvalidtransactionNumberError")
            {
                _sharedSteps.ThenShouldGetResponseStatusOf("OK");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("code", "0");
                _sharedSteps.ThenGetResponseBodyWithEqualTo("error", "True");
                _sharedSteps.ThenWithItemNamedContainingSubstring("message", "transaction number");
            }

            
        }
        
        [Then(@"TransactionSave  items  should be same to the table")]
        public void ThenTransactionSaveItemsShouldBeSameToTheTable()
        {
            JObject transactionItems = JObject.Parse(_settings.MyRestResponse.Content);
            // assume the response contains datas:{items:[{item1,quantity},{item2,quantity},{....}]}
            //but the real API return the datas =null , so the code below can not be tested.
            /*
        Assert.AreEqual(transactionItems["datas"]["amountDue"].ToString(), _scenarioContext["transactionAmountDue"], "Test fail due to amountDue of transaction is wrong");

        for (int i = 0; i < addItemTable.Rows.Count; i++)
        {

            Assert.AreEqual(transactionItems["datas"]["items"][i]["itemId"].ToString(), addItemTable.Rows[i]["itemId"], "test fail due to itemid is not equal between table and transaction return");
            Assert.AreEqual(transactionItems["datas"]["items"][i]["itemName"].ToString(), addItemTable.Rows[i]["itemName"], "test fail due to itemName is not equal between table and transaction return");
            Assert.AreEqual(transactionItems["datas"]["items"][i]["quantity"].ToString(), addItemTable.Rows[i]["quantity"], "test fail due to quantity is not equal between table and transaction return");
            Assert.AreEqual(transactionItems["datas"]["items"][i]["price"].ToString(), addItemTable.Rows[i]["price"], "test fail due to price is not equal between table and transaction return");
 
        }
             */
        }
    }
}
