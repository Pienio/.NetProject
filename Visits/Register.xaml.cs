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
using DatabaseAccess;
using System.Security.Cryptography;
using Visits.Validations;
using System.Security;
using DatabaseAccess.Model;
using Microsoft.Practices.Unity;
using Visits.ViewModels;

namespace Visits
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        [Dependency]
        public RegisterVievModel ViewModel
        {
            get { return DataContext as RegisterVievModel; }
            set { DataContext = value; }
        }
        public bool WH { get; set; }
        public Register()
        {
            InitializeComponent();
         
            

        }
    

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.Initialize(WH);
        }

       

        private void Anul_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
