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
using System.Windows.Forms;

namespace CarWorkshopUI
{
    /// <summary>
    /// Logika interakcji dla klasy ManageTasksWindow.xaml
    /// </summary>
    public partial class ManageTasksWindow : Window
    {
        List<CustomerModel> customers = new List<CustomerModel>();
        List<CarModel> cars = new List<CarModel>();
        List<ServiceModel> services = new List<ServiceModel>();
        List<AppointmentModel> appointments = new List<AppointmentModel>();
        List<WarehouseModel> warehouse = new List<WarehouseModel>();
        List<OrdersModel> orders = new List<OrdersModel>();
        List<EmployeeModel> employees = new List<EmployeeModel>();
        List<WorkplaceModel> workplaces = new List<WorkplaceModel>();

        EmployeeModel chosenEmployee = new EmployeeModel();
        WorkplaceModel chosenWorkplace = new WorkplaceModel();
        DateTime chosenDateTime = new DateTime();
        List<PartObject> chosenPartsList = new List<PartObject>();

        public ManageTasksWindow()
        {
            InitializeComponent();
            services = DatabaseAccess.loadServices();
            customers = DatabaseAccess.loadCustomers();
            cars = DatabaseAccess.loadCars();
            appointments = DatabaseAccess.loadAppointments();
            warehouse = DatabaseAccess.loadWarehouse();
            orders = DatabaseAccess.loadOrders();
            employees = DatabaseAccess.loadEmployees();
            workplaces = DatabaseAccess.loadWorkplace();

            foreach (var item in services)
            {
                appointmentTypeComboBox.Items.Add(item.serviceName);
            }
        }

        private void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow objMainWindow = new MainWindow();
            objMainWindow.Show();
            this.Close();
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            services = DatabaseAccess.loadServices();
            customers = DatabaseAccess.loadCustomers();
            cars = DatabaseAccess.loadCars();
            appointments = DatabaseAccess.loadAppointments();
            warehouse = DatabaseAccess.loadWarehouse();
            orders = DatabaseAccess.loadOrders();
            employees = DatabaseAccess.loadEmployees();
            workplaces = DatabaseAccess.loadWorkplace();

            CustomerModel customer = new CustomerModel();
            CarModel car = new CarModel();
            AppointmentModel appointment = new AppointmentModel();
            WarehouseModel part;
            OrdersModel orderedPart;

            /*
            if (partsStatus.Count == 0)
            {
                MessageBox.Show("Uzupełnij wszystkie dane i naciśnij przycisk \"Oblicz\"");
                return;
            }
            else
            {
                foreach (var item in partsStatus)
                {
                    if (partsStatus.Count == 0)
                    {
                        MessageBox.Show("Uzupełnij wszystkie dane i naciśnij przycisk \"Oblicz\"");
                        return;
                    }
                    else if (partsStatus.ContainsValue(PartStatus.Unavailable))
                    {
                        MessageBox.Show("Części są niedostępne");
                        return;
                    }
                }
            }
            */

            customer = customers.Find(x => x.firstName == firstNameText.Text &&
                                                x.lastName == lastNameText.Text &&
                                                x.phoneNumber == phoneText.Text);
            if (customer == null)
            {
                customer = new CustomerModel();
                customer.firstName = firstNameText.Text;
                customer.lastName = lastNameText.Text;
                customer.phoneNumber = phoneText.Text;
                customer.adress = adressText.Text;

                DatabaseAccess.saveCustomer(customer);
                customers = DatabaseAccess.loadCustomers();
                customer = customers.Find(x => x.firstName == customer.firstName &&
                                                    x.lastName == customer.lastName &&
                                                    x.phoneNumber == customer.phoneNumber);
            }

            car = cars.Find(x => x.registrationNumber == car.registrationNumber);
            if (car == null)
            {
                car = new CarModel();
                car.idCustomer = customer.idCustomer;
                car.brand = brandText.Text;
                car.model = modelText.Text;
                car.registrationNumber = registrationNumberText.Text;

                DatabaseAccess.saveCar(car);
                cars = DatabaseAccess.loadCars();
                car = cars.Find(x => x.registrationNumber == car.registrationNumber);
            }

            //make appointment
            appointment.idCar = car.idCar;
            appointment.idWorkplace = chosenWorkplace.idWorkplace;
            appointment.idEmployee = chosenEmployee.idEmployee;
            appointment.date = chosenDateTime;
            appointment.appointmentType = appointmentTypeComboBox.SelectedItem.ToString();
            appointment.cost = double.Parse(priceTextBlock.Content.ToString());
            appointment.estimatedTime = double.Parse(estimatedTimeText.Text);
            appointment.neededParts = neededPartsText.Text;

            foreach (var item in chosenPartsList)
            {
                part = warehouse.Find(x => x.partName == item.partName);
                if (item.partStatus == CarWorkshopLibrary.PartStatus.Ordered)
                {
                    orderedPart = orders.Find(x => x.idParts == part.idParts && x.amount >= x.bookedAmount && x.status != "zrealizowane");
                    orderedPart.bookedAmount += 1;
                    DatabaseAccess.updateOrderBookedAmount(orderedPart);
                } 
                else if (item.partStatus == CarWorkshopLibrary.PartStatus.Available)
                {
                    part.stockQuantity -= 1;
                    DatabaseAccess.updateWarehouse(part);
                }
            }

