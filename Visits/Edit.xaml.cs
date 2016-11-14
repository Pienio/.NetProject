using DatabaseAccess;
using DatabaseAccess.Model;
using MahApps.Metro.Controls;
using Microsoft.Practices.Unity;
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
using Visits.Validations;
using Visits.ViewModels;

namespace Visits
{
    /// <summary>
    /// Interaction logic for Edit.xaml
    /// </summary>
    public partial class Edit : MetroWindow
    {
        
        [Dependency]
        public EditViewModel ViewModel
        {
            get { return DataContext as EditViewModel; }
            set { DataContext = value; }
        }
        public Edit()
        {
            InitializeComponent();
          
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
         
            ViewModel?.Initialize();
        
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
