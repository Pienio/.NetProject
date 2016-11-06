using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace Visits
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private bool result=false;
        private string User;
        private string Password;
        public Login()
        {
            InitializeComponent();
        }
        public bool GetResult()
        {
            return result;
        }
        public string GetUser()
        {
            return User;
        }
        public string GetHashedPassword()
        {
            return Password;
        }

        private void Anul_Click(object sender, RoutedEventArgs e)
        {
            result = false;
            this.Close();
        }
        private string HashPassword(string input)
        {
            StringBuilder sBuilder = new StringBuilder();
            using (MD5 md5Hash = MD5.Create())
            {

                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }

            return sBuilder.ToString();
        }

        private void Log_Click(object sender, RoutedEventArgs e)
        {
            if (usr.Text != null && password.Password != null)
            {
                result = true;
                User = usr.Text;
                Password = HashPassword(password.Password);
            }
            this.Close();
        }
    }
}
