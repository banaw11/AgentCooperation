using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        bool checkPswd = true;
        string msgNoUser = "User doesn't exists";
        string msgPswdErr = "Password isn't correct";
        string msgPswdSet = "Please set your password to access to the system";
        string msgPswdReq = "Password doesn't meet the requirements ";
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IdTxtB.Visibility == Visibility.Visible)
            {
                id = IdTxtB.Text.ToUpper();
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
                    if (!SqliteDataAccess.CheckIfPasswordSetted(id))
                        InformationTxtB.Clear();
                    else
                    {
                        InformationTxtB.Text = msgPswdSet;
                        checkPswd = false;
                    }
                    
                    
                }
            }
            else if(PasswordTxtB.Visibility == Visibility.Visible)
            {
                pswd = PasswordTxtB.Password;
                if (checkPswd)
                {
                    if (!SqliteDataAccess.CheckPassword(id, pswd))
                    {
                        InformationTxtB.Text = msgPswdErr;
                        PasswordTxtB.Clear();
                    }
                    else
                    {
                        MainWindow.agent = id;
                        Close();
                    }
                }
                else
                {
                    if (PasswordRequirments(pswd))
                    {
                        SqliteDataAccess.SetPassword(id, pswd);
                        MainWindow.agent = id;
                        Close();
                    }
                    else
                    {
                        InformationTxtB.Text = msgPswdReq;
                        PasswordTxtB.Clear();

                    }
                        

                }
               
            }
        }

        private bool PasswordRequirments(string password)
        {
            return password.Length >7 && password.Any(char.IsDigit) && password.Any(char.IsUpper) && password.Any(char.IsLower) &&  Regex.IsMatch(password, @"[^A-Za-z0-9]");
        }
    }
}
