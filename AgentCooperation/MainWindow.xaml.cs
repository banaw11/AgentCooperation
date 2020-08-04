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
        public MainWindow()
        {
            ShowLoginWindow();
            InitializeComponent();
            gridView = ViewOfCriteria;
        }

        private void ShowLoginWindow()
        {
            LoginWindow login = new LoginWindow();
            login.Show();
        }

        private void AgentsBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OrdersBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CustomersBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
