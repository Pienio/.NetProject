using MahApps.Metro.Controls;
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
    /// Interaction logic for LekPac.xaml
    /// </summary>
    public partial class LekPac : MetroWindow
    {
        private int result=0;
        public LekPac()
        {
            InitializeComponent();
            
        }
        public int GetResult()
        {
            return result;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            result = 1;
            this.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            result = 2;
            this.Close();
        }
    }
}
