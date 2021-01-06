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
        int startHour = 9;  // Godzina startu pracy
        int endHour = 17;   // Godzina konca pracy

        public ManageTasksWindow()
        {
            InitializeComponent();
            services = DatabaseAccess.loadServices();
            customers = DatabaseAccess.loadCustomers();
            cars = DatabaseAccess.loadCars();
            appointments = DatabaseAccess.loadAppointments();
            warehouse = DatabaseAccess.loadWarehouse();
            orders = DatabaseAccess.loadOrders();

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
            AppointmentModel employeeInAppointment;
            DateTime nearestDate = RoundUp(DateTime.UtcNow, TimeSpan.FromMinutes(15));
            


            string[] neededParts = neededPartsText.Text.Split(separator, StringSplitOptions.RemoveEmptyEntries);

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
                        DateTime orderDelay = pendingOrder.realizationDate;
                    }
                }
            } //all parts ready

            if(true) // checkbox czy chcesz najblizszy termin
            {
                var appointmentsTmp = appointments.FindAll(x => x.date.Date == nearestDate.Date && x.date.Date == nearestDate.AddDays(-1).Date);
                appointmentsTmp = appointmentsTmp.FindAll(x => x.date.TimeOfDay < nearestDate.TimeOfDay && x.date.AddHours(x.estimatedTime).TimeOfDay > nearestDate.TimeOfDay);
                dateTextBlock.Content = nearestDate;
                foreach (var item in appointments) // wolny pracownik i stanowisko
                {
                    employeeInAppointment = 
                }
            } 
            else // wybierz termin
            {

            }
        }

        DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }

        bool isWorkDay(DateTime dt)
        {
            if (dt.DayOfWeek == DayOfWeek.Sunday || dt.DayOfWeek == DayOfWeek.Saturday)
                return false;
            else
                return true;
        }

        bool isWorkHour(DateTime dt)
        {
            if (dt.Hour >= startHour && dt.Hour < endHour)
                return true;
            else
                return false;
        }

        DateTime changeDateToNextWorkDay(DateTime dt) //  DateTime + TimeSpan  <- moze byc blad
        {
            TimeSpan ts = new TimeSpan(startHour, 0, 0);
            if (isWorkDay(dt))
            {
                if (dt.DayOfWeek == DayOfWeek.Friday)
                {
                    return dt.AddDays(3) + ts;
                }
                else
                {
                    return dt.AddDays(1) + ts;
                }
            }
            else
            {
                if (dt.DayOfWeek == DayOfWeek.Saturday)
                {
                    return dt.AddDays(2) + ts;
                }
                else
                {
                    return dt.AddDays(1) + ts;
                }
            }
        }

    }
}
