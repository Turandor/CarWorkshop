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
    /// Logika interakcji dla klasy ManageCustomersWindow.xaml
    /// </summary>
    public partial class ManageCustomersWindow : Window
    {
        List<CustomerModel> customers = new List<CustomerModel>();
        List<CarModel> cars = new List<CarModel>();
        List<AppointmentModel> appointments = new List<AppointmentModel>();

        public ManageCustomersWindow()
        {
            InitializeComponent();

            cars = DatabaseAccess.loadCars();
            appointments = DatabaseAccess.loadAppointments();
            loadCustomersList();
        }

        private void loadCustomersList()
        {
            customers = DatabaseAccess.loadCustomers();
            wireUpCustomersList();
        }
        private void wireUpCustomersList()
        {
            listCustomersListView.ItemsSource = null;
            listCustomersListView.ItemsSource = customers;
        }

        private void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow objMainWindow = new MainWindow();
            objMainWindow.Show();
            this.Close();
        }

        private void addCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerModel customer = new CustomerModel();

            customer.firstName = firstNameText.Text;
            customer.lastName = lastNameText.Text;
            customer.phoneNumber = phoneNumberText.Text;
            customer.adress = adressText.Text;

            DatabaseAccess.saveCustomer(customer);

            firstNameText.Text = null;
            lastNameText.Text = null;
            phoneNumberText.Text = null;
            adressText.Text = null;

            loadCustomersList();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerModel selectedCustomer = (dynamic)listCustomersListView.SelectedItems[0];

            selectedCustomer.firstName = firstNameText.Text;
            selectedCustomer.lastName = lastNameText.Text;
            selectedCustomer.phoneNumber = phoneNumberText.Text;
            selectedCustomer.adress = adressText.Text;

            DatabaseAccess.updateCustomer(selectedCustomer);

            loadCustomersList();

            firstNameText.Text = null;
            lastNameText.Text = null;
            phoneNumberText.Text = null;
            adressText.Text = null;
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerModel selectedCustomer = (dynamic)listCustomersListView.SelectedItems[0];
            List<CarModel> selectedCars = cars.FindAll(x => x.idCustomer == selectedCustomer.idCustomer);

            DatabaseAccess.deleteCustomer(selectedCustomer);

            foreach (var item in selectedCars)
            {
                List<AppointmentModel> selectedAppointments = appointments.FindAll(x => x.idCar == item.idCar);
                foreach (var app in selectedAppointments)
                {
                    DatabaseAccess.deleteAppointment(app);
                }
                DatabaseAccess.deleteCar(item);
            }

            firstNameText.Text = null;
            lastNameText.Text = null;
            phoneNumberText.Text = null;
            adressText.Text = null;

            loadCustomersList();
            appointments = DatabaseAccess.loadAppointments();
            cars = DatabaseAccess.loadCars();
        }

        private void listCustomersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CustomerModel selectedCustomer = (dynamic)listCustomersListView.SelectedItems[0];

                firstNameText.Text = selectedCustomer.firstName;
                lastNameText.Text = selectedCustomer.lastName;
                phoneNumberText.Text = selectedCustomer.phoneNumber ;
                adressText.Text = selectedCustomer.adress;
            }
            catch (ArgumentOutOfRangeException)
            {
            }

        }
    }
}
