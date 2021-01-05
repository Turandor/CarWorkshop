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
        private static string loadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }


        /***************************** Customer *********************************/
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

        /************************** Employee *********************************/

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
        public static void deleteEmployee(EmployeeModel employee)
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                cnn.Execute("delete from Employee where idEmployee=@idEmployee and firstName=@firstName and lastName=@lastName and specialization=@specialization", employee);
            }
        }
        /********************** Warehouse ***********************************/
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
                cnn.Execute("insert into Warehouse (partName, producent, price, stockQuantity) values (@partName, @producent, @price, @stockQuantity)", warehouse);
            }
        }

        public static void updateWarehouse(WarehouseModel warehouse)
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                cnn.Execute("update Warehouse set stockQuantity = @stockQuantity where idParts = @idParts", warehouse);
            }
        }

        /************************* Orders ***********************************/
        public static List<OrdersModel> loadOrders()
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                var output = cnn.Query<OrdersModel>("select * from Orders", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void saveOrders(OrdersModel orders)
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                cnn.Execute("insert into Orders (idParts, amount, orderDate, status, realizationDate) values (@idParts, @amount, @orderDate, @status, @realizationDate)", orders);
            }
        }

        public static void updateOrders(OrdersModel orders, WarehouseModel warehouse)
        {
            if(orders.status != "zrealizowane")
            {
                using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
                {
                    cnn.Execute("update Orders set status = 'zrealizowane' where idOrder = @idOrder", orders);
                }
                using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
                {
                    cnn.Execute("update Warehouse set stockQuantity = stockQuantity + @amount where idParts = @idParts", orders);
                }
            }
        }

        /********************************* Cars ***********************************/
        public static List<CarModel> loadCars()
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                var output = cnn.Query<CarModel>("select * from Car", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void saveCar(CarModel car)
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                cnn.Execute("insert into Car (idCustomer, brand, model, registrationNumber) values (@idCustomer, @brand, @model, @registrationNumber)", car);
            }
        }

        /*********************************** Services **********************************/
        public static List<ServiceModel> loadServices()
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                var output = cnn.Query<ServiceModel>("select * from Service", new DynamicParameters());
                return output.ToList();
            }
        }

        /********************************** Appointments ***********************/
        public static List<AppointmentModel> loadAppointments()
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                var output = cnn.Query<AppointmentModel>("select * from Appointment", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void saveAppointment(AppointmentModel appointment)
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                cnn.Execute("insert into Appointment (idCar, idWorkplace, idEmployee, date, appointmentType, cost, estimatedTime, neededParts)" +
                    " values (@idCar, @idWorkplace, @idEmployee, @date, @appointmentType, @cost, @estimatedTime, @neededParts)", appointment);
            }
        }
    }
}
