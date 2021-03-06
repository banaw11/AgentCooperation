﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public static DataGrid gridView;
        private static List<Agent> agents;
        public static  List<Agent> AgentsList
        {
            get { return agents; }
            set
            {
                agents = value;
                OnPropertyChanged("AgentsList");
            }
        } 
        public Agents()
        {
            InitializeComponent();
            AgentNameLbl.Content = SqliteDataAccess.GetName(MainWindow.agent);
            gridView = AgentsGridView;
            LoadAgents();
            Refreshing();
            LoadSearchCriteria();
        }

        protected static void OnPropertyChanged(string propertyName)
        {
            if (propertyName == "AgentsList")
                Refreshing();
            
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
        public void LoadAgents()
        {
            AgentsList = SqliteDataAccess.LoadAgents();

        }
        public static  void Refreshing()
        {
            gridView.ItemsSource = AgentsList;
        }
        private void AddNewRecord(object sender, EventArgs e)
        {
            AddRecordAgents addRecord = new AddRecordAgents();
            addRecord.Show();
            
        }
        private void RemoveRecord(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in gridView.SelectedItems.Cast<Agent>().ToList())
                {
                    SqliteDataAccess.RemoveAgent(item);
                }
            }
            catch (Exception fe)
            {
                MessageBox.Show("Something went wrong. Record not deleted \n " + fe.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoadAgents();
            }
            
        }

        private void RefreshView(object sender, EventArgs e)
        {
            LoadAgents();
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
                AgentsList = SqliteDataAccess.SearchAgents(item.Tag, valueOfSearch);
            }
            else
            {
                Refreshing();
            }
            
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            Close();
            mainWindow.Show();
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

        public Agent()
        {

        }

        public  Agent( string name, string area, float commission, string phone, string country)
        {
            Agent_Name = name;
            Working_Area = area;
            Commission = commission;
            Phone_No = phone;
            Country = country;
        }

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