            DatabaseAccess.saveAppointment(appointment);
            DatabaseAccess.loadAppointments();


            firstNameText.Text = null;
            lastNameText.Text = null;
            phoneText.Text = null;
            adressText.Text = null;
            brandText.Text = null;
            modelText.Text = null;
            registrationNumberText.Text = null;
            appointmentTypeComboBox.SelectedItem = null;
            estimatedTimeText.Text = null;
            neededPartsText.Text = null;

            chosenDatePicker.SelectedDate = null;
            nearestDateCheckBox.IsChecked = false;
            dateTextBlock.Content = null;
            priceTextBlock.Content = null;
            employeeTextBlock.Content = null;
            workplaceTextBlock.Content = null;
        }

        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            services = DatabaseAccess.loadServices();
            customers = DatabaseAccess.loadCustomers();
            cars = DatabaseAccess.loadCars();
            appointments = DatabaseAccess.loadAppointments();
            warehouse = DatabaseAccess.loadWarehouse();
            orders = DatabaseAccess.loadOrders();
            employees = DatabaseAccess.loadEmployees();
            workplaces = DatabaseAccess.loadWorkplace();

            string[] separator = new string[]{ ", " };
            string[] neededParts = neededPartsText.Text.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            WarehouseModel part;
            OrdersModel pendingOrder;
            ServiceModel service = services.Find(x => x.serviceName == appointmentTypeComboBox.Text);

            DateTime nearestDate = AppointmentModel.RoundUp(DateTime.Now, TimeSpan.FromMinutes(15));
            bool nearestDateFound = false;
            List<WarehouseModel> confirmedNeededParts = new List<WarehouseModel>();
            List<AppointmentModel> collidingAppointments = new List<AppointmentModel>();
            List<EmployeeModel> availableEmployees = new List<EmployeeModel>();
            List<WorkplaceModel> availableWorkplaces = new List<WorkplaceModel>();

            OrdersModel orderedPart;
            PartObject chosenPart = new PartObject("",PartStatus.Unavailable,0);
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            string caption = "Czy chcesz ją zamówić?";
            DialogResult result;

            chosenPartsList.Clear();

            //reset employee and workplace
            chosenEmployee = new EmployeeModel();
            chosenWorkplace = new WorkplaceModel();

            //make appointment after working hours
            if(!AppointmentModel.isWorkDay(nearestDate) || !AppointmentModel.isWorkHour(nearestDate))
            {
                nearestDate = AppointmentModel.changeDateToNextWorkDay(nearestDate);
            }

            foreach (var item in neededParts)
            {
                part = warehouse.Find(x => x.partName == item); //by names 
                orderedPart = orders.Find(x => x.idParts == part.idParts && x.amount > x.bookedAmount && x.status != "zrealizowane");
                if (part == default)
                {
                    System.Windows.Forms.MessageBox.Show("Część: " + item + " nie jest znana");
                    chosenPart = new PartObject(item, PartStatus.Unavailable, 0);
                    chosenPartsList.Add(chosenPart);
                    return;
                }
                else if (part.stockQuantity == 0)  
                {
                    pendingOrder = orders.Find(x => x.idParts == part.idParts && x.status != "zrealizowane" && x.amount > x.bookedAmount);  
                    if (pendingOrder == default)
                    {
                        result = System.Windows.Forms.MessageBox.Show("Brak części: " + item + " w magazynie.", caption, buttons); //rozwinąć ew (przechodzenie do zamówienia części
                        if(result == System.Windows.Forms.DialogResult.Yes)
                        {
                            ManageOrdersWindow objManageOrdersWindow = new ManageOrdersWindow();
                            objManageOrdersWindow.Show();
                            this.Close();
                        }
                        else
                        {
                            chosenPart = new PartObject(item, PartStatus.Unavailable, 0);
                            chosenPartsList.Add(chosenPart);
                            return;
                        }
                    }
                    else
                    {
                        if (nearestDate < pendingOrder.realizationDate)
                        {
                            nearestDate = AppointmentModel.RoundUp(pendingOrder.realizationDate, TimeSpan.FromMinutes(15)); // If parts are ordered take new available nearestDate
                            if (!AppointmentModel.isWorkDay(nearestDate) || !AppointmentModel.isWorkHour(nearestDate))
                                nearestDate = AppointmentModel.changeDateToNextWorkDay(nearestDate);
                            chosenPart = new PartObject(item, PartStatus.Ordered, orderedPart.amount);
                            orders[orders.FindIndex(x => x.idOrder == orderedPart.idOrder)].bookedAmount += 1;
                            chosenPartsList.Add(chosenPart);
                        }
                    }
                }
                else
                {
                    chosenPart = new PartObject(item,PartStatus.Available, part.stockQuantity);
                    warehouse[warehouse.FindIndex(x => x.partName == item)].stockQuantity -= 1;
                    chosenPartsList.Add(chosenPart);
                }

                confirmedNeededParts.Add(part);

            } //all parts ready

