using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace Visits
{
    /// <summary>
    /// Interaction logic for Edit.xaml
    /// </summary>
    public partial class Edit : Window
    {
        private bool result = false;
        private Specialization NewSpec = null;
        private string password;
        private List<Specialization> SpecList;
        Patient actualpatient;
        Doctor actualdoctor;
        public Edit()
        {
            InitializeComponent();
          
        }
        public Edit(Patient a)
        {
            InitializeComponent();
            actualpatient = a;
            label4.Visibility = Visibility.Collapsed;
            Spec.Visibility = Visibility.Collapsed;
            GP.Visibility = Visibility.Collapsed;
            AddSpec.Visibility = Visibility.Collapsed;
            LoadUser();
        }
        public Edit(List<Specialization> a,Doctor b)
        {
            InitializeComponent();
            SpecList = a;
            Spec.ItemsSource = a;
            Spec.SelectedIndex = 0;
            actualdoctor = b;
            LoadUser();
        }
        private void LoadUser()
        {
            if(actualpatient!=null)
            {
                Pes.Text = actualpatient.User.PESEL;
                Imi.Text = actualpatient.User.Name.Name;
                Nazw.Text = actualpatient.User.Name.Surname;
                password = actualpatient.User.Password;
                
            }
            else
            {
                Pes.Text = actualdoctor.User.PESEL;
                Imi.Text = actualdoctor.User.Name.Name;
                Nazw.Text = actualdoctor.User.Name.Surname;
                password = actualdoctor.User.Password;
                Spec.SelectedIndex = SpecList.FindIndex(p => p.Name == actualdoctor.Specialization.Name);
                PS.Text =actualdoctor.MondayWorkingTime.Start.ToString();
                PE.Text = actualdoctor.MondayWorkingTime.End.ToString();
                WS.Text = actualdoctor.TuesdayWorkingTime.Start.ToString();
                WE.Text = actualdoctor.TuesdayWorkingTime.End.ToString();
                SS.Text = actualdoctor.WednesdayWorkingTime.Start.ToString();
                SE.Text = actualdoctor.WednesdayWorkingTime.End.ToString();
                CS.Text = actualdoctor.ThursdayWorkingTime.Start.ToString();
                CE.Text = actualdoctor.ThursdayWorkingTime.End.ToString();
                PIS.Text = actualdoctor.FridayWorkingTime.Start.ToString();
                PIE.Text = actualdoctor.FridayWorkingTime.End.ToString();
            }
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
            if(password==HashPassword(Haslo.Password))
            {
                result = true;
                if (!label4.IsVisible)
                {
                    actualpatient.User.Name.Name = Imi.Text;
                    actualpatient.User.Name.Surname = Nazw.Text;
                    actualpatient.User.PESEL = Pes.Text;

                }
                else
                {
                    actualdoctor.User.Name.Surname = Nazw.Text;
                    actualdoctor.User.PESEL = Pes.Text;

                    actualdoctor.User.Name.Name = Imi.Text;
                    actualdoctor.Specialization = (Specialization)Spec.SelectedItem;
                    actualdoctor.MondayWorkingTime = new WorkingTime();
                    actualdoctor.MondayWorkingTime.Start = Int32.Parse(PS.Text);
                    actualdoctor.MondayWorkingTime.End = Int32.Parse(PE.Text);
                    actualdoctor.TuesdayWorkingTime = new WorkingTime();
                    actualdoctor.TuesdayWorkingTime.Start = Int32.Parse(WS.Text);
                    actualdoctor.TuesdayWorkingTime.End = Int32.Parse(WE.Text);
                    actualdoctor.WednesdayWorkingTime = new WorkingTime();
                    actualdoctor.WednesdayWorkingTime.Start = Int32.Parse(SS.Text);
                    actualdoctor.WednesdayWorkingTime.End = Int32.Parse(SE.Text);
                    actualdoctor.ThursdayWorkingTime = new WorkingTime();
                    actualdoctor.ThursdayWorkingTime.Start = Int32.Parse(CS.Text);
                    actualdoctor.ThursdayWorkingTime.End = Int32.Parse(CE.Text);
                    actualdoctor.FridayWorkingTime = new WorkingTime();
                    actualdoctor.FridayWorkingTime.Start = Int32.Parse(PIS.Text);
                    actualdoctor.FridayWorkingTime.End = Int32.Parse(PIE.Text);

                }
                this.Close();
            }
           
        }
        public Patient GetPatient()
        {
            return actualpatient;
        }
        public Doctor GetDoctor()
        {
            return actualdoctor;
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var chp = new ChangePass(password);
            chp.ShowDialog();
            if (chp.GetResult())
            {
                password = chp.GetPassword();
                
                if (actualdoctor != null)
                    actualdoctor.User.Password = chp.GetPassword();
                else
                    actualpatient.User.Password = chp.GetPassword();

                MessageBox.Show("Zmieniono hasło z powodzeniem");//password + "    " + chp.GetPassword());
            }
           
        }
    }
}
