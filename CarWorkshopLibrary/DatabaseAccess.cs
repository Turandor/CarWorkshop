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
                cnn.Execute("delete from Employee where idEmployee=@idEmployee and firstName=@firstName and lastName=@lastName and specialization=@specialization", employee);
            }
        }

        public static List<WarehouseModel> loadWarehouse()
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                var output = cnn.Query<WarehouseModel>("select * from Warehouse", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void saveWarehouse(WarehouseModel warehouse)
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                cnn.Execute("insert into Warehouse (partName, producent, price, stockQuantity, deliveryTime) values (@partName, @producent, @price, @stockQuantity, @deliveryTime)", warehouse);
            }
        }

        public static void updateWarehouse(WarehouseModel warehouse)
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                cnn.Execute("update Warehouse set stockQuantity = @stockQuantity where idParts = @idParts", warehouse);
            }
        }
    }
}
