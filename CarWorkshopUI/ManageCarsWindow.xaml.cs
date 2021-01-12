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
    /// Logika interakcji dla klasy ManageCarsWindow.xaml
    /// </summary>
    public partial class ManageCarsWindow : Window
    {
        List<CarModel> cars = new List<CarModel>();
        List<CustomerModel> customers = new List<CustomerModel>();
        List<AppointmentModel> appointments = new List<AppointmentModel>();

        public ManageCarsWindow()
        {
            InitializeComponent();

            customers = DatabaseAccess.loadCustomers();
            appointments = DatabaseAccess.loadAppointments();
            loadCarsList();
        }

        private void loadCarsList()
        {
            cars = DatabaseAccess.loadCars();

            wireUpCarsList();
        }
        private void wireUpCarsList()
        {
            listCarsListView.ItemsSource = null;
            listCarsListView.ItemsSource = cars;
        }

        private void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow objMainWindow = new MainWindow();
            objMainWindow.Show();
            this.Close();
        }

        private void editCarButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCar = (dynamic)listCarsListView.SelectedItems[0];

            selectedCar.brand = brandText.Text;
            selectedCar.model = modelText.Text;
            selectedCar.registrationNumber = registrationNumberText.Text;

            DatabaseAccess.updateCar(selectedCar);

            loadCarsList();

            brandText.Text = null;
            modelText.Text = null;
            registrationNumberText.Text = null;
        }

        private void deleteCar_Click(object sender, RoutedEventArgs e)
        {
            CarModel selectedCar = (dynamic)listCarsListView.SelectedItems[0];
            List<AppointmentModel> selectedAppointments = appointments.FindAll(x => x.idCar == selectedCar.idCar);

            DatabaseAccess.deleteCar(selectedCar);

            foreach (var item in selectedAppointments)
            {
                DatabaseAccess.deleteAppointment(item);
            }
            

            brandText.Text = null;
            modelText.Text = null;
            registrationNumberText.Text = null;

            loadCarsList();
            appointments = DatabaseAccess.loadAppointments();
        }

        private void listCarsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CarModel selectedCar = (dynamic)listCarsListView.SelectedItems[0];
                CustomerModel selectedCustomer = customers.Find(x => x.idCustomer == selectedCar.idCustomer);

                brandText.Text = selectedCar.brand;
                modelText.Text = selectedCar.model;
                registrationNumberText.Text = selectedCar.registrationNumber;
                nameText.Content = selectedCustomer.firstName + " " + selectedCustomer.lastName;
                phoneNumberText.Content = selectedCustomer.phoneNumber;
                adressText.Content = selectedCustomer.adress;
            }
            catch (ArgumentOutOfRangeException)
            {
            }

        }
    }
}
