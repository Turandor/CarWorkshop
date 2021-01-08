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

        EmployeeModel chosenEmployee = new EmployeeModel();
        WorkplaceModel chosenWorkplace = new WorkplaceModel();
        DateTime chosenDataTime = new DateTime();

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
            CustomerModel customer = new CustomerModel();
            CarModel car = new CarModel();
            AppointmentModel appointment = new AppointmentModel();

            customer = customers.Find(x => x.firstName == firstNameText.Text &&
                                                x.lastName == lastNameText.Text &&
                                                x.phoneNumber == phoneText.Text);
            if (customer == null)
            {
                customer = new CustomerModel();
                customer.firstName = firstNameText.Text;
                customer.lastName = lastNameText.Text;
                customer.phoneNumber = phoneText.Text;
                customer.adress = adressText.Text;

                DatabaseAccess.saveCustomer(customer);
                customers = DatabaseAccess.loadCustomers();
                customer = customers.Find(x => x.firstName == customer.firstName &&
                                                    x.lastName == customer.lastName &&
                                                    x.phoneNumber == customer.phoneNumber);
            }

            car = cars.Find(x => x.registrationNumber == car.registrationNumber);
            if (car == null)
            {
                car = new CarModel();
                car.idCustomer = customer.idCustomer;
                car.brand = brandText.Text;
                car.model = modelText.Text;
                car.registrationNumber = registrationNumberText.Text;

                DatabaseAccess.saveCar(car);
                cars = DatabaseAccess.loadCars();
                car = cars.Find(x => x.registrationNumber == car.registrationNumber);
            }

            //make appointment
            appointment.idCar = car.idCar;
            appointment.idWorkplace = chosenWorkplace.idWorkplace;    
            appointment.idEmployee = chosenEmployee.idEmployee;
            appointment.date = chosenDataTime;
            appointment.appointmentType = appointmentTypeComboBox.SelectedItem.ToString();
            appointment.cost = double.Parse(priceTextBlock.Content.ToString());
            appointment.estimatedTime = double.Parse(estimatedTimeText.Text);
            appointment.neededParts = neededPartsText.Text;

            DatabaseAccess.saveAppointment(appointment);
            DatabaseAccess.loadAppointments();

            services = DatabaseAccess.loadServices();
            customers = DatabaseAccess.loadCustomers();
            cars = DatabaseAccess.loadCars();
            appointments = DatabaseAccess.loadAppointments();
            warehouse = DatabaseAccess.loadWarehouse();
            orders = DatabaseAccess.loadOrders();
            employees = DatabaseAccess.loadEmployees();
            workplaces = DatabaseAccess.loadWorkplace();
        }
        
        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            string[] separator = new string[]{ ", " };
            string[] neededParts = neededPartsText.Text.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            WarehouseModel part;
            OrdersModel pendingOrder;
            ServiceModel service = services.Find(x => x.serviceName == appointmentTypeComboBox.Text);

            DateTime nearestDate = AppointmentModel.RoundUp(DateTime.UtcNow, TimeSpan.FromMinutes(15));
            bool nearestDateFound = false;
            List<WarehouseModel> confirmedNeededParts = new List<WarehouseModel>();
            List<AppointmentModel> collidingAppointments = new List<AppointmentModel>();
            List<EmployeeModel> availableEmployees = new List<EmployeeModel>();
            List<WorkplaceModel> availableWorkplaces = new List<WorkplaceModel>();

            //reset employee and workplace
            chosenEmployee = new EmployeeModel();
            chosenWorkplace = new WorkplaceModel();

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
                        if (nearestDate > pendingOrder.realizationDate)
                            nearestDate = AppointmentModel.RoundUp(pendingOrder.realizationDate, TimeSpan.FromMinutes(15)); // If parts are ordered take new available nearestDate
                    }
                }
                confirmedNeededParts.Add(part);
            } //all parts ready

            if(!nearestDateCheckBox.IsChecked.Value)
            {
                if (AppointmentModel.isWorkDay(chosenDatePicker.SelectedDate.Value))
                {
                    if (nearestDate.Date < chosenDatePicker.SelectedDate)
                    {
                        nearestDate = chosenDatePicker.SelectedDate.Value;
                        nearestDate = new DateTime(nearestDate.Year, nearestDate.Month, nearestDate.Day, AppointmentModel.startHour, 0, 0);
                    }
                    else if (nearestDate.Date > chosenDatePicker.SelectedDate)
                    {
                        MessageBox.Show("Brak części lub zły termin");
                        return;
                    }
                    // if equal take nearestDate
                }
                else
                {
                    MessageBox.Show("Warsztat nieczynny w weekendy");
                    return;
                }

            }

            while(!nearestDateFound) //found neares available date
            {
                collidingAppointments = appointments.FindAll(x => (x.date < nearestDate && x.GetFinishDate() > nearestDate) ||
                                                                        (x.date > nearestDate && x.GetFinishDate() < AppointmentModel.GetFinishDate(nearestDate, double.Parse(estimatedTimeText.Text))) ||
                                                                        (x.date > nearestDate && x.date < AppointmentModel.GetFinishDate(nearestDate, double.Parse(estimatedTimeText.Text)) && x.GetFinishDate() > AppointmentModel.GetFinishDate(nearestDate, double.Parse(estimatedTimeText.Text))));
                if (collidingAppointments.Count != 0)
                {
                    availableEmployees = new List<EmployeeModel>(employees);
                    availableWorkplaces = new List<WorkplaceModel>(workplaces);
                    foreach (var item in collidingAppointments) // wolny pracownik i stanowisko
                    {
                        availableEmployees.Remove(availableEmployees.Find(x => x.idEmployee == item.idEmployee));
                        availableWorkplaces.Remove(availableWorkplaces.Find(x => x.idWorkplace == item.idWorkplace));
                    }

                    if (service.serviceName == "ogólny")
                    {
                        chosenEmployee = availableEmployees[0];
                        chosenWorkplace = availableWorkplaces.Find(x => x.workplaceName == service.serviceCategory);
                    }
                    else
                    {
                        chosenEmployee = availableEmployees.Find(x => x.specialization == service.serviceCategory);
                        chosenWorkplace = availableWorkplaces.Find(x => x.workplaceName == service.serviceCategory);
                    }

                    if (chosenWorkplace == default || chosenEmployee == default)  //sprawidzić działanie default
                    {
                        nearestDate.AddMinutes(15);
                        if (!AppointmentModel.isWorkHour(nearestDate))
                        {
                            if (!nearestDateCheckBox.IsChecked.Value)
                            {
                                MessageBox.Show("Brak wolnych godzin w wybranym dniu");
                                return;
                            }
                            else 
                                AppointmentModel.changeDateToNextWorkDay(nearestDate);
                        }
                        //następna iteracja
                    }
                    else
                    {
                        dateTextBlock.Content = nearestDate;
                        nearestDateFound = true;
                    }
                }
                else
                {
                    dateTextBlock.Content = nearestDate + " - " + AppointmentModel.GetFinishDate(nearestDate,double.Parse(estimatedTimeText.Text)); //weź nearestDate
                    chosenEmployee = employees.Find(x => x.specialization == service.serviceCategory);
                    chosenWorkplace = workplaces.Find(x => x.workplaceName == service.serviceCategory);
                    nearestDateFound = true;
                }
            }
            //sprawdzić czy któraś ścieżka nie przypisuje
            employeeTextBlock.Content = chosenEmployee.firstName + " " + chosenEmployee.lastName;
            workplaceTextBlock.Content = chosenWorkplace.workplaceName + " stanowisko: " + chosenWorkplace.idWorkplace;
            //save chosen date
            chosenDataTime = nearestDate;

            //calculate price
            double price = 0;
            foreach (var item in confirmedNeededParts)
            {
                price += item.price;
            }
            price += service.price * double.Parse(estimatedTimeText.Text);
            priceTextBlock.Content = price;

        }

        private void nearestDateCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            chosenDatePicker.Visibility = Visibility.Hidden;
            chosenDatePicker.SelectedDate = null;
        }

        private void nearestDateCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            chosenDatePicker.Visibility = Visibility.Visible;
        }
    }
}
