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
    /// Logika interakcji dla klasy BrowseCalendarWindow.xaml
    /// </summary>
    public partial class BrowseCalendarWindow : Window
    {
        List<AppointmentModel> dailyAppointments = new List<AppointmentModel>();
        List<CustomerModel> customers = new List<CustomerModel>();
        List<CarModel> cars = new List<CarModel>();
        List<EmployeeModel> employees = new List<EmployeeModel>();
        AppointmentModel selectedAppointment = new AppointmentModel();
        CustomerModel selectedCustomer = new CustomerModel();
        CarModel selectedCar = new CarModel();
        EmployeeModel selectedEmployee = new EmployeeModel();

        public BrowseCalendarWindow()
        {
            InitializeComponent();
            loadAppointmentsList();
            customers = DatabaseAccess.loadCustomers();
            cars = DatabaseAccess.loadCars();
            employees = DatabaseAccess.loadEmployees();
        }

        private void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow objMainWindow = new MainWindow();
            objMainWindow.Show();
            this.Close();
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            loadAppointmentsList();
        }

        private void loadAppointmentsList()
        {
            dailyAppointments = DatabaseAccess.loadAppointments();
            wireUpAppointmentsList();
        }
        private void wireUpAppointmentsList()
        {
            listOrdersListView.ItemsSource = null;
            if (calendar.SelectedDate == null)
                listOrdersListView.ItemsSource = dailyAppointments.FindAll(x => x.date.Date == DateTime.Now.Date);
            else
                listOrdersListView.ItemsSource = dailyAppointments.FindAll(x => x.date.Date == calendar.SelectedDate);
        }

        private void listOrdersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selectedAppointment = (dynamic)listOrdersListView.SelectedItems[0];
                selectedCar = cars.Find(x => x.idCar == selectedAppointment.idCar);
                selectedCustomer = customers.Find(x => x.idCustomer == selectedCar.idCustomer);
                selectedEmployee = employees.Find(x => x.idEmployee == selectedAppointment.idEmployee);

                finishDateText.Content = AppointmentModel.GetFinishDate(selectedAppointment.date, selectedAppointment.estimatedTime);
                employeeNameText.Content = selectedEmployee.firstName + " " + selectedEmployee.lastName;
                customerNameText.Content = selectedCustomer.firstName + " " + selectedCustomer.lastName;
                phoneNumberText.Content = selectedCustomer.phoneNumber;
                modelText.Content = selectedCar.model;
                brandText.Content = selectedCar.brand;
                registrationNumberText.Content = selectedCar.registrationNumber;
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
    }
}
