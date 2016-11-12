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
    public class WizListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IApplicationDataFactory _applicationDataFactory;
        private ILogUserService _loggedUser;
        private IEnumerable<Visit> _visits;
        private VisitsType _selectedType;

        public WizListViewModel(ILogUserService user, IApplicationDataFactory factory)
        {
            _loggedUser = user;
            _applicationDataFactory = factory;
        }

        public Person LoggedUser => _loggedUser.Logged;

        public IEnumerable<Visit> Visits
        {
            get { return _visits; }
            set
            {
                _visits = value;
                OnPropertyChanged(nameof(Visits));
            }
        }

        public VisitsType SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
                OnPropertyChanged(nameof(SelectedType));

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                SetVisits();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        public ICommand DeleteVisitCmd => new Command(async p =>
        {
            Visit v = p as Visit;
            if (MessageBox.Show("Czy na pewno chcesz odwołać zaznaczoną wizytę?", App.Name, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            var db = _applicationDataFactory.CreateTransactionalApplicationData();
            await Task.Run(() => db.Visits.Remove(v));
            db.Commit();
            await SetVisits();

            MessageBox.Show("Odwołano wizytę z powodzeniem", App.Name, MessageBoxButton.OK, MessageBoxImage.Information);
        });

        virtual protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task SetVisits()
        {
            var db = _applicationDataFactory.CreateApplicationData();
            DateTime now = DateTime.Now;
            IEnumerable<Visit> visits = null;
            if (_loggedUser.Logged is Patient)
                visits = await Task.Run(() => from v in db.Visits.Local
                                            where v.Patient.Key == _loggedUser.Logged.Key && (SelectedType == VisitsType.Archiwalne ? v.Date <= now : v.Date > now)
                                            select v);
            else if (_loggedUser.Logged is Doctor)
                visits = await Task.Run(() => from v in db.Visits.Local
                                            where v.Doctor.Key == _loggedUser.Logged.Key && (SelectedType == VisitsType.Archiwalne ? v.Date <= now : v.Date > now)
                                            select v);
            Visits = visits;
        }



        public void Initialize()
        {
        }

        public enum VisitsType { Planowane, Archiwalne }
    }
}
