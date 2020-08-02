﻿using System;
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
    /// Logika interakcji dla klasy AddRecordAgents.xaml
    /// </summary>
    public partial class AddRecordAgents : Window
    {
        public AddRecordAgents()
        {
            InitializeComponent();
        }

        private void SaveRecord(object sender, EventArgs e)
        {
            bool add = true;
            if(TbName.Text == "" || TbArea.Text == "" || TbCommission.Text == "" || TbPhone.Text == "" || TbPhone.Text == "")
            {
                string messageBoxText = "Some field is empty. Continue adding record ?";
                string caption = "Adding Record";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage image = MessageBoxImage.Question;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, image);

                if (result == MessageBoxResult.No) add = false;
            }

            if (add)
            {
                try
                {
                    Agent agent = new Agent() { Agent_Name = TbName.Text, Working_Area = TbArea.Text, Commission = float.Parse(TbCommission.Text), Phone_No = TbPhone.Text, Country = TbCountry.Text };
                    SqliteDataAccess.AddAgent(agent);
                }
                catch(FormatException)
                { 
                    MessageBox.Show("Value in field 'COMMISSION' is not correct. Record not added");
                    TbCommission.Text = null;
                }
                catch(Exception fe)
                {
                    MessageBox.Show("Something went wrong... Record not added \n {0}", fe.Message);
                }
                finally
                {
                    Close();
                }
                
                
            }
        }

        private void CancelAdding(object sender, EventArgs e)
        {
            Close();
        }
    }
}
