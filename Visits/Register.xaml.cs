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
using System.Windows.Shapes;
using DatabaseAccess;
using System.Security.Cryptography;

namespace Visits
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        private bool result=false;
        private Patient NewPatient = null;
        private Doctor NewDoctor=null;
        private Specialization NewSpec = null;
        private List<Specialization> SpecList;
        public Register()
        {
            InitializeComponent();
            label4.Visibility = Visibility.Collapsed;
            Spec.Visibility = Visibility.Collapsed;
            GP.Visibility = Visibility.Collapsed;
            AddSpec.Visibility = Visibility.Collapsed;
        }
        public Register(List<Specialization> a)
        {
            InitializeComponent();
            SpecList = a;
            Spec.ItemsSource = a;
            Spec.SelectedIndex = 0;


        }
        public bool GetResult()
        {
            return result;
        }
        public Specialization GetSpec()
        {
            return NewSpec;
        }
        private void Anul_Click(object sender, RoutedEventArgs e)
        {
            result = false;
            this.Close();
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            result = true;
            if(!label4.IsVisible)
            {
                NewPatient = new Patient();
                NewPatient.User = new User() { Name = new PersonName() };
                NewPatient.User.Name.Name = Imi.Text;
                NewPatient.User.Name.Surname = Nazw.Text;
                NewPatient.User.PESEL = Pes.Text;
                NewPatient.User.Password = HashPassword(passwordBox.Password);
                NewPatient.User.Kind = DocOrPat.Patient;

            }
            else
            {
                NewDoctor = new Doctor();
                NewDoctor.User = new User() { Name = new PersonName() };
                NewDoctor.User.Name.Name = Imi.Text;
                NewDoctor.User.Name.Surname = Nazw.Text;
                NewDoctor.User.PESEL = Pes.Text;
                NewDoctor.User.Password = HashPassword(passwordBox.Password);
                NewDoctor.User.Kind = DocOrPat.Doctor;
                NewDoctor.Specialization =(Specialization)Spec.SelectedItem;
                NewDoctor.MondayWorkingTime = new WorkingTime();
                NewDoctor.MondayWorkingTime.Start = Int32.Parse(PS.Text);
                NewDoctor.MondayWorkingTime.End = Int32.Parse(PE.Text);
                NewDoctor.TuesdayWorkingTime = new WorkingTime();
                NewDoctor.TuesdayWorkingTime.Start = Int32.Parse(WS.Text);
                NewDoctor.TuesdayWorkingTime.End = Int32.Parse(WE.Text);
                NewDoctor.WednesdayWorkingTime = new WorkingTime();
                NewDoctor.WednesdayWorkingTime.Start = Int32.Parse(SS.Text);
                NewDoctor.WednesdayWorkingTime.End = Int32.Parse(SE.Text);
                NewDoctor.ThursdayWorkingTime = new WorkingTime();
                NewDoctor.ThursdayWorkingTime.Start = Int32.Parse(CS.Text);
                NewDoctor.ThursdayWorkingTime.End = Int32.Parse(CE.Text);
                NewDoctor.FridayWorkingTime = new WorkingTime();
                NewDoctor.FridayWorkingTime.Start = Int32.Parse(PIS.Text);
                NewDoctor.FridayWorkingTime.End = Int32.Parse(PIE.Text);

            }
            this.Close();
        }
        public Patient GetPatient()
        {
            return NewPatient;
        }
        public Doctor GetDoctor()
        { 
            return NewDoctor;
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

        private void AddSpec_Click(object sender, RoutedEventArgs e)
        {
            var zar = new AddSpec();
            zar.ShowDialog();
            if (zar.GetResult())
            {
                NewSpec = new Specialization() { Name = zar.GetName() };
                SpecList.Add(NewSpec);
                Spec.ItemsSource = SpecList;

            }
        }
    }
}
