using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CarWorkshopLibrary;

namespace CarWorkshopUI
{
    /// <summary>
    /// Logika interakcji dla klasy ManageTasksWindow.xaml
    /// </summary>
    public partial class ManageTasksWindow : Window
    {
        List<CustomerModel> customers = new List<CustomerModel>();
        List<CarModel> cars = new List<CarModel>();
        List<ServiceModel> services = new List<ServiceModel>();
        List<AppointmentModel> appointments = new List<AppointmentModel>();
        List<WarehouseModel> warehouse = new List<WarehouseModel>();
        List<OrdersModel> orders = new List<OrdersModel>();
        List<EmployeeModel> employees = new List<EmployeeModel>();
        List<WorkplaceModel> workplaces = new List<WorkplaceModel>();
        public ManageTasksWindow()
        {
            InitializeComponent();
            services = DatabaseAccess.loadServices();
            customers = DatabaseAccess.loadCustomers();
            cars = DatabaseAccess.loadCars();
            appointments = DatabaseAccess.loadAppointments();
            warehouse = DatabaseAccess.loadWarehouse();
            orders = DatabaseAccess.loadOrders();
            employees = DatabaseAccess.loadEmployees();
            workplaces = DatabaseAccess.loadWorkplace();

            AppointmentModel cos = new AppointmentModel();
            cos.date = DateTime.UtcNow;
            cos.estimatedTime = 101;
            MessageBox.Show(cos.date + "\n" + cos.GetFinishDate());

            foreach (var item in services)
            {
                appointmentTypeComboBox.Items.Add(item.serviceName);
            }
        }

        private void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow objMainWindow = new MainWindow();
            objMainWindow.Show();
            this.Close();
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerModel custormer = new CustomerModel();
            CarModel car = new CarModel();
            AppointmentModel appointment = new AppointmentModel();
            
            custormer = customers.Find(x => x.firstName == firstNameText.Text &&
                                                x.lastName == lastNameText.Text &&
                                                x.phoneNumber == phoneText.Text);
            if (custormer == null)
            {
                custormer.firstName = firstNameText.Text;
                custormer.lastName = lastNameText.Text;
                custormer.phoneNumber = phoneText.Text;
                custormer.adress = adressText.Text;

                DatabaseAccess.saveCustomer(custormer);
                customers = DatabaseAccess.loadCustomers();
                custormer = customers.Find(x => x.firstName == custormer.firstName &&
                                                    x.lastName == custormer.lastName &&
                                                    x.phoneNumber == custormer.phoneNumber);
            }

            var carsList = DatabaseAccess.loadCars(); //loading cars 
            car = carsList.Find(x => x.registrationNumber == car.registrationNumber);
            if (car == null)
            {
                car.idCustomer = custormer.idCustomer;
                car.brand = brandText.Text;
                car.model = modelText.Text;
                car.registrationNumber = registrationNumberText.Text;

                DatabaseAccess.saveCar(car);
                carsList = DatabaseAccess.loadCars();
                car = carsList.Find(x => x.registrationNumber == car.registrationNumber);
            }

            //make appointment
            appointment.idCar = car.idCar;
            appointment.idWorkplace = int.Parse(workplaceTextBlock.Content.ToString());
            appointment.idEmployee = int.Parse(employeeTextBlock.Content.ToString());
            appointment.date = DateTime.Parse(dateTextBlock.Content.ToString());
            appointment.appointmentType = appointmentTypeComboBox.SelectedItem.ToString();
            appointment.cost = int.Parse(priceTextBlock.Content.ToString());
            appointment.estimatedTime = double.Parse(estimatedTimeText.Text);
            appointment.neededParts = neededPartsText.Text;

