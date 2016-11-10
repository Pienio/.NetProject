using DatabaseAccess;
using DatabaseAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
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

        private IApplicationDataFactory _applicationDataFactory;

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

        public ICommand ChangeWeekCmd => new Command(async p => CurrentWeek = await Week.Create(CurrentDoctor, CurrentWeek.From.AddDays(int.Parse(p.ToString())), _applicationDataFactory.CreateApplicationData()));
        public ICommand RegisterVisitCmd => new Command(async p =>
        {
            var selectedDate = (DateTime)p;
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
            var db = _applicationDataFactory.CreateTransactionalApplicationData();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            await AddVisit(new Visit(db.Patients.First(), (from d in db.Doctors where d.Key == CurrentDoctor.Key select d).First(), selectedDate), db);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            db.Commit();
            CurrentWeek = await Week.Create(CurrentDoctor, CurrentWeek.Days[0].Date, db);
            MessageBox.Show("Wizyta została zarejestrowana", App.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Information);
        });

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
        }

        public async Task Load()
        {
            var first = CurrentDoctor.FirstFreeSlot();
            first = first.AddDays(-Week.DayOfWeekNo(first));
            CurrentWeek = await Week.Create(CurrentDoctor, first.Date, _applicationDataFactory.CreateApplicationData());
        }

        private async Task AddVisit(Visit item, ITransactionalApplicationData context)
        {
            await Task.Run(() => context.Visits.Add(item));
        }

        public class Week
        {
            public Day[] Days { get; }
            public DateTime From { get; }
            public string Title => string.Format("Aktualny tydzień: {0:dd.MM.yyyy} - {1:dd.MM.yyyy}", From, From.AddDays(6));

            public Week(DateTime monday, Day[] days)
            {
                Days = days;
                From = monday;
            }

            public async static Task<Week> Create(Doctor doc, DateTime monday, IApplicationData db)
            {
                if (doc == null)
                    throw new ArgumentNullException(nameof(doc));
                int i = 0;
                List<Day> days = new List<Day>();
                foreach (var time in doc.WeeklyWorkingTime)
                {
                    List<DateTime> slots = new List<DateTime>();
                    DateTime current = monday.AddDays(i - DayOfWeekNo(monday));
                    if (time != null)
                    {
                        current = new DateTime(current.Year, current.Month, current.Day, time.Start, 0, 0);
                        var visits = await GetVisitsDates(db, doc, current);
                        for (DateTime s = current; s.Hour < time.End; s = s.AddMinutes(30))
                            if (!visits.Contains(s) && s >= DateTime.Now.AddHours(1))
                                slots.Add(s);
                    }
                    if (slots.Count > 0)
                        days.Add(new Day(current.Date, slots.ToArray()));
                    i++;
                }
                return new Week(monday, days.ToArray());
            }

            private async static Task<List<DateTime>> GetVisitsDates(IApplicationData db, Doctor doctor, DateTime day)
            {
                return await (from v in db.Visits
                       where v.Doctor.Key == doctor.Key && v.Date.Year == day.Year && v.Date.Month == day.Month && v.Date.Day == day.Day
                       select v.Date).ToListAsync();
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
