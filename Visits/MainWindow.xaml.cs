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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DatabaseAccess;

namespace Visits
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //To tylko na potrzeby testów
           
            try
            {
                var a = new ApplicationDataFactory();
                var db = a.CreateApplicationData();
                db.FillSpec();
                foreach (var c in db.AddSpec())
                {
                    MessageBox.Show(c.Name);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
           

            
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var zal = new Login();
            zal.Show();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var zar = new Register();
            zar.Show();
        }

        private void ZW_Click(object sender, RoutedEventArgs e)
        {
            var regwiz = new RegVisit();
            regwiz.Show();
        }
    }
}
