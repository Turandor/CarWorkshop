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
        public ManageTasksWindow()
        {
            InitializeComponent();
            services = DatabaseAccess.loadServices();
            customers = DatabaseAccess.loadCustomers();
            cars = DatabaseAccess.loadCars();
            appointments = DatabaseAccess.loadAppointments();

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
            
            custormer = customers.Find(x => x.firstName == custormer.firstName &&
                                                x.lastName == custormer.lastName &&
                                                x.phoneNumber == custormer.phoneNumber);
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
            //TO DO
            //calculate date, employee and workplace
            DateTime foundDate;
            if (appointments.Count == 0)
            {
                foundDate = DateTime.UtcNow;
            }
            else
            {
                foreach (var item in appointments)
                {
                    if (true) //if employee is free
                    {
                        if (true) //if workplace is free
                        {
                            foundDate = DateTime.UtcNow;
                            if (foundDate.CompareTo(item.date.AddHours(item.estimatedTime)) >= 0) //make workday eg 6:00 - 18:00
                                dateTextBlock.Content = foundDate;
                            else
                            {
                                
                            }
                        }
                    }
                }
            }

            //calculate price
        }
    }
}
