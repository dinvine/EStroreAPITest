using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using EStoreShoppingSys;


namespace EStoreShoppingTest
{
    [TestFixture]
    public class EmpTest
    {
        List<EmployeeDetails> li;
        [Test]
        public void CheckDetails()
        {
            
            EmployeeInfo empInfo = new EmployeeInfo();
            li = empInfo.getAllUsers();
            foreach (var item in li)
            {
                Assert.IsNotNull(item.id);
                Assert.IsNotNull(item.name);
                Assert.IsNotNull(item.gender);
                Assert.IsNotNull(item.salary);
            }
        }
        [Test]
        public void TestLogin()
        {
            EmployeeInfo empInfo = new EmployeeInfo();
            string regularReturnStr = empInfo.login("aa", "bb");
            string emptyReturnStr = empInfo.login("", "");
            string adminReturnStr = empInfo.login("Admin", "Admin");
            Assert.AreEqual("userid or password empty!", emptyReturnStr);
            Assert.AreEqual("welcome " + "aa", regularReturnStr);
            Assert.AreEqual("welcome admin!", adminReturnStr);
        }
        [Test]
        public void getUserDetials()
        {
            EmployeeInfo empInfo = new EmployeeInfo();
            var empGetList = empInfo.getEmploeeDetail(101);
            foreach (var item in empGetList)
            {
                Assert.AreEqual(item.id, 101, "Test fail due to get back wrong id ");
                Assert.AreEqual(item.name, "bb", "Test fail due to get back wrong name ");
            }
        }
        static void Main(string[] args)
        {

        }
    }
}
