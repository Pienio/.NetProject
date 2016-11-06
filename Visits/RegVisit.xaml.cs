using DatabaseAccess;
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
    /// Interaction logic for RegVisit.xaml
    /// </summary>
    public partial class RegVisit : Window
    {
        public RegVisit(Doctor doctor)
        { 
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private class Week
        {
            public Doctor Doctor { get; }
            public Slot[][] Slots { get; } = new Slot[5][];
            public Week(Doctor doc, DateTime from)
            {
                if (doc == null)
                    throw new ArgumentNullException(nameof(doc));
                Doctor = doc;
                int i = 0;
                using (var db = new ApplicationDataFactory().CreateApplicationData())
                {
                    foreach (var time in doc.WeeklyWorkingTime)
                    {
                        if (time == null || DayOfWeekNo(from) > i)
                        {
                            i++;
                            continue;
                        }
                        List<Slot> slots = new List<Slot>();
                        var visits = from v in db.Visits
                                     where v.Doctor == Doctor && DayOfWeekNo(v.Date) == i
                                     select v.Date;
                    }
                }
                    
            }

            private int DayOfWeekNo(DateTime day)
            {
                switch(day.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        return 0;
                    case DayOfWeek.Tuesday:
                        return 1;
                    case DayOfWeek.Wednesday:
                        return 2;
                    case DayOfWeek.Thursday:
                        return 3;
                    case DayOfWeek.Friday:
                        return 4;
                }
                throw new ArgumentException("Dzień nie może być sobotą ani niedzielą.", nameof(day));
            }
        }

        private class Slot
        {
            public DateTime Start { get; }
            public DateTime End
            {
                get { return Start.AddMinutes(30); }
            }
            public Slot(DateTime start)
            {
                Start = start;
            }
        }
    }
}
