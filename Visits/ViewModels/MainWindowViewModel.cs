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

        //public string LoggedUserName => LoggingService?.Logged?.User?.Name?.ToString();
        public string LoggedUserName
        {
            get { return LoggingService?.Logged?.User?.Name?.ToString(); }
           
        }
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

        public ICommand LoginCmd => new Command(p =>
        {
            
                if (LoggingService.Logged == null)
                {
                var db = DbFactory.CreateApplicationData();
                var wnd = App.Container.Resolve<Login>();
                wnd.ShowDialog();

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
            var wnd = App.Container.Resolve<Edit>();
            wnd.ShowDialog();
            OnPropertyChanged(nameof(LoggedUserName));



        });

        public ICommand VisitsViewCmd => new Command(p =>
        {
            var db = DbFactory.CreateApplicationData();
            DateTime now = DateTime.Now;

            try
            {
                db.Visits.Load();
                var wnd = App.Container.Resolve<WizList>();
                wnd.ShowDialog();
               
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
