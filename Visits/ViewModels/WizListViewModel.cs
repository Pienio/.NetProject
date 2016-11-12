using DatabaseAccess;
using DatabaseAccess.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Visits.Services;

namespace Visits.ViewModels
{
    public class WizListViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IApplicationDataFactory _applicationDataFactory;
        private ILogUserService _loggedUser;
        private Patient _Patient;
        private Doctor _Doctor;
        private IEnumerable<Visit> wizytyakt;
        private IEnumerable<Visit> wizytyarc;
        private IEnumerable<Visit> _wat;
        private bool _WHO;
        public WizListViewModel(ILogUserService user, IApplicationDataFactory factory)
        {
            _loggedUser = user;
            _applicationDataFactory = factory;


        }
        public bool Who
        {
            get { return _WHO; }
            set
            {
                _WHO = value;
                OnPropertyChanged("Who");
            }
        }
        public IEnumerable<Visit> Wat
            {
                get { return _wat; }
                set
                {
                    _wat = value;
                     OnPropertyChanged("Wat");

                 }
            }

        public ICommand Change1 => new Command(p =>
        {

            Wat = wizytyakt;
            OnPropertyChanged("Wat");

        });
        public ICommand Change2 => new Command(p =>
        {

            Wat = wizytyarc;
            OnPropertyChanged("Wat");

        });
        virtual protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Initialize()
        {
            var db = _applicationDataFactory.CreateApplicationData();
            try
            {
                if (_loggedUser.Logged is Patient)
                {
                    _Patient = _loggedUser.Logged as Patient;

                    DateTime now = DateTime.Now;
                    
                    var da = from v in db.Visits.Local
                             where v.Patient.Key == _Patient.Key && DateTime.Compare(v.Date, now) > 0
                             select new Visit {Key=v.Key,Version=v.Version, Doctor = v.Doctor, Patient=v.Patient, Date = v.Date };

                    var dar = from v in db.Visits.Local
                              where v.Patient.Key == _Patient.Key && DateTime.Compare(v.Date, now) <= 0
                              select new Visit { Key = v.Key, Version = v.Version, Doctor = v.Doctor, Patient = v.Patient, Date = v.Date };
                    wizytyakt = da;
                     wizytyarc = dar;
                    Who = false;

                }
                else
                {
                    _Doctor = _loggedUser.Logged as Doctor;
                    DateTime now = DateTime.Now;

                    var da = from v in db.Visits.Local
                             where v.Doctor.Key == _Doctor.Key && DateTime.Compare(v.Date, now) > 0
                             select new Visit { Key = v.Key, Version = v.Version, Doctor = v.Doctor, Patient = v.Patient, Date = v.Date };

                    var dar = from v in db.Visits.Local
                              where v.Doctor.Key == _Doctor.Key && DateTime.Compare(v.Date, now) <= 0
                              select new Visit { Key = v.Key, Version = v.Version, Doctor = v.Doctor, Patient = v.Patient, Date = v.Date };
                    wizytyakt = da;
                    wizytyarc = dar;
                    Who = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
          
            Wat = wizytyakt;
            OnPropertyChanged("Wat");
            OnPropertyChanged("Who");

        }
     }
}
