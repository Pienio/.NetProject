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
        private Week _week;
        private Doctor CurrentDoctor { get; set; }
        private Patient CurrentPatient { get; set; }
        private Week CurrentWeek
        {
            get { return _week; }
            set
            {
                if (value != null)
                {
                    object[] items = new object[value.Slots.Length];
                    for (int i = 0; i < value.Slots.Length; i++)
                    {
                        DateTime current = value.From.AddDays(i - Week.DayOfWeekNo(value.From));
                        items[i] = new
                        {
                            DayOfWeek = new CultureInfo("pl-PL").DateTimeFormat.GetDayName(current.DayOfWeek),
                            Date = current,
                            Hours = value.Slots[i]
                        };
                    }
                    daysOfWeek.ItemsSource = items;
                    weekTxt.Text = string.Format("Aktualny tydzień: {0:dd.MM.yyyy} - {1:dd.MM.yyyy}", value.From, value.From.AddDays(6));
                }
                _week = value;
            }
        }

        public RegVisit(Doctor doctor, Patient patient)
        {
            InitializeComponent();

            textBlock.Text = "Lekarz: " + doctor.User.Name.ToString();

            CurrentDoctor = doctor;
            CurrentPatient = patient;

            DateTime first = doctor.FirstFreeSlot();
            CurrentWeek = new Week(doctor, Week.GetLastMonday(doctor.FirstFreeSlot()));
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private class Week
        {
            public DateTime[][] Slots { get; } = new DateTime[5][];
            public DateTime From { get; }

            public Week(Doctor doc, DateTime monday)
            {
                if (doc == null)
                    throw new ArgumentNullException(nameof(doc));
                From = monday;
                DateTime firstFree = doc.FirstFreeSlot();
                firstFree = firstFree > monday ? firstFree : monday;
                int i = 0;
                using (var db = new ApplicationDataFactory().CreateApplicationData())
                {
                    foreach (var time in doc.WeeklyWorkingTime)
                    {
                        if (time != null && DayOfWeekNo(firstFree) <= i)
                        {
                            DateTime current = monday.AddDays(i - DayOfWeekNo(monday));
                            current = new DateTime(current.Year, current.Month, current.Day, time.Start, 0, 0);
                            List<DateTime> slots = new List<DateTime>();
                            var visits = from v in db.Visits
                                         where v.Doctor.Key == doc.Key && v.Date.Year == current.Year && v.Date.Month == current.Month && v.Date.Day == current.Day
                                         select v.Date;
                            for (DateTime s = current; s.Hour < time.End; s = s.AddMinutes(30))
                                if (s >= firstFree && !visits.Contains(s))
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
                switch (day.DayOfWeek)
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

            public static DateTime GetLastMonday(DateTime day)
            {
                day = day.AddDays(-DayOfWeekNo(day));
                return new DateTime(day.Year, day.Month, day.Day, 0, 0, 0);
            }
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var box = sender as ListBox;
            if (box.SelectedItem == null)
                return;
            DateTime chosen = (DateTime)box.SelectedItem;
            if (CurrentPatient == null)
            {
                Login login = new Login();
                login.ShowDialog();
                if (!login.GetResult())
                    return;
            }
            if (MessageBox.Show(string.Format("Czy na pewno chcesz zarejestrować się do {0} na termin dnia {1:dd.MM.yyyy} o godzinie {1:HH:mm}?",
                CurrentDoctor.User.Name, chosen), App.ResourceAssembly.GetName().Name,
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            //dodac sprawdzenie, czy na pewno dany termin jest wolny
            using (var db = new ApplicationDataFactory().CreateApplicationData())
            {
                db.AddVisit(new Visit()
                {
                    Date = chosen,
                    Doctor = (from d in db.Doctors where d.Key == CurrentDoctor.Key select d).First(),
                    Patient = db.Patients.First()
                });
            }
            CurrentWeek = new Week(CurrentDoctor, CurrentWeek.From);
            MessageBox.Show("Wizyta została zarejestrowana", App.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void nextWeek_Click(object sender, RoutedEventArgs e)
        {
            CurrentWeek = new Week(CurrentDoctor, CurrentWeek.From.AddDays(7));
            if (!prevWeek.IsEnabled)
                prevWeek.IsEnabled = true;
        }

        private void prevWeek_Click(object sender, RoutedEventArgs e)
        {
            CurrentWeek = new Week(CurrentDoctor, CurrentWeek.From.AddDays(-7));
            prevWeek.IsEnabled = CurrentDoctor.FirstFreeSlot() < CurrentWeek.From;
        }
    }
}
