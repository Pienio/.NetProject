using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Visits.ViewModels
{
    class RegVisitViewModel : INotifyPropertyChanged
    {
        private Doctor _currentDoctor;
        private Patient _loggedPatient;
        private Week _currentWeek;

        public Doctor CurrentDoctor
        {
            get { return _currentDoctor; }
            set { _currentDoctor = value; OnPropertyChanged(nameof(CurrentDoctor)); }
        }
        public Patient LoggedPatient
        {
            get { return _loggedPatient; }
            set { _loggedPatient = value; OnPropertyChanged(nameof(LoggedPatient)); }
        }
        public Week CurrentWeek
        {
            get { return _currentWeek; }
            set { _currentWeek = value; OnPropertyChanged(nameof(CurrentWeek)); }
        }

        public ICommand ChangeWeekCmd => new Command((p) => CurrentWeek = new Week(CurrentDoctor, CurrentWeek.Days[0].Date.AddDays(int.Parse(p.ToString()))));

        public ICommand RegisterVisitCmd => new Command(p => RegisterVisit((DateTime)p));

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public RegVisitViewModel(Doctor doctor, Patient loggedPatient)
        {
            CurrentDoctor = doctor;
            LoggedPatient = loggedPatient;

            var first = doctor.FirstFreeSlot();
            first = first.AddDays(-Week.DayOfWeekNo(first));
            CurrentWeek = new Week(doctor, first.Date);
        }

        private void RegisterVisit(DateTime selectedDate)
        {
            //ListBox box = null;// sender as ListBox;
            //if (box.SelectedItem == null)
            //    return;
            if (LoggedPatient == null)
            {
                Login login = new Login();
                login.ShowDialog();
                if (!login.GetResult())
                    return;
            }
            if (MessageBox.Show(string.Format("Czy na pewno chcesz zarejestrować się do {0} na termin dnia {1:dd.MM.yyyy} o godzinie {1:HH:mm}?",
                CurrentDoctor.User.Name, selectedDate), App.ResourceAssembly.GetName().Name,
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            //dodac sprawdzenie, czy na pewno dany termin jest wolny
            using (var db = new ApplicationDataFactory().CreateApplicationData())
            {
                db.AddVisit(new Visit()
                {
                    Date = selectedDate,
                    Doctor = (from d in db.Doctors where d.Key == CurrentDoctor.Key select d).First(),
                    Patient = db.Patients.First()
                });
            }
            CurrentWeek = new Week(CurrentDoctor, CurrentWeek.Days[0].Date);
            MessageBox.Show("Wizyta została zarejestrowana", App.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public class Week
        {
            public Day[] Days { get; } = new Day[5];
            public string Title => string.Format("Aktualny tydzień: {0:dd.MM.yyyy} - {1:dd.MM.yyyy}", Days[0].Date, Days[0].Date.AddDays(6));

            public Week(Doctor doc, DateTime monday)
            {
                if (doc == null)
                    throw new ArgumentNullException(nameof(doc));
                int i = 0;
                using (var db = new ApplicationDataFactory().CreateApplicationData())
                {
                    foreach (var time in doc.WeeklyWorkingTime)
                    {
                        List<DateTime> slots = new List<DateTime>();
                        DateTime current = monday.AddDays(i - DayOfWeekNo(monday));
                        if (time != null)
                        {
                            current = new DateTime(current.Year, current.Month, current.Day, time.Start, 0, 0);
                            var visits = (from v in db.Visits
                                         where v.Doctor.Key == doc.Key && v.Date.Year == current.Year && v.Date.Month == current.Month && v.Date.Day == current.Day
                                         select v.Date).ToList();
                            for (DateTime s = current; s.Hour < time.End; s = s.AddMinutes(30))
                                if (!visits.Contains(s) && s >= DateTime.Now.AddHours(1))
                                    slots.Add(s);
                            
                        }
                        Days[i] = new Day(current.Date, slots.ToArray());
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
        }

        public class Day
        {
            public string DayName => new CultureInfo("pl-PL").DateTimeFormat.GetDayName(Date.DayOfWeek);
            public DateTime Date { get; }
            public DateTime[] Slots { get; }

            public Day(DateTime date, DateTime[] slots)
            {
                Date = date;
                Slots = slots;
            }
        }
    }
}
