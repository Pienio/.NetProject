using DatabaseAccess.Model;
using Microsoft.Practices.Unity;
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
using Visits.ViewModels;

namespace Visits
{
    /// <summary>
    /// Interaction logic for AddSpec.xaml
    /// </summary>
    public partial class AddSpec : Window 
    {
        [Dependency]
        public AddSpecViewModel ViewModel
        {
            get { return DataContext as AddSpecViewModel; }
            set { DataContext = value; }
        }
        public Specialization Specialization  { get; private set; }

        public AddSpec()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.CloseRequested += (o, args) => { DialogResult = args.DialogResult; Close(); };
        }
    }
}
