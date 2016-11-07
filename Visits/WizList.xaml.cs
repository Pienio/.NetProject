using DatabaseAccess;
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

namespace Visits
{
    /// <summary>
    /// Interaction logic for WizList.xaml
    /// </summary>
    public partial class WizList : Window
    {
        private IEnumerable wizytyakt; 
        private IEnumerable wizytyarc; 
        public WizList(IEnumerable wizyta, IEnumerable wizytar)
        {
            InitializeComponent();
            wizytyakt = wizyta;
            wizytyarc = wizytar;
            Listawizyt.ItemsSource = wizytyakt;

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            groupBox.Header = "Planowane";
            Listawizyt.ItemsSource = wizytyakt;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            groupBox.Header = "Archiwalne";
            Listawizyt.ItemsSource = wizytyarc;
        }
    }
}
