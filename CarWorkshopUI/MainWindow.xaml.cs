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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CarWorkshopLibrary;

namespace CarWorkshopUI
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        // Change those buttons or back buttons bc they always create new windows without closing them later
        private void manageEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            ManageEmployeeWindow objManageEmployeeWindow = new ManageEmployeeWindow();
            this.Visibility = Visibility.Hidden;
            objManageEmployeeWindow.Show();
        }

        private void manageWarehouseButton_Click(object sender, RoutedEventArgs e)
        {
            ManageWarehouseWindow objManageWarehouseWindow = new ManageWarehouseWindow();
            this.Visibility = Visibility.Hidden;
            objManageWarehouseWindow.Show();
        }

        private void manageOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            ManageOrdersWindow objManageOrdersWindow = new ManageOrdersWindow();
            this.Visibility = Visibility.Hidden;
            objManageOrdersWindow.Show();
        }

        private void addTaskButton_Click(object sender, RoutedEventArgs e)
        {
            ManageTasksWindow objManageTasksWindow = new ManageTasksWindow();
            this.Visibility = Visibility.Hidden;
            objManageTasksWindow.Show();
        }

        private void browseCalendarButton_Click(object sender, RoutedEventArgs e)
        {
            BrowseCalendarWindow objBrowseCalendarWindow = new BrowseCalendarWindow();
            this.Visibility = Visibility.Hidden;
            objBrowseCalendarWindow.Show();
        }

        private void manageCarsButton_Click(object sender, RoutedEventArgs e)
        {
            ManageCarsWindow objManageCarsWindow = new ManageCarsWindow();
            this.Visibility = Visibility.Hidden;
            objManageCarsWindow.Show();
        }

        private void manageCustomersButton_Click(object sender, RoutedEventArgs e)
        {
            ManageCustomersWindow objManageCustomersWindow = new ManageCustomersWindow();
            this.Visibility = Visibility.Hidden;
            objManageCustomersWindow.Show();
        }
    }
}
