using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;


namespace EStoreShoppingSys.Model
{
    public static class FunctionsShared
    {
        public static string GetRandomString(int length)
        {
            Random r = new Random();
            string rStr = "";
            string charNumStr = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";
            for (int i = 0; i < length; i++)
            {
                rStr += charNumStr[r.Next(charNumStr.Length)];
            }
            return rStr;
        }

        public static Dictionary<string, string> GetResponseHeaderDict(IRestResponse restResponse) {

            Dictionary<string, string> headerList = new Dictionary<string, string>();
            string[] keyPairs;
            foreach (var item in restResponse.Headers)
            {
                keyPairs = item.ToString().Split('=');
                headerList.Add(keyPairs[0], keyPairs[1]);
                Console.WriteLine(keyPairs[0] + ">>>>>>>>>" + keyPairs[1]);
            }
            return headerList;
        }

        public static async Task<IRestResponse> ExecuteAsyncRequest(this RestClient client, IRestRequest request) 
        {
            var taskCompletionSource = new TaskCompletionSource<IRestResponse>();
            client.ExecuteAsync(request, restResponse =>
            {
                if (restResponse.ErrorException != null)
                {
                    const string message = "Error retrieving response.";
                    throw new ApplicationException(message, restResponse.ErrorException);
                }
                taskCompletionSource.SetResult(restResponse);
            });
            return await taskCompletionSource.Task;
        }

        public static Boolean CompareJsonWithSchema(string jsonStr,string schemaName)
        {
            JObject jObject= JObject.Parse(jsonStr);
            JSchemaGenerator generator = new JSchemaGenerator();
            Type schemaClass = Type.GetType("EStoreShoppingSys.Model." + schemaName);
            Assert.IsNotNull(schemaClass, "test failed due to schema class not found:" + schemaName);
            JSchema jsonSchema = generator.Generate(schemaClass);
            Boolean isValidJobject = jObject.IsValid(jsonSchema);
            return isValidJobject;
        }

    }

    
}
