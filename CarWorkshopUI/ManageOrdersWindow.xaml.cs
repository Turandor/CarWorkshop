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
    /// <summary>
    /// Logika interakcji dla klasy ManageOrdersWindow.xaml
    /// </summary>
    public partial class ManageOrdersWindow : Window
    {
        List<OrdersModel> orders = new List<OrdersModel>();
        List<WarehouseModel> warehouse = new List<WarehouseModel>();
        WarehouseModel selectedPart;

        public ManageOrdersWindow()
        {
            InitializeComponent();

            warehouse = DatabaseAccess.loadWarehouse();
            loadOrdersList();
        }

        private void loadOrdersList()
        {
            orders = DatabaseAccess.loadOrders();
            wireUpOrdersList();
        }
        private void wireUpOrdersList()
        {
            listOrdersListView.ItemsSource = null;
            listOrdersListView.ItemsSource = orders;
        }

        private void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow objMainWindow = new MainWindow();
            objMainWindow.Show();
            this.Close();
        }

        private void addOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            OrdersModel orders = new OrdersModel();

            orders.idParts = int.Parse(idPartsText.Text);
            orders.amount = int.Parse(amountText.Text);  
            orders.orderDate = RoundUp(DateTime.Now,TimeSpan.FromMinutes(1));
            orders.realizationDate = orders.orderDate.AddDays(int.Parse(deliveryTimeText.Text));
            orders.status = "w realizacji";
            orders.bookedAmount = 0;
            

            DatabaseAccess.saveOrders(orders);

            idPartsText.Text = "";
            partNameText.Text = "";
            amountText.Text = "";
            deliveryTimeText.Text = "";

            loadOrdersList();
        }

        private void confirmOrderButton_Click(object sender, RoutedEventArgs e)
        {
            OrdersModel order = new OrdersModel();

            var selectedOrder = (dynamic)listOrdersListView.SelectedItems[0];

            order.idOrder = selectedOrder.idOrder;
            order.idParts = selectedOrder.idParts;
            order.amount = selectedOrder.amount;
            order.orderDate = selectedOrder.orderDate;
            order.status = selectedOrder.status;
            order.realizationDate = selectedOrder.realizationDate;
            order.bookedAmount = selectedOrder.bookedAmount;

            var warehouse = DatabaseAccess.loadWarehouse();
            WarehouseModel part = new WarehouseModel();

            for (int i = 0; i < warehouse.Count; i++)
            {
                if (warehouse[i].idParts == order.idParts)
                {
                    part = warehouse[i];
                    break;
                }
            }

            DatabaseAccess.updateOrders(order, part); //change status - finished
            loadOrdersList();
        }
        public static DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }

        private void partNameText_TextChanged(object sender, TextChangedEventArgs e)
        {
            selectedPart = warehouse.Find(x => x.partName == partNameText.Text);
            if (selectedPart != null)
                idPartsText.Text = selectedPart.idParts.ToString();
        }

        private void idPartsText_TextChanged(object sender, TextChangedEventArgs e)
        {
            int partID;
            if (int.TryParse(idPartsText.Text, out partID))
                selectedPart = warehouse.Find(x => x.idParts == int.Parse(idPartsText.Text));
            if (selectedPart != null)
                partNameText.Text = selectedPart.partName;
        }
    }
}
