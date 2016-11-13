using DatabaseAccess;
using DatabaseAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Visits.EventArgsTypes;
using Visits.Services;
using Visits.Utils;

namespace Visits.ViewModels
{
    public class LoginViewModel : ViewModel
    {
        public event EventHandler<CloseRequestedEventArgs> CloseRequested;
        private string _pesel = "";

        public string Pesel
        {
            get { return _pesel; }
            set { _pesel = value; OnPropertyChanged(nameof(Pesel)); }
        }

        public LoginViewModel(ILogUserService user, IApplicationDataFactory factory) : base(factory, user) { }

        public ICommand Close => new Command(p =>
        {
            OnCloseRequested(false);
        });

        public ICommand LoginUser => new Command(async p =>
        {
            var db = _applicationDataFactory.CreateApplicationData();
            string pps = PasswordHasher.CreateHash(((PasswordBox)p).Password);
            var e = db.Users.Select(d => d).Where(s => s.PESEL == Pesel && s.Password == pps).ToList();
            if (e.Count != 0)
            {
                await _loggedUser.LogIn(e.First().PESEL, e.First().Password, db);       
                OnCloseRequested(true);
            }
            else
            {
                MessageBox.Show("Zły login lub hasło");
            }
        });

        protected virtual void OnCloseRequested(bool dialogResult)
        {
            if (CloseRequested != null)
                CloseRequested(this, new CloseRequestedEventArgs(dialogResult));
        }

    }
}
