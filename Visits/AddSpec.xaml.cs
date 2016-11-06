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

namespace Visits
{
    /// <summary>
    /// Interaction logic for AddSpec.xaml
    /// </summary>
    public partial class AddSpec : Window
    {
        private bool result = false;
        private string name;

        public AddSpec()
        {
            InitializeComponent();
        }
        public bool GetResult()
        {
            return result;
        }
        public string GetName()
        {
            return name;
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            result = false;
            this.Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            name = textBox.Text;
            result = true;
            this.Close();

        }
    }
}
