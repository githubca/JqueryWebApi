using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmployeeDataAccess;

namespace WebApi
{
    public class EmployeeSecurity
    {
        public static bool Login(string username, string password)
        {
            using (TestEntities db = new TestEntities())
            {
                return db.users.Any(u => u.username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                u.password == password);
            }
        }
    }
}