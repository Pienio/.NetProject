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
    public class ValDoc : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        virtual protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        private Doctor us;
        private string _pas="";
        private string _pasp = "";
        public ValDoc()
        {
            us = new Doctor();
            us.User = new User();
            us.User.Name = new PersonName();
            us.MondayWorkingTime = new WorkingTime();
            us.TuesdayWorkingTime = new WorkingTime();
            us.ThursdayWorkingTime = new WorkingTime();
            us.WednesdayWorkingTime = new WorkingTime();
            us.FridayWorkingTime = new WorkingTime();
            us.Specialization = new Specialization();
            us.User.Kind = DocOrPat.Doctor;
            us.User.PESEL = "";
            us.User.Password = "";
        }
        public void SetDoc(Doctor a)
        {
            us = a;


        }
        public Doctor GetDoc()
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
                    int a;

                    if (Int32.TryParse(Pesel, out a))
                        result = "Pesel musi być ciągiem znaków!";
                    if (Pesel.Length != 11)
                        result = "Pesel musi mieć 11 cyfr!";
                }
                if (fieldName == "Pas")
                {

                    if (Pas.Length < 6)
                        result = "Hasło musi mieć 6 cyfr!";
                }

                if (fieldName == "Pasp")
                {

                    if (Pas != null && Pas != Pasp)
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
        public Specialization Spec
        {
            get { return us.Specialization; }
            set
            { 
                us.Specialization = value;
                OnPropertyChanged("Spec");
            }
        }

        public int PS
        {
            get { return us.MondayWorkingTime.Start; }
            set
            {
                
                us.MondayWorkingTime.Start =value;
                OnPropertyChanged("PS");
            }
        }
        public int PE
        {
            get { return us.MondayWorkingTime.End; }
            set
            {
                us.MondayWorkingTime.End = value;
                OnPropertyChanged("PE");
            }
        }
        public int WS
        {
            get { return us.TuesdayWorkingTime.Start; }
            set
            {
                us.TuesdayWorkingTime.Start = value;
                OnPropertyChanged("WS");
            }
        }
        public int WE
        {
            get { return us.TuesdayWorkingTime.End; }
            set
            {
                us.TuesdayWorkingTime.End = value;
                OnPropertyChanged("WE");
            }
        }
        public int SS
        {
            get { return us.WednesdayWorkingTime.Start; }
            set
            {
                us.WednesdayWorkingTime.Start = value;
                OnPropertyChanged("SS");
            }
        }
        public int SE
        {
            get { return us.WednesdayWorkingTime.End; }
            set
            {
                us.WednesdayWorkingTime.End = value;
                OnPropertyChanged("SE");
            }
        }
        public int CS
        {
            get { return us.ThursdayWorkingTime.Start; }
            set
            {
                us.ThursdayWorkingTime.Start = value;
                OnPropertyChanged("CS");
            }
        }
        public int CE
        {
            get { return us.ThursdayWorkingTime.End; }
            set
            {
                us.ThursdayWorkingTime.End= value;
                OnPropertyChanged("CE");
            }
        }
        public int PIS
        {
            get { return us.FridayWorkingTime.Start; }
            set
            {
                us.FridayWorkingTime.Start = value;
                OnPropertyChanged("PIS");
            }
        }
        public int PIE
        {
            get { return us.FridayWorkingTime.End; }
            set
            {
                us.FridayWorkingTime.End = value;
                OnPropertyChanged("PIE");
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
