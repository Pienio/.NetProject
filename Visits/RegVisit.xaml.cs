using DatabaseAccess;
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

            textBlock.Text = "Lekarz: " + doctor.User.Name.ToString();
            DateTime first = doctor.FirstFreeSlot();
            Week w = new Week(doctor, first);
            object[] items = new object[w.Slots.Length];
            for (int i = 0; i < w.Slots.Length; i++)
            {
                items[i] = new
                {
                    DayOfWeek = new CultureInfo("pl-PL").DateTimeFormat.GetDayName(first.AddDays(i - Week.DayOfWeekNo(first)).DayOfWeek),
                    Hours = w.Slots[i] 
                };
            }
            daysOfWeek.ItemsSource = items;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private class Week
        {
            public Doctor Doctor { get; }
            public DateTime[][] Slots { get; } = new DateTime[5][];
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
                        if (time != null && DayOfWeekNo(from) <= i)
                        {
                            DateTime current = from.AddDays(i - DayOfWeekNo(from));
                            current = new DateTime(current.Year, current.Month, current.Day, time.Start, 0, 0);
                            List<DateTime> slots = new List<DateTime>();
                            //wizyty u danego lekarza danego dnia
                            var visits = from v in db.Visits
                                         where v.Doctor.Key == Doctor.Key && v.Date.Year == current.Year && v.Date.Month == current.Month && v.Date.Day == current.Day
                                         select v.Date;
                            for (DateTime s = current; s.Hour < time.End; s = s.AddMinutes(30))
                                if (s >= from && !visits.Contains(s))
                                    slots.Add(s);
                            Slots[i] = slots.ToArray();
                        }
                        else Slots[i] = new DateTime[0];
                        i++;
                    }
                }
                    
            }

            public static int DayOfWeekNo(DateTime day)
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
    }
}
