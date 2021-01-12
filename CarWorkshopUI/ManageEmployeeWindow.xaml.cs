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
    public partial class ManageEmployeeWindow : Window
    {
        List<EmployeeModel> employee = new List<EmployeeModel>();
        List<AppointmentModel> appointments = new List<AppointmentModel>();

        public ManageEmployeeWindow()
        {
            InitializeComponent();

            loadEmployeesList();

        }

        private void loadEmployeesList()
        {
            employee = DatabaseAccess.loadEmployees();
            appointments = DatabaseAccess.loadAppointments();

            wireUpEmployeesList();
        }

        private void wireUpEmployeesList()
        {
            listEmployeesListView.ItemsSource = null;
            listEmployeesListView.ItemsSource = employee;
        }

        private void addEmployeeButton_Click(object sender, EventArgs e)
        {
            EmployeeModel employee = new EmployeeModel();

            employee.firstName = firstNameText.Text;
            employee.lastName = lastNameText.Text;
            employee.specialization = specializationText.Text;

            DatabaseAccess.saveEmployee(employee);

            firstNameText.Text = "";
            lastNameText.Text = "";
            specializationText.Text = "";

            loadEmployeesList();
        }

        private void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow objMainWindow = new MainWindow();
            objMainWindow.Show();
            this.Close();
        }

        private void deleteEmployee_Click(object sender, RoutedEventArgs e) 
        {
            EmployeeModel selectedEmployee = (dynamic)listEmployeesListView.SelectedItems[0];
            List<AppointmentModel> selectedAppointments = appointments.FindAll(x => x.idEmployee == selectedEmployee.idEmployee);

            DatabaseAccess.deleteEmployee(selectedEmployee);

            foreach (var item in selectedAppointments)
            {
                DatabaseAccess.deleteAppointment(item);
            }


            loadEmployeesList();
            appointments = DatabaseAccess.loadAppointments();

            firstNameText.Text = null;
            lastNameText.Text = null;
            specializationText.Text = null;
        }

        private void editEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeModel selectedEmployee = (dynamic)listEmployeesListView.SelectedItems[0];

            selectedEmployee.firstName = firstNameText.Text;
            selectedEmployee.lastName = lastNameText.Text;
            selectedEmployee.specialization = specializationText.Text;

            DatabaseAccess.updateEmployee(selectedEmployee);

            loadEmployeesList();

            firstNameText.Text = null;
            lastNameText.Text = null;
            specializationText.Text = null;
        }

        private void listEmployeesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                EmployeeModel selectedEmployee = (dynamic)listEmployeesListView.SelectedItems[0];

                firstNameText.Text = selectedEmployee.firstName;
                lastNameText.Text = selectedEmployee.lastName;
                specializationText.Text = selectedEmployee.specialization;
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
    }
}
