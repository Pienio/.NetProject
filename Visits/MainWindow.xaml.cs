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

namespace Visits
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //To tylko na potrzeby testów
            //try
            //{
            var a = new ApplicationDataFactory();
            using (var db = a.CreateApplicationData())
            {
                db.Fill();
                //    foreach (var c in db.ShowSpec())
                //    {
                //        MessageBox.Show(c.Name);
                //    }
                //    foreach (var c in db.ShowDoc())
                //    {
                //        MessageBox.Show(c.User.Name.Name + " " + c.User.Name.Surname + " ");//+c.MondayWorkingTime.Start.ToString()+" "+c.MondayWorkingTime.End.ToString());
                //    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}

                var specs = new List<Specialization>();
                specs.Add(new Specialization("- brak -"));
                specs.AddRange(db.Specializations);
                Spec.ItemsSource = specs;
                Spec.SelectedIndex = 0;

                db.Doctors.Load();
                WynikiWyszukiwania.ItemsSource = from d in db.Doctors.Local
                                                 select new { Name = d.User.Name, Specialization = d.Specialization, NextSlot = d.FirstFreeSlot() };
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var zal = new Login();
            zal.Show();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var zar = new Register();
            zar.Show();
        }

        private void ZW_Click(object sender, RoutedEventArgs e)
        {
            var regwiz = new RegVisit();
            regwiz.Show();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
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
    }
}
