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
    /// Interaction logic for ChangePass.xaml
    /// </summary>
    public partial class ChangePass : Window
    {
        private bool result = false;
        private string Password;
        public ChangePass(string pass)
        {
            InitializeComponent();
            Password = pass;
        }
        public bool GetResult()
        {
            return result;
        }
        public string GetPassword()
        {
            return Password;
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (HashPassword(sh.Password) == Password)
            {
                result = true;
                Password = HashPassword(nh.Password);
                this.Close();
            }
            else
                MessageBox.Show("Hasło nieprawidłowe");
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
    }
}
