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
            listEmployeesListBox.ItemsSource = null;
            listEmployeesListBox.ItemsSource = employee;
            listEmployeesListBox.DisplayMemberPath = "fullInfromation";
        }

        private void refreshListButton_Click(object sender, EventArgs e)
        {
            loadEmployeesList();
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
        }

        private void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow objMainWindow = new MainWindow();
            this.Visibility = Visibility.Hidden;
            objMainWindow.Show();
        }


        /***********************************************************************************/ //TO DO
        private void deleteEmployee_Click(object sender, RoutedEventArgs e) 
        {
            EmployeeModel employee = new EmployeeModel();

            employee.firstName = firstNameText.Text;
            employee.lastName = lastNameText.Text;
            employee.specialization = specializationText.Text;

            DatabaseAccess.deleteEmployee(employee);

            firstNameText.Text = "";
            lastNameText.Text = "";
            specializationText.Text = "";
        }
    }
}