            if(!nearestDateCheckBox.IsChecked.Value)
            {
                if (AppointmentModel.isWorkDay(chosenDatePicker.SelectedDate.Value))
                {
                    if (nearestDate.Date < chosenDatePicker.SelectedDate)
                    {
                        nearestDate = chosenDatePicker.SelectedDate.Value;
                        nearestDate = new DateTime(nearestDate.Year, nearestDate.Month, nearestDate.Day, AppointmentModel.startHour, 0, 0);
                    }
                    else if (nearestDate.Date > chosenDatePicker.SelectedDate)
                    {
                        System.Windows.Forms.MessageBox.Show("Brak części lub zły termin");
                        return;
                    }
                    // if equal take nearestDate
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Warsztat nieczynny w weekendy");
                    return;
                }

            }

            while(!nearestDateFound) //found neares available date
            {
                collidingAppointments = appointments.FindAll(x => (x.date <= nearestDate && x.GetFinishDate() > nearestDate) ||
                                                                  (x.date >= nearestDate && x.GetFinishDate() <= AppointmentModel.GetFinishDate(nearestDate, double.Parse(estimatedTimeText.Text))) ||
                                                                  (x.date >= nearestDate && x.date < AppointmentModel.GetFinishDate(nearestDate, double.Parse(estimatedTimeText.Text)) && x.GetFinishDate() > AppointmentModel.GetFinishDate(nearestDate, double.Parse(estimatedTimeText.Text))));
                if (collidingAppointments.Count != 0)
                {
                    availableEmployees = new List<EmployeeModel>(employees);
                    availableWorkplaces = new List<WorkplaceModel>(workplaces);
                    foreach (var item in collidingAppointments) // wolny pracownik i stanowisko
                    {
                        availableEmployees.Remove(availableEmployees.Find(x => x.idEmployee == item.idEmployee));
                        availableWorkplaces.Remove(availableWorkplaces.Find(x => x.idWorkplace == item.idWorkplace));
                    }

                    if (service.serviceName == "ogólny")
                    {
                        chosenEmployee = availableEmployees[0];
                        chosenWorkplace = availableWorkplaces.Find(x => x.workplaceName == service.serviceCategory);
                    }
                    else
                    {
                        chosenEmployee = availableEmployees.Find(x => x.specialization == service.serviceCategory);
                        chosenWorkplace = availableWorkplaces.Find(x => x.workplaceName == service.serviceCategory);
                    }

                    if (chosenWorkplace == default || chosenEmployee == default)  //sprawidzić działanie default
                    {
                        nearestDate = nearestDate.AddMinutes(15);
                        if (!AppointmentModel.isWorkHour(nearestDate))
                        {
                            if (!nearestDateCheckBox.IsChecked.Value)
                            {
                                System.Windows.Forms.MessageBox.Show("Brak wolnych godzin w wybranym dniu");
                                return;
                            }
                            else 
                                nearestDate = AppointmentModel.changeDateToNextWorkDay(nearestDate);
                        }
                        //następna iteracja
                    }
                    else
                    {
                        if (nearestDate.Hour == 18)
                            nearestDate = AppointmentModel.changeDateToNextWorkDay(nearestDate);
                        dateTextBlock.Content = nearestDate + " - " + AppointmentModel.GetFinishDate(nearestDate, double.Parse(estimatedTimeText.Text));
                        nearestDateFound = true;
                    }
                }
                else
                {
                    if (nearestDate.Hour == 18)
                        nearestDate = AppointmentModel.changeDateToNextWorkDay(nearestDate);
                    dateTextBlock.Content = nearestDate + " - " + AppointmentModel.GetFinishDate(nearestDate,double.Parse(estimatedTimeText.Text));
                    chosenEmployee = employees.Find(x => x.specialization == service.serviceCategory);
                    chosenWorkplace = workplaces.Find(x => x.workplaceName == service.serviceCategory);
                    nearestDateFound = true;
                }
            }
            //sprawdzić czy któraś ścieżka nie przypisuje
            employeeTextBlock.Content = chosenEmployee.firstName + " " + chosenEmployee.lastName;
            workplaceTextBlock.Content = chosenWorkplace.workplaceName + " stanowisko: " + chosenWorkplace.idWorkplace;
            //save chosen date
            chosenDateTime = nearestDate;

            //calculate price
            double price = 0;
            foreach (var item in confirmedNeededParts)
            {
                price += item.price;
            }
            price += service.price * double.Parse(estimatedTimeText.Text);
            priceTextBlock.Content = price;

        }

        private void nearestDateCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            chosenDatePicker.Visibility = Visibility.Hidden;
            chosenDatePicker.SelectedDate = null;
        }

        private void nearestDateCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            chosenDatePicker.Visibility = Visibility.Visible;
        }
    }
}
