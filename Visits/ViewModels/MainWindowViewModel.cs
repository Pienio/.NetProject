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
using Visits.Utils;

namespace Visits.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private IEnumerable<Doctor> _doctors;
        private IEnumerable<Specialization> _specializations;
        private Doctor _selectedDoctor;
        private Specialization _selectedSpecialization;
        private string _searchByName;

        public string LoggedUserName => _loggedUser?.Logged?.User?.Name?.ToString();

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

        public MainWindowViewModel(ILogUserService user, IApplicationDataFactory factory) : base(factory, user)
        {
            _loggedUser.LoggedChanged += (o, e) => OnPropertyChanged(nameof(LoggedUserName));
        }

        public ICommand LoginCmd => new Command(p =>
        {

            if (_loggedUser.Logged == null)
            {
                var db = _applicationDataFactory.CreateApplicationData();
                var wnd = App.Container.Resolve<Login>();
                wnd.ShowDialog();

            }
            else
            {
                _loggedUser.LogOut();
            }
            OnPropertyChanged(nameof(LoggedUserName));
        });

        public ICommand RegisterCmd => new Command(parameter =>
        {

            var lekpac = new LekPac();
            lekpac.ShowDialog();
            if (lekpac.GetResult() != 0)
            {
                var db = _applicationDataFactory.CreateApplicationData();
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
            var db = _applicationDataFactory.CreateApplicationData();
            var wnd = App.Container.Resolve<RegVisit>();
            wnd.SelectedDoctor = SelectedDoctor;
            wnd.Show();
        });


        public ICommand SearchCmd => new Command(p =>
        {
            var db = _applicationDataFactory.CreateApplicationData();
            db.Doctors.Load();

            Predicate<Doctor> isValid = (doc) => true;
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
            var db = _applicationDataFactory.CreateApplicationData();
            var wnd = App.Container.Resolve<Edit>();
            wnd.ShowDialog();
            OnPropertyChanged(nameof(LoggedUserName));
        });

        public ICommand VisitsViewCmd => new Command(p =>
        {
            var db = _applicationDataFactory.CreateApplicationData();
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

        public ICommand ClearFilters => new Command(p =>
        {
            SelectedSpecialization = null;
            SearchByNameText = "";

            SearchCmd.Execute(null);
        });

        public void Initialize()
        {
            var db = _applicationDataFactory.CreateApplicationData();

            db.Specializations.Load();
            Specializations = db.Specializations.Local;

            db.Users.Load();
            db.Doctors.Load();
            Doctors = db.Doctors.Local;

            _loggedUser.LogIn("95122907757", PasswordHasher.CreateHash("panda2"), _applicationDataFactory.CreateApplicationData());
        }
    }
}
