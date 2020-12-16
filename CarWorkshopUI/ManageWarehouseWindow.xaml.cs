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
    public partial class ManageWarehouseWindow : Window
    {
        List<WarehouseModel> warehouse = new List<WarehouseModel>();
        public ManageWarehouseWindow()
        {
            InitializeComponent();

            loadWarehouseList();
        }

        private void loadWarehouseList()
        {
            warehouse = DatabaseAccess.loadWarehouse();

            wireUpWarehouseList();
        }
        private void wireUpWarehouseList()
        {
            listWarehouseListBox.ItemsSource = null;
            listWarehouseListBox.ItemsSource = warehouse;
            listWarehouseListBox.DisplayMemberPath = "fullInfromation";
        }

        private void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow objMainWindow = new MainWindow();
            this.Visibility = Visibility.Hidden;
            objMainWindow.Show();
        }

        private void refreshListButton_Click(object sender, RoutedEventArgs e)
        {
            loadWarehouseList();
        }

        private void addWarehouseButton_Click(object sender, RoutedEventArgs e)
        {
            WarehouseModel warehouse = new WarehouseModel();

            warehouse.partName = partNameText.Text;
            warehouse.producent = producentText.Text;
            warehouse.price = priceText.Text;
            warehouse.stockQuantity = stockQuantityText.Text;
            warehouse.deliveryTime = deliveryTimeText.Text;

            DatabaseAccess.saveWarehouse(warehouse);

            partNameText.Text = "";
            producentText.Text = "";
            priceText.Text = "";
            stockQuantityText.Text = "";
            deliveryTimeText.Text = "";
        }

        /*****************************************************************************************************/
        private void addOnePartToWarehouseButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
