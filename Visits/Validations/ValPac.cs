using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Visits.Validations
{
    public class ValPac : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        virtual protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public ValPac()
        {
            us = new Patient();
            us.User = new User();
            us.User.Name = new PersonName();
            us.User.Kind = DocOrPat.Patient;
            us.User.PESEL = "";
            us.User.Password = "";
           
        }
        public void SetPac(Patient a)
        {
            us = a;
        }
        private Patient us;
        private string _pas="";
        private string _pasp="";
        public Patient GetPat()
        {
            return us;
        }
        public string Error
        {
            get { return String.Empty; }
        }

        public string this[string fieldName]
        {
            get
            {
                string result = null;
                if (fieldName == "FirstName")
                {
                    if (string.IsNullOrEmpty(FirstName))
                        result = "Imię nie może być puste!";
                }
                if (fieldName == "LastName")
                {
                    if (string.IsNullOrEmpty(LastName))
                        result = "Nazwisko nie może być puste!";
                }
                if (fieldName == "Pesel")
                {
                    if (string.IsNullOrEmpty(Pesel))
                        result = "Pesel nie może być pusty!";

                    if (Pesel.Length!=11)
                        result = "Pesel musi mieć 11 cyfr!";
                }
                if (fieldName == "Pas")
                {
                    
                    if (Pas == null || Pas.Length == 0||Pas.Length < 6)
                        result = "Hasło musi mieć 6 cyfr!";
                }
                
                if (fieldName == "Pasp")
                {

                    if ( Pas !=null && Pas!= Pasp)
                        result = "Hasła muszą być takie same!";
                }
                return result;
            }
        }
        public string FirstName
        {
            get { return us.User.Name.Name; }
            set
            {
                us.User.Name.Name = value;
                OnPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get { return us.User.Name.Surname; }
            set
            {
                us.User.Name.Surname = value;
                OnPropertyChanged("LastName");
            }
        }

        public string Pesel
        {
            get { return us.User.PESEL; }
            set
            {
                us.User.PESEL = value;
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
        public string Pasp
        {
            get { return _pasp; }
            set
            {
                _pasp = value;
                us.User.Password = HashPassword(value);
                OnPropertyChanged("Pasp");
            }
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
