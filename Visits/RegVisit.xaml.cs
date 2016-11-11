using DatabaseAccess;
using DatabaseAccess.Model;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for RegVisit.xaml
    /// </summary>
    public partial class RegVisit : Window
    {
        [Dependency]
        public RegVisitViewModel ViewModel
        {
            get { return DataContext as RegVisitViewModel; }
            set { DataContext = value; }
        }

        public Doctor SelectedDoctor { get; set; }

        public RegVisit()
        {
            InitializeComponent();
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.Initialize(SelectedDoctor);
        }
    }
}
