using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using NUnit.Framework;
namespace RestSharpExample
{
    [TestFixture]
    public class CityWheatherTest
    {
        [Test]
        public void DisplayAllNodesInAPI()
        {
            RestClient restClient = new RestClient("http://restapi.demoqa.com/utilities/weather/city/");
            RestRequest restRequest = new RestRequest("Auckland", Method.GET);
            IRestResponse restResponse = restClient.Execute(restRequest);


            //response status 
         //   Assert.AreEqual("OK", restResponse.StatusCode, "Test fail due to Response StatusCode is not equal to 200");
            Console.WriteLine(restResponse.StatusCode);
            Console.WriteLine(restResponse.ResponseStatus);
            //Header
            Dictionary<string, string> headerList = new Dictionary<string, string>();
            string[] keyPairs=null;
            foreach(var item in restResponse.Headers)
            {
                keyPairs = item.ToString().Split('=');
                headerList.Add(keyPairs[0], keyPairs[1]);
                Console.WriteLine(keyPairs[0] + ">>>>>>>>>" + keyPairs[1]);
            }

            Assert.AreEqual("application/json", headerList["Content-Type"], "Test fail due to the Conten-Type in header is not application json");

            var jObject = JObject.Parse(restResponse.Content);
            Console.WriteLine(jObject.GetValue("City"));
            Assert.AreEqual("Auckland", jObject.GetValue("City"), "Test fail  due to city name not matched");
            Console.WriteLine(jObject.GetValue("Temperature"));
            String temperatureStr = jObject.GetValue("Temperature").ToString();
            string[] temperatureStrArray= temperatureStr.Split(' ');

            Console.WriteLine(float.Parse(temperatureStrArray[0]));
            Assert.IsTrue(Math.Abs(float.Parse(temperatureStrArray[0])) <100, "Test fail  due to the temperature is not in the range of 100");


        }
    }
}
