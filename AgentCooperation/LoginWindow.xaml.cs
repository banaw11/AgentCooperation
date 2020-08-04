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
    /// Logika interakcji dla klasy LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        string id;
        string pswd;
        string msgNoUser = "User doesn't exists";
        string msgPswdErr = "Password isn't correct";
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IdTxtB.Visibility == Visibility.Visible)
            {
                id = IdTxtB.Text;
                if (!SqliteDataAccess.CheckId(id))
                {
                    InformationTxtB.Text = msgNoUser;
                    IdTxtB.Clear();
                }
                else
                {
                    IdTxtB.Visibility = Visibility.Hidden;
                    IdLbl.Visibility = Visibility.Hidden;
                    PasswordTxtB.Visibility = Visibility.Visible;
                    PswdLbl.Visibility = Visibility.Visible;
                }
            }
            else if(PasswordTxtB.Visibility == Visibility.Visible)
            {
                pswd = PasswordTxtB.Password;
                if (!SqliteDataAccess.CheckPassword(id, pswd))
                {
                    InformationTxtB.Text = msgPswdErr;
                    PasswordTxtB.Clear();
                }
                else
                {
                    Close();
                }
            }
        }
    }
}
