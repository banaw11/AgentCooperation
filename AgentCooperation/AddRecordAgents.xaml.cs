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
                    Agent agent = new Agent(TbName.Text, TbArea.Text, float.Parse(TbCommission.Text), TbPhone.Text, TbCountry.Text) ;
                    SqliteDataAccess.AddAgent(agent);
                }
                catch(FormatException)
                { 
                    MessageBox.Show("Value in field 'COMMISSION' is not correct. Record not added","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                    TbCommission.Text = null;
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

        private void CheckingCharInCommision(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            char value = (char)KeyInterop.VirtualKeyFromKey(e.Key);
            if (!char.IsDigit(value) && textBox.Text.Contains(','))
            {
                e.Handled = true;
            }
            else if (!char.IsDigit(value) && value != 188 && value !=190)
            {
                e.Handled = true;
            }
            
        }

        private void DotToComma(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            char value = (char)KeyInterop.VirtualKeyFromKey(e.Key);
            if (value != 188 && value != 190)
            {
                e.Handled = true;
            }
            else if (value == 190)
            {
                textBox.Text = textBox.Text.Replace('.', ',');
                textBox.Focus();
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private void CheckingCharInPhone(object sender, KeyEventArgs e)
        {
            if(!char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)))
                e.Handled = true;

            TextBox textBox = sender as TextBox;
            if (textBox.Text.Length == 3)
            {
                textBox.Text += '-';
                
            } 
        }
    }
}
