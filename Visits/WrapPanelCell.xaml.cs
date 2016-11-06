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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Visits
{
    /// <summary>
    /// Interaction logic for WrapPanelCell.xaml
    /// </summary>
    public partial class WrapPanelCell : UserControl
    {
        public IEnumerable Hours
        {
            get { return (IEnumerable)GetValue(DatesProperty); }
            set { SetValue(DatesProperty, value); OnHoursChanged(); }
        }

        // Using a DependencyProperty as the backing store for Dates.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DatesProperty =
            DependencyProperty.Register("Hours", typeof(IEnumerable), typeof(WrapPanelCell), new PropertyMetadata(new DateTime[0]));
        
        public WrapPanelCell()
        {
            InitializeComponent();
            
        }

        private void OnHoursChanged()
        {
            foreach (DateTime t in Hours)
            {
                Button b = new Button()
                {
                    Content = string.Format($"{t.Hour:d2}:{t.Minute:d2}"),
                    Tag = t,
                    Padding = new Thickness(5),
                    Margin = new Thickness(5),
                };
                b.Click += button_Click;
                mainPanel.Children.Add(b);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ;
        }
    }
}
