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
using Visits.Services;
using Visits.Utils;

namespace Visits.ViewModels
{
    public class ChangePassViewModel : ViewModel, IDataErrorInfo
    {
        private string _org = "";
        private string _pas = "";
        private string _pasp = "";

        public ChangePassViewModel(ILogUserService user, IApplicationDataFactory factory) : base(factory, user) { }

        public string Error
        {
            get { return String.Empty; }
        }

        public string this[string fieldName]
        {
            get
            {
                string result = null;
                if (fieldName == "Org")
                {

                    if (Org.Length < 1)
                        result = "Wprowadź stare hasło!";
                    
                }
                if (fieldName == "Pas")
                {

                    if (Pas.Length < 6)
                        result = "Hasło musi mieć 6 znaków!";

                }

                if (fieldName == "Pasp")
                {

                    if (Pas != null && Pas != Pasp)
                        result = "Hasła muszą być takie same!";
                }
                return result;
            }
        }
        public string Org
        {
            get { return _org; }
            set
            {
                _org = value;
                OnPropertyChanged("Org");
                
            }
        }
        public string Pas
        {
            get { return _pas; }
            set
            {
                _pas = value;
                OnPropertyChanged("Pas");
                OnPropertyChanged("Pasp");
            }
        }
        public string Pasp
        {
            get { return _pasp; }
            set
            {
                _pasp = value;
                
                OnPropertyChanged("Pasp");
            }
        }
        public ICommand ChangePass => new Command(p =>
        {

            PasswordBox a = (PasswordBox)p;
            Org = a.Password;
            
            

        });
        public ICommand Close => new Command(p =>
        {

            Window k = p as Window;
            k.Close();

        });
        public ICommand UpdatePassword => new Command(p =>
        {

            var db = _applicationDataFactory.CreateTransactionalApplicationData();
            
            if (PasswordHasher.CreateHash(Org) == _loggedUser.Logged.User.Password)
            {
                _loggedUser.Logged.User.Password =PasswordHasher.CreateHash(Pasp);
                db.Commit();
                Window k = p as Window;
                k.Close();
            }
            else
            {
                MessageBox.Show("Nieprawidłowe hasło");
            }

        });
        public ICommand ChangePass1 => new Command(p =>
        {

            PasswordBox a = (PasswordBox)p;
            Pas = a.Password;

        });
        public ICommand ChangePass2 => new Command(p =>
        {

            PasswordBox a = (PasswordBox)p;
            Pasp = a.Password;

        });
        public void Initialize()
        {
            //OnPropertyChanged("Org");
            //OnPropertyChanged("Pas");
        }
      
    }
}
