using TechTalk.SpecFlow;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using EStoreShoppingSys.Model;

namespace EStoreShoppingSys.Steps
{
    [Binding]
    public class TransactionSaveSteps
    {
        readonly ScenarioContext context;
        readonly Settings _settings;
        readonly CommonSteps _sharedSteps;
        public Table addItemTable;

        TransactionSaveSteps(ScenarioContext scenarioContext, Settings p_settings)
        {

            context = scenarioContext;
            _settings = p_settings;
            _sharedSteps = new CommonSteps(scenarioContext, p_settings);
            context["browserId"] = "honor";

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
            _sharedSteps.GivenSaveTheItemsTableToTransaction(table,"TransactionSave");
        }
        
        [When(@"TransactionSave add the items in table to transaction with invalid credential")]
        public void WhenTransactionSaveAddTheItemsInTableToTransactionWithInvalidCredential(Table table)
        {
            
            context["accessToken"] = "Invalid" + context["accessToken"];
            _sharedSteps.GivenSaveTheItemsTableToTransaction(table,"invalidToken");
            context["accessToken"] = context["accessToken"].ToString().Replace("Invalid", "");
        }
        
        [When(@"TransactionSave add the items in table to transaction with invalid accountNumber")]
        public void WhenTransactionSaveAddTheItemsInTableToTransactionWithInvalidAccountNumber(Table table)
        {
            context["accountNumber"] = "Invalid" + context["accountNumber"];
            _sharedSteps.GivenSaveTheItemsTableToTransaction(table,"invalidAccountNumber");
            context["accountNumber"] = context["accountNumber"].ToString().Replace("Invalid", "");
        }
        
        [When(@"TransactionSave add the items in table to transaction with invalid transactionNumber")]
        public void WhenTransactionSaveAddTheItemsInTableToTransactionWithInvalidTransactionNumber(Table table)
        {
            context["transactionNumber"] = context["transactionNumber"]+"9791";
            _sharedSteps.GivenSaveTheItemsTableToTransaction(table,"invalidTransactionNum");
            context["transactionNumber"] = context["transactionNumber"].ToString().Replace("9791", "");
        }
        
        
        [Then(@"TransactionSave  items  should be same to the table")]
        public void ThenTransactionSaveItemsShouldBeSameToTheTable()
        {
            JObject transactionItems = JObject.Parse(_settings.MyRestResponse.Content);
            // assume the response contains datas:{items:[{item1,quantity},{item2,quantity},{....}]}
            //but the real API return the datas =null , so the code below can not be tested.
            
        Assert.AreEqual(transactionItems["datas"]["amountDue"].ToString(), context["transactionAmountDue"], "Test fail due to amountDue of transaction is wrong");

        for (int i = 0; i < addItemTable.Rows.Count; i++)
            {

                Assert.AreEqual(transactionItems["datas"]["items"][i]["itemId"].ToString(), addItemTable.Rows[i]["itemId"], "test fail due to itemid is not equal between table and transaction return");
                Assert.AreEqual(transactionItems["datas"]["items"][i]["itemName"].ToString(), addItemTable.Rows[i]["itemName"], "test fail due to itemName is not equal between table and transaction return");
                Assert.AreEqual(transactionItems["datas"]["items"][i]["quantity"].ToString(), addItemTable.Rows[i]["quantity"], "test fail due to quantity is not equal between table and transaction return");
                Assert.AreEqual(transactionItems["datas"]["items"][i]["price"].ToString(), addItemTable.Rows[i]["price"], "test fail due to price is not equal between table and transaction return");
             }
             
        }
    }
}
