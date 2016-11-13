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
using Microsoft.Practices.Unity;
using Visits.Utils;

namespace Visits.ViewModels
{
    public class EditViewModel : ViewModel, IDataErrorInfo
    {
        private User _User;
        private IEnumerable<Specialization> _SpecList;
        private Patient _Patient;
        private Doctor us;
        private string _pas = "";
        private bool _Who;

        public EditViewModel(ILogUserService user, IApplicationDataFactory factory) : base(factory, user) { }
        
        public bool Who
        {
            get { return _Who; }
            set
            {
                _Who = value;
                OnPropertyChanged("Who");

            }
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
                        result = "Pesel musi być ciągiem cyfr!";
                    if (Pesel.Length != 11)
                        result = "Pesel musi mieć 11 cyfr!";


                }
                if (fieldName == "Pas")
                {

                    if (Pas.Length < 1)
                        result = "Prosze wprowadzić hasło, aby zatwierdzić zmiany!";

                }

                
                return result;
            }
        }
        public string FirstName
        {
            get { return _User.Name.Name; }
            set
            {
                _User.Name.Name = value;
                OnPropertyChanged("FirstName");

            }
        }

        public string LastName
        {
            get { return _User.Name.Surname; }
            set
            {
                _User.Name.Surname = value;
                OnPropertyChanged("LastName");
            }
        }

        public string Pesel
        {
            get { return _User.PESEL; }
            set
            {
                _User.PESEL = value;
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
      
        public IEnumerable<Specialization> SpecList
        {
            get { return _SpecList; }
            set
            {
                _SpecList = value;
                OnPropertyChanged("SpecList");
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

                us.MondayWorkingTime.Start = value;
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
                us.ThursdayWorkingTime.End = value;
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

        public ICommand ChangePass => new Command(p =>
        {

            PasswordBox a = (PasswordBox)p;
            Pas =HashPassword(a.Password);
            
        });
        public ICommand Close => new Command(p =>
        {

            Window k = p as Window;
            k.Close();

        });
        public ICommand ChangePassword => new Command(p =>
        {
       
            var db = _applicationDataFactory.CreateApplicationData();
            var wnd = App.Container.Resolve<ChangePass>();
            wnd.ShowDialog();
        
            
        });
        public ICommand UpdateUser => new Command( p =>
        {
           
                var db = _applicationDataFactory.CreateTransactionalApplicationData();

                if (Pas == _User.Password)
                {
                    db.Commit();
                    
                 


                    Window k = p as Window;
                    k.Close();
                }
                else
                {
                    MessageBox.Show("Hasło nieprawidłowe");
                    Pas = "";
                }
          
            
                

        });

      
  
        
        private async Task AddSpec(Specialization item, ITransactionalApplicationData context)
        {
            await App.Current.Dispatcher.BeginInvoke((Action)(() => { context.Specializations.Add(item); }));

        }
        public void Initialize()
        {
           
            var db = _applicationDataFactory.CreateApplicationData();
            if(_loggedUser.Logged is Patient)
            {
                _Patient = _loggedUser.Logged as Patient;  
                _User = _Patient.User;
                Who = false;
            }
            else
            {
                us = _loggedUser.Logged as Doctor; 
                _User = us.User;
                Who = true;
                var spec = new List<Specialization>();
                spec.AddRange(db.Specializations);
                SpecList = spec;
                OnPropertyChanged("PS");
                OnPropertyChanged("PE");
                OnPropertyChanged("WS");
                OnPropertyChanged("WE");
                OnPropertyChanged("SS");
                OnPropertyChanged("SE");
                OnPropertyChanged("CS");
                OnPropertyChanged("CE");
                OnPropertyChanged("PIS");
                OnPropertyChanged("PIE");
                OnPropertyChanged("Spec");
            }
            OnPropertyChanged("Pesel");
            OnPropertyChanged("Who");
            OnPropertyChanged("FirstName");
            OnPropertyChanged("LastName");

        }
    }

}
