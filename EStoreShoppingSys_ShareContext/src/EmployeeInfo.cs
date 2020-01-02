using System;
using System.Collections.Generic;
using System.Text;

namespace EStoreShoppingSys
{
    public class EmployeeInfo
    {
        public string login(string username,string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return "userid or password empty!";
            }
            else
            {
                if (username == "Admin" && password == "Admin")
                {
                    return "welcome admin!";
                }
                else
                {
                    return "welcome " + username;
                }
            }
        }
        public List<EmployeeDetails> getAllUsers()
        {
            List<EmployeeDetails> empList = new List<EmployeeDetails>();
            empList.Add(new EmployeeDetails { id = 100, name = "aa", salary = 50000, gender = "M" });
            empList.Add(new EmployeeDetails { id = 101, name = "bb", salary = 60000, gender = "f" });
            empList.Add(new EmployeeDetails { id = 102, name = "cc", salary = 70000, gender = "f" });
            empList.Add(new EmployeeDetails { id = 103, name = "dd", salary = 80000, gender = "M" });
            empList.Add(new EmployeeDetails { id = 104, name = "ee", salary = 90000, gender = "M" });
            empList.Add(new EmployeeDetails { id = 105, name = "ff", salary = 100000, gender = "M" });
            empList.Add(new EmployeeDetails { id = 106, name = "gg", salary = 120000, gender = "M" });
            return empList;
        }
        public List<EmployeeDetails> getEmploeeDetail(int employeeid)
        {
            List<EmployeeDetails> employeeList = new List<EmployeeDetails>();
            EmployeeInfo empInfo = new EmployeeInfo();
            var li = empInfo.getAllUsers();
            foreach(var item in li)
            {
                if (item.id == employeeid)
                {
                    employeeList.Add(item);
                }
            }
            return employeeList;
        }
    }
}
