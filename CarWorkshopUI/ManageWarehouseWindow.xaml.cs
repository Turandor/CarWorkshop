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
        List<OrdersModel> orders = new List<OrdersModel>();
        public ManageWarehouseWindow()
        {
            InitializeComponent();

            loadWarehouseList();
            orders = DatabaseAccess.loadOrders();
        }

        private void loadWarehouseList()
        {
            warehouse = DatabaseAccess.loadWarehouse();

            wireUpWarehouseList();
        }
        private void wireUpWarehouseList()
        {
            listWarehouseListView.ItemsSource = null;
            listWarehouseListView.ItemsSource = warehouse;
        }

        private void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow objMainWindow = new MainWindow();
            objMainWindow.Show();
            this.Close();
        }

        private void addPartButton_Click(object sender, RoutedEventArgs e)
        {
            WarehouseModel selectedPart = new WarehouseModel();

            selectedPart.partName = partNameText.Text;
            selectedPart.producent = producentText.Text;
            selectedPart.price = double.Parse(priceText.Text);
            selectedPart.stockQuantity = int.Parse(stockQuantityText.Text);

            DatabaseAccess.saveWarehouse(selectedPart);

            partNameText.Text = null;
            producentText.Text = null;
            priceText.Text = null;
            stockQuantityText.Text = null;

            loadWarehouseList();
        }

        private void editPartButton_Click(object sender, RoutedEventArgs e)
        {
            WarehouseModel selectedPart = (dynamic)listWarehouseListView.SelectedItems[0];

            selectedPart.partName = partNameText.Text;
            selectedPart.producent = producentText.Text;
            selectedPart.price = double.Parse(priceText.Text);
            selectedPart.stockQuantity = int.Parse(stockQuantityText.Text);

            DatabaseAccess.updateWarehouse(selectedPart);

            loadWarehouseList();

            partNameText.Text = null;
            producentText.Text = null;
            priceText.Text = null;
            stockQuantityText.Text = null;
        }

        private void deletePartButton_Click(object sender, RoutedEventArgs e)
        {

            WarehouseModel selectedPart = (dynamic)listWarehouseListView.SelectedItems[0];
            List<OrdersModel> selectedOrders = orders.FindAll(x => x.idParts == selectedPart.idParts); 

            DatabaseAccess.deleteWarehouse(selectedPart);

            foreach (var item in selectedOrders)
            {
                DatabaseAccess.deleteOrder(item);
            }


            loadWarehouseList();
            orders = DatabaseAccess.loadOrders();

            partNameText.Text = null;
            producentText.Text = null;
            priceText.Text = null;
            stockQuantityText.Text = null;
        }

        private void listWarehouseListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                WarehouseModel selectedPart = (dynamic)listWarehouseListView.SelectedItems[0];

                partNameText.Text = selectedPart.partName;
                producentText.Text = selectedPart.producent;
                priceText.Text = selectedPart.price.ToString();
                stockQuantityText.Text = selectedPart.stockQuantity.ToString();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
    }

}
