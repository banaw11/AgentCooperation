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

namespace AgentCooperation
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static DataGrid gridView;
        public static string agent;
        public static bool logged;
        public MainWindow()
        {
            if (!logged)
            {
                agent = null;
                while (agent is null)
                {
                    logged = false;
                    ShowLoginWindow();
                    if (agent is null)
                    if (LogInLoop() == MessageBoxResult.No)
                        Environment.Exit(0);
                }
                logged = true;
            }
            
            InitializeComponent();
            LoadCriteria();
            LoggedInAgentName.Content = SqliteDataAccess.GetName(agent);
            gridView = ViewOfCriteria;
            LoadGridView();
        }

        private MessageBoxResult LogInLoop()
        {
            return  MessageBox.Show("Acces to the system is only for authenticated \n Are you want logg in now ? ", "Login required", MessageBoxButton.YesNo, MessageBoxImage.Information);   
        }
        private void LoadCriteria()
        {
            ComboBoxItem item = new ComboBoxItem();
            item.Text = "Customers";
            item.Tag = 0;
            Criteria.Items.Add(item);
            item = new ComboBoxItem();
            item.Text = "Orders";
            item.Tag = 1;
            Criteria.Items.Add(item);
            Criteria.SelectedIndex = 0;
        }
        private void ShowLoginWindow()
        {
            LoginWindow login = new LoginWindow();
            login.ShowDialog();
        }

        private void LoadGridView()
        {
            if (Criteria.SelectedIndex == 0)
                gridView.ItemsSource = SqliteDataAccess.LoadOrders(agent);
            if (Criteria.SelectedIndex == 1)
                gridView.ItemsSource = SqliteDataAccess.LoadCustomers(agent);
        }

        private void AgentsBtn_Click(object sender, RoutedEventArgs e)
        {
            Agents agentsWindow = new Agents();
            Close();
            agentsWindow.Show();
        }

        private void OrdersBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CustomersBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            agent = null;
            Close();
            mainWindow.Show();
        }

        public class Order
        {
            DateTime date;
            public double Ord_Num { get; set; }
            public double Ord_Amount { get; set; }
            public double Advance_Amount { get; set; }
            public DateTime Ord_Date
            {
                get { return date; }
                set { date = value; } 
            }
            
            public string Cust_Code { get; set; }
            public string Ord_Description { get; set; }

            public Order()
            {

            }

            public Order(double or_NO, double or_amount, double adv_amount, DateTime date, string cust_code, string desc)
            {
                Ord_Num = or_NO;
                Ord_Amount = Ord_Amount;
                Advance_Amount = adv_amount;
                Ord_Date = date;
                Cust_Code = cust_code;
                Ord_Description = desc;
            }
        }

        public class Customer
        {
            public string Cust_Code { get; set; }
            public string Cust_Name { get; set; }
            public string Working_Area { get; set; }
            public string Cust_Country { get; set; }
            public string Phone_No { get; set; }

            public Customer()
            {

            }

            public Customer(string code, string name, string area, string country, string phone)
            {
                Cust_Code = code;
                Cust_Name = name;
                Working_Area = area;
                Cust_Country = country;
                Phone_No = phone;
            }
        }

        
    }
}
