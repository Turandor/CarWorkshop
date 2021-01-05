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
        public ManageEmployeeWindow()
        {
            InitializeComponent();

            loadEmployeesList();
        }

        private void loadEmployeesList()
        {
            employee = DatabaseAccess.loadEmployees();

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
            EmployeeModel employee = new EmployeeModel();

            var selectedEmployee = (dynamic)listEmployeesListView.SelectedItems[0];

            employee.idEmployee = selectedEmployee.idEmployee;
            employee.firstName = selectedEmployee.firstName;
            employee.lastName = selectedEmployee.lastName;
            employee.specialization = selectedEmployee.specialization;


            DatabaseAccess.deleteEmployee(employee);

            loadEmployeesList();
        }
    }
}
