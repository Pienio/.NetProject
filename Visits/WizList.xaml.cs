using DatabaseAccess;
using Microsoft.Practices.Unity;
using System;
using System.Collections;
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
    /// Interaction logic for WizList.xaml
    /// </summary>
    public partial class WizList : Window
    {
        [Dependency]
        public WizListViewModel ViewModel
        {
            get { return DataContext as WizListViewModel; }
            set { DataContext = value; }
        }
        public WizList()
        {
            InitializeComponent();

            visitsTypeBox.ItemsSource = Enum.GetValues(typeof(WizListViewModel.VisitsType));
        }
    }
}
