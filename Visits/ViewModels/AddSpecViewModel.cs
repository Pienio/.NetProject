using DatabaseAccess;
using DatabaseAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Visits.EventArgsTypes;
using Visits.Services;
using Visits.Utils;

namespace Visits.ViewModels
{
    public class AddSpecViewModel : ViewModel
    {
        public event EventHandler<CloseRequestedEventArgs> CloseRequested;
        private Specialization _specialization = new Specialization() { Name = "" };

        public Specialization Specialization
        {
            get { return _specialization; }
            set { _specialization = value; OnPropertyChanged(nameof(Specialization)); }
        }

        public AddSpecViewModel(ILogUserService user, IApplicationDataFactory factory) : base(factory, user) { }

        public ICommand AcceptCmd => new Command(async p =>
        {
            var db = _applicationDataFactory.CreateTransactionalApplicationData();
            bool res = await Task.Run(() => db.Specializations.Any(s => Specialization.Name == s.Name));
            if (res)
            {
                MessageBox.Show("Specjalizacja o danej nazwie już istnieje.", App.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            await App.Current.Dispatcher.BeginInvoke((Action)(() => db.Specializations.Add(Specialization)));
            db.Commit();
            OnCloseRequested(true);
        }, p =>
        {
            return !string.IsNullOrWhiteSpace(Specialization?.Name);
        });

        public ICommand CancelCmd => new Command(p =>
        {
            Specialization = null;
            OnCloseRequested(false);
        });

        private void OnCloseRequested(bool dialogResult)
        {
            if (CloseRequested != null)
                CloseRequested(this, new CloseRequestedEventArgs(dialogResult));
        }
    }
}
