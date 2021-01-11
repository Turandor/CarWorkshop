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

        public BrowseCalendarWindow()
        {
            InitializeComponent();
            loadAppointmentsList();
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
    }
}
