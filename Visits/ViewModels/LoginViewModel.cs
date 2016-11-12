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

namespace Visits.ViewModels
{
    public class LoginViewModel: INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IApplicationDataFactory _applicationDataFactory;
        private ILogUserService _loggedUser;
        string _pesel="";
        string _pas = "";

        public LoginViewModel(ILogUserService user, IApplicationDataFactory factory)
        {
            _loggedUser = user;
            _applicationDataFactory = factory;


        }

        virtual protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public string Error
        {
            get { return String.Empty; }
        }
        public ICommand ChangePass => new Command(p =>
        {

            PasswordBox a = (PasswordBox)p;
            Pas = a.Password;
            
        });
        public ICommand Close => new Command(p =>
        {

            Window k = p as Window;
            k.Close();

        });
        public ICommand LoginUser => new Command(async p =>
        {
            var db = _applicationDataFactory.CreateApplicationData();
            string pps = HashPassword(Pas);
            var e = db.Users.Select(d => d).Where(s => s.PESEL == Pesel && s.Password == pps).ToList();
            if(e.Count!=0)
            {
                await _loggedUser.LogIn(e.First().PESEL, e.First().Password, db);
                Window k = p as Window;
                k.Close();
            }
            else
            {
                MessageBox.Show("Zły login lub hasło");
            }
            

        });
        public string this[string fieldName]
        {
            get
            {
                string result = null;
                if (fieldName == "Pesel")
                {

                    if (string.IsNullOrEmpty(Pesel))
                        result = "Pesel nie może być pusty!";
                    int a;

                    if (Int32.TryParse(Pesel, out a))
                        result = "Pesel musi być ciągiem cyfr!";
                    if (Pesel.Length != 11)
                        result = "Pesel musi mieć 11 cyfr!";

                }
                if (fieldName == "Pas")
                {

                    if (Pas.Length < 6)
                        result = "Hasło musi mieć 6 znaków!";

                }

                return result;
            }
        }
        public string Pesel
        {
            get { return _pesel; }
            set
            {
                _pesel = value;
                OnPropertyChanged("Pesel");

            }
        }
        public string Pas
        {
            get { return _pas; }
            set
            {
                _pas = value;
                OnPropertyChanged("Pas");
                
            }
        }
        public void Initialize()
        {
            OnPropertyChanged("Pesel");
            OnPropertyChanged("Pas");
        }
        private string HashPassword(string input)
        {
            StringBuilder sBuilder = new StringBuilder();
            using (MD5 md5Hash = MD5.Create())
            {

                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }

            return sBuilder.ToString();
        }
    }
}
