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

        private void manageEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            ManageEmployeeWindow objManageEmployeeWindow = new ManageEmployeeWindow();
            this.Visibility = Visibility.Hidden;
            objManageEmployeeWindow.Show();
        }
    }
}
