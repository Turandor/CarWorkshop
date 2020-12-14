using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWorkshopLibrary
{
    public class DatabaseAccess
    {
        public static List<CustomerModel> loadCustomers()
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                var output = cnn.Query<CustomerModel>("select * from Customer", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void saveCustomer(CustomerModel customer)
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                cnn.Execute("insert into Customer (firstName, lastName, phoneNumber, adress) values (@firstName, @lastName, @phoneNumber, @adress)", customer);
            }
        }

        public static List<EmployeeModel> loadEmployees()
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                var output = cnn.Query<EmployeeModel>("select * from Employee", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void saveEmployee(EmployeeModel employee)
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                cnn.Execute("insert into Employee (firstName, lastName, specialization) values (@firstName, @lastName, @specialization)", employee);
            }
        }
        private static string loadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public static void deleteEmployee(EmployeeModel employee)
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                cnn.Execute("delete from Employee where firstName=@firstName and lastName=@lastName and specialization=@specialization", employee);
            }
        }
    }
}
