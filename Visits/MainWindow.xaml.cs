using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DatabaseAccess;
using System.Data.Entity;
using System.Security.Cryptography;

namespace Visits
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User ActuallyLogged=null;
        public MainWindow()
        {
            InitializeComponent();
            
            EdProf.Visibility = Visibility.Collapsed;
            var a = new ApplicationDataFactory();
            using (var db = a.CreateApplicationData())
            {
               
                    db.Fill();

                var specs = new List<Specialization>();
                specs.Add(new Specialization("- brak -"));
                specs.AddRange(db.Specializations);
                specs.RemoveAt(1);
                Spec.ItemsSource = specs;
                Spec.SelectedIndex = 0;

                db.Doctors.Load();
                WynikiWyszukiwania.ItemsSource = from d in db.Doctors.Local
                                                 select new { Name = d.User.Name, Specialization = d.Specialization, NextSlot = d.FirstFreeSlot() };
            }
           
        }
        private void LoggedChanges()
        {
            
            User.Content = "Witaj "+ ActuallyLogged.Name.ToString();
            Login.Content = "Wyloguj";
            Register.Visibility = Visibility.Collapsed;
            EdProf.Visibility = Visibility.Visible;
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {

           if(ActuallyLogged==null)
            {
                var zal = new Login();
                zal.ShowDialog();
                if (zal.GetResult())
                {
                    var a = new ApplicationDataFactory();
                    using (var db = a.CreateApplicationData())
                    {
                        string us = zal.GetUser();
                        string pas = zal.GetHashedPassword();
                        var usr = db.Users.Select(n => n).Where(p => p.PESEL == us && p.Password == pas);
                        if (usr.Count() != 0)
                        {
                            ActuallyLogged = usr.First();
                            LoggedChanges();
                        }     
                        else
                        {
                            MessageBox.Show("Błędny login lub hasło");
                            return;
                        }
                      
                    }
                }
            }
           else
            {
                ActuallyLogged = null;
                Login.Content = "Zaloguj";
                User.Content = "Witaj gościu!";
                Register.Visibility = Visibility.Visible;
                EdProf.Visibility = Visibility.Collapsed;
            }

        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var lekpac = new LekPac();
            lekpac.ShowDialog();
            if(lekpac.GetResult()!=0)
            {
                var a = new ApplicationDataFactory();
                var specs = new List<Specialization>();
                using (var db = a.CreateApplicationData())
                {
                   

                    specs.AddRange(db.Specializations);
                    specs.RemoveAt(1);
                 
                Register zar;
                if (lekpac.GetResult() == 1)
                    zar = new Register(specs);
                else
                    zar = new Register();

                zar.ShowDialog();
                if(zar.GetResult())
                {
                        string us ="";
                        string pas = "";
                        if (lekpac.GetResult()==1)
                        {
                            Specialization newspec = zar.GetSpec();
                            if (newspec != null)
                                db.AddSpecialization(newspec);

                            db.AddDoctor(zar.GetDoctor());
                            us = zar.GetDoctor().User.PESEL;
                            pas = zar.GetDoctor().User.Password;                
                        }
                     else
                        {
                            db.AddPatient(zar.GetPatient());
                            us = zar.GetPatient().User.PESEL;
                            pas = zar.GetPatient().User.Password;
                        }
                        var usr = db.Users.Select(n => n).Where(p => p.PESEL == us && p.Password == pas);
                        if (usr.Count() != 0)
                        {
                            ActuallyLogged = usr.First();
                            LoggedChanges();
                        }
                    }
                }
            }
            
        }

      

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                var a = new ApplicationDataFactory();
                using (var db = a.CreateApplicationData())
                {
                    db.Doctors.Load();
                    
                    Predicate<Doctor> isValid;
                    if (Spec.SelectedIndex > 0)
                    {
                        if (Nazwisko.Text == null)
                            isValid = (doc) => doc.Specialization.Name == Spec.SelectedItem.ToString();
                        else
                            isValid = (doc) => doc.Specialization.Name == Spec.SelectedItem.ToString() && doc.User.Name.ToString().ToLower().Contains(Nazwisko.Text.ToLower());
                    }
                    else if (Nazwisko.Text != null)
                        isValid = (doc) => doc.User.Name.ToString().ToLower().Contains(Nazwisko.Text.ToLower());
                    else return;

                    WynikiWyszukiwania.ItemsSource = from d in db.Doctors.Local
                                                     where isValid(d)
                                                     select new { Name = d.User.Name, Specialization = d.Specialization, NextSlot = d.FirstFreeSlot() };

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void EdProf_Click(object sender, RoutedEventArgs e)
        {
            var a = new ApplicationDataFactory();
            using (var db = a.CreateApplicationData())
            {
                Edit ed=new Edit();
                if (ActuallyLogged.Kind == DocOrPat.Doctor)
                {
                    var usr = db.Doctors.Select(n => n).Where(p => p.User.Key == ActuallyLogged.Key);
                    if(usr.Count()!=0)
                    {
                        var specs = new List<Specialization>();
                        specs.AddRange(db.Specializations);
                        specs.RemoveAt(1);
                        ed = new Edit(specs, usr.First());
                        
                    }
                    else
                    {
                        MessageBox.Show("non?");
                    }
                  
                }
                else
                {
                    var usr = db.Patients.Select(n => n).Where(p => p.User.Key == ActuallyLogged.Key);
                    if (usr.Count() != 0)
                    {
                        ed = new Edit(usr.First());
                    }
                        
                }
                ed.ShowDialog();
                if (ed.GetResult())
                {
                    if(ed.GetPatient()!=null)
                    {
                        db.UpdatePatient(ed.GetPatient());
                        
                    }
                    else
                    {
                        if (ed.GetSpec() != null)
                            db.AddSpecialization(ed.GetSpec());
                        db.UpdateDoctor(ed.GetDoctor());
                    }
                    var usr = db.Users.Select(n => n).Where(p => p.Key == ActuallyLogged.Key);
                    if (usr.Count() != 0)
                    {
                        ActuallyLogged = usr.First();
                        LoggedChanges();
                    }
                }

            }
            
        }
    }
}