            DatabaseAccess.saveAppointment(appointment);
            DatabaseAccess.loadAppointments();
        }
        
        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            string[] separator = new string[]{ ", " };
            WarehouseModel part;
            OrdersModel pendingOrder;
            DateTime nearestDate = AppointmentModel.RoundUp(DateTime.UtcNow, TimeSpan.FromMinutes(15));
            EmployeeModel choosenEmployee;
            WorkplaceModel choosenWorkplace;
            ServiceModel service = services.Find(x => x.serviceName == appointmentTypeComboBox.Text);
            string[] neededParts = neededPartsText.Text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            List<WarehouseModel> confirmedNeededParts = new List<WarehouseModel>();
            foreach (var item in neededParts)
            {
                part = warehouse.Find(x => x.partName == item); //by names 
                if (part == default) // jak co to sprawdzić default // zrobić wielokrotność potrzebnych części 1 rodzaju
                {
                    MessageBox.Show("Część: " + item + " nie jest znana");
                    break;
                }
                else if (part.stockQuantity == 0)
                {
                    pendingOrder = orders.Find(x => x.idParts == part.idParts && x.status != "zrealizowane");  //do zastanowienia: więcej wizyt niż zamówień na te same częśći (3 silniki 4 wymiany)
                    if (pendingOrder == default)
                    {
                        MessageBox.Show("Brak części: " + item + " w magazynie"); //rozwinąć ew (przechodzenie do zamówienia części
                        break;
                    }
                    else
                    {
                        //TimeSpan orderDelay = pendingOrder.realizationDate - DateTime.UtcNow;
                        DateTime orderDelay = pendingOrder.realizationDate; // DODAĆ POTEM WAŻNE DO NEAREST DATE albo co..
                    }
                }
                confirmedNeededParts.Add(part);
            } //all parts ready

            if(true) // checkbox czy chcesz najblizszy termin
            {
                var collidingAppointments = appointments.FindAll(x => (x.date < nearestDate && x.GetFinishDate() > nearestDate) ||
                                                                      (x.date > nearestDate && x.GetFinishDate() < AppointmentModel.GetFinishDate(nearestDate, double.Parse(estimatedTimeText.Text))) ||
                                                                      (x.date > nearestDate && x.date < AppointmentModel.GetFinishDate(nearestDate, double.Parse(estimatedTimeText.Text)) && x.GetFinishDate() > AppointmentModel.GetFinishDate(nearestDate, double.Parse(estimatedTimeText.Text))));
                
                if (collidingAppointments.Count != 0)
                {
                    List<EmployeeModel> availableEmployees = new List<EmployeeModel>(employees);
                    List<WorkplaceModel> availableWorkplaces = new List<WorkplaceModel>(workplaces);
                    foreach (var item in collidingAppointments) // wolny pracownik i stanowisko
                    {
                        availableEmployees.Remove(availableEmployees.Find(x => x.idEmployee == item.idEmployee));
                        availableWorkplaces.Remove(availableWorkplaces.Find(x => x.idWorkplace == item.idWorkplace));
                    }

                    if(service.serviceName == "ogólny")
                    {
                        choosenEmployee = availableEmployees[0];
                        choosenWorkplace = availableWorkplaces.Find(x => x.workplaceName == service.serviceName);
                    }
                    else
                    {
                        choosenEmployee = availableEmployees.Find(x => x.specialization == service.serviceName);
                        choosenWorkplace = availableWorkplaces.Find(x => x.workplaceName == service.serviceName);
                    }

                    if (choosenWorkplace == default || choosenEmployee == default)  //sprawidzić działanie default
                    {
                        nearestDate.AddMinutes(15);
                        if (!AppointmentModel.isWorkHour(nearestDate))
                            AppointmentModel.changeDateToNextWorkDay(nearestDate);
                        //następna iteracja
                    }
                    else
                    {
                        dateTextBlock.Content = nearestDate;
                    }
                }
                else
                {
                    dateTextBlock.Content = nearestDate; //weź nearestDate
                    choosenEmployee = employees.Find(x => x.specialization == service.serviceName);
                    choosenWorkplace = workplaces.Find(x => x.workplaceName == service.serviceName);
                }

                employeeTextBlock.Content = choosenEmployee.firstName + choosenEmployee.lastName;
                workplaceTextBlock.Content = choosenWorkplace.workplaceName + "stanowisko: " + choosenWorkplace.idWorkplace;
            } 
            else // wybierz termin
            {

            }

            //calculate price
            double price = 0;
            foreach (var item in confirmedNeededParts)
            {
                price += item.price;
            }
            price += service.price * double.Parse(estimatedTimeText.Text);
            priceTextBlock.Content = price;
        }
    }
}
