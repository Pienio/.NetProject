using DatabaseAccess;
using DatabaseAccess.Model;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Visits.Services;

namespace Visits.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ILogUserService LoggingService { get; set; }
        private IApplicationDataFactory DbFactory { get; set; }
        private IEnumerable<Doctor> _doctors;
        private IEnumerable<Specialization> _specializations;
        private Doctor _selectedDoctor;
        private Specialization _selectedSpecialization;
        private string _searchByName;

        public string LoggedUserName => LoggingService?.Logged?.User?.Name?.ToString();
        public IEnumerable<Doctor> Doctors
        {
            get { return _doctors; }
            set { _doctors = value; OnPropertyChanged(nameof(Doctors)); }
        }
        public IEnumerable<Specialization> Specializations
        {
            get { return _specializations; }
            set { _specializations = value; OnPropertyChanged(nameof(Specializations)); }
        }
        public Doctor SelectedDoctor
        {
            get { return _selectedDoctor; }
            set { _selectedDoctor = value; OnPropertyChanged(nameof(SelectedDoctor)); }
        }
        public Specialization SelectedSpecialization
        {
            get { return _selectedSpecialization; }
            set { _selectedSpecialization = value; OnPropertyChanged(nameof(SelectedSpecialization)); }
        }
        public string SearchByNameText
        {
            get { return _searchByName; }
            set { _searchByName = value; OnPropertyChanged(nameof(SearchByNameText)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindowViewModel(ILogUserService user, IApplicationDataFactory factory)
        {
            LoggingService = user;
            DbFactory = factory;

            LoggingService.LoggedChanged += (o, e) =>
            OnPropertyChanged(nameof(LoggedUserName));
        }

        public ICommand LoginCmd => new Command(async p =>
        {
            if (LoggingService.Logged == null)
            {
                var zal = new Login();
                zal.ShowDialog();
                if (zal.GetResult())
                {
                    var db = DbFactory.CreateApplicationData();
                    string us = zal.GetUser();
                    string pas = zal.GetHashedPassword();
                    await LoggingService.LogIn(us, pas, DbFactory.CreateApplicationData());
                }
            }
            else
            {
                LoggingService.LogOut();
            }
            OnPropertyChanged(nameof(LoggedUserName));

        });

        
        public ICommand RegisterCmd => new Command(parameter =>
        {

            var lekpac = new LekPac();
            lekpac.ShowDialog();
            if (lekpac.GetResult() != 0)
            {
                var db = DbFactory.CreateApplicationData();
              
                    
                    
                    var wnd = App.Container.Resolve<Register>();
                    if (lekpac.GetResult() == 2)
                        wnd.WH = false;
                    else
                        wnd.WH = true;

                
                    wnd.Show();
           
                

               
            }
        });

        public ICommand RegisterVisitCmd => new Command(parameter =>
        {
            var db = DbFactory.CreateApplicationData();
            var wnd = App.Container.Resolve<RegVisit>();
            wnd.SelectedDoctor = SelectedDoctor;
            wnd.Show();
        });


        public ICommand SearchCmd => new Command(p =>
        {
            var db = DbFactory.CreateApplicationData();
            db.Doctors.Load();

            Predicate<Doctor> isValid;
            if (SelectedSpecialization != null)
            {
                if (SearchByNameText == null)
                    isValid = (doc) => doc.Specialization.Name == SelectedSpecialization.ToString();
                else
                    isValid = (doc) => doc.Specialization.Name == SelectedSpecialization.ToString() && doc.User.Name.ToString().ToLower().Contains(SearchByNameText.ToLower());
            }
            else if (SearchByNameText != null)
                isValid = (doc) => doc.User.Name.ToString().ToLower().Contains(SearchByNameText.ToLower());
            else return;
            
            Doctors = from d in db.Doctors.Local
                      where isValid(d)
                      select d;

        });

        public ICommand EditProfileCmd => new Command(parameter =>
        {
            var db = DbFactory.CreateApplicationData();
            Edit ed = new Edit();

            if (LoggingService.Logged is Doctor)
            {
                var specs = new List<Specialization>();
                specs.AddRange(db.Specializations);
                ed = new Edit(specs, LoggingService.Logged as Doctor);
            }
            else if (LoggingService.Logged is Patient)
            {
                ed = new Edit(LoggingService.Logged as Patient);
            }
            ed.ShowDialog();
            if (ed.GetResult())
            {
                var transaction = DbFactory.CreateTransactionalApplicationData();
                if (ed.GetPatient() != null)
                {
                    //db.UpdatePatient(ed.GetPatient());
                }
                else
                {
                    if (ed.GetSpec() != null)
                        transaction.Specializations.Add(ed.GetSpec());
                    //db.UpdateDoctor(ed.GetDoctor());
                }
                var usr = db.Users.Select(n => n).Where(p => p.Key == LoggingService.Logged.User.Key);
                if (usr.Count() != 0)
                {
                    LoggingService.LogIn(usr.First().PESEL, usr.First().Password, db);
                }

            }
        });

        public ICommand VisitsViewCmd => new Command(p =>
        {
            var db = DbFactory.CreateApplicationData();
            DateTime now = DateTime.Now;

            try
            {
                db.Visits.Load();
                var da = from v in db.Visits.Local
                         where v.Patient.Key == LoggingService.Logged.Key && DateTime.Compare(v.Date, now) > 0
                         select new { DoctorId = v.Doctor.Key, Name = v.Doctor.User.Name, Specialization = v.Doctor.Specialization, DataWizyty = v.Date };

                var dar = from v in db.Visits.Local
                          where v.Patient.User.Key == LoggingService.Logged.Key && DateTime.Compare(v.Date, now) <= 0
                          select new { DoctorId = v.Doctor.Key, Name = v.Doctor.User.Name, Specialization = v.Doctor.Specialization, DataWizyty = v.Date };

                WizList op = new WizList(da, dar);
                op.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        });

        public void Initialize()
        {
            var db = DbFactory.CreateApplicationData();

            var specs = new List<Specialization>();
            specs.Add(new Specialization("- brak -"));
            specs.AddRange(db.Specializations);
            Specializations = specs;

            db.Users.Load();
            db.Doctors.Load();
            Doctors = db.Doctors.Local;
        }
    }
}
