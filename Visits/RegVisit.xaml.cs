using DatabaseAccess;
using DatabaseAccess.Model;
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
        public RegVisit(Doctor doctor, Patient loggedPatient)
        {
            InitializeComponent();

            DataContext = new RegVisitViewModel(doctor, loggedPatient);
        }
    }
}
