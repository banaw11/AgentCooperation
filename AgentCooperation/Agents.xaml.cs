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
    /// Logika interakcji dla klasy Agents.xaml
    /// </summary>
    public partial class Agents : Window
    {
        private static List<Agent> agents;
        public Agents()
        {
            InitializeComponent();
            LoadAgents();
            Refreshing();
            LoadSearchCriteria();
        }

        private void LoadSearchCriteria()
        {
            int value = AgentsGridView.Columns.Count();

            for (int i = 0; i < value; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Text = AgentsGridView.Columns[i].Header.ToString();
                item.Tag = i;
                Criteria.Items.Add(item);
            }

            Criteria.SelectedIndex = 0;
        }
        public static void LoadAgents()
        {
            agents = SqliteDataAccess.LoadAgents();
            
        }
        public void Refreshing()
        {
            AgentsGridView.ItemsSource = agents;
        }
        private void AddNewRecord(object sender, EventArgs e)
        {
            AddRecordAgents addRecord = new AddRecordAgents();
            addRecord.Show();
        }
        private void RemoveRecord(object sender, EventArgs e)
        {

        }

        private void RefreshView(object sender, EventArgs e)
        {
            LoadAgents();
            Refreshing();
            SearchText.Text = null;
        }

        private void SearchData(object sender, EventArgs e)
        {
            bool search = true;
            if(SearchText.Text == "")
            {
                string messageBoxText = "Searching field is empty. Continue searching data ?";
                string caption = "Searching data";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage image = MessageBoxImage.Question;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, image);

                if (result == MessageBoxResult.No) search = false;
            }
            if (search)
            {
                ComboBoxItem item = Criteria.SelectedItem as ComboBoxItem;
                string valueOfSearch = SearchText.Text;
                agents = SqliteDataAccess.SearchAgents(item.Tag, valueOfSearch);
                Refreshing();
            }
            else
            {
                Refreshing();
            }
            
        }


    }

    public class Agent
    {
        public string Agent_Code { get; set; }
        public string Agent_Name { get; set; }
        public string Working_Area { get; set; }
        public float Commission { get; set; }
        public string Phone_No { get; set; }
        public string Country { get; set; }

    }

    public class ComboBoxItem
    {
        public string Text { get; set; }
        public int Tag { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
