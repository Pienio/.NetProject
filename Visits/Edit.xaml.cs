using DatabaseAccess;
using DatabaseAccess.Model;
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
using Visits.Validations;

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
        ValPac actualpatient;
        ValDoc actualdoctor;

        public Edit()
        {
            InitializeComponent();
          
        }
        public Edit(Patient a)
        {
            InitializeComponent();
            actualpatient =new ValPac();
            actualpatient.SetPac(a);
            label4.Visibility = Visibility.Collapsed;
            Spec.Visibility = Visibility.Collapsed;
            GP.Visibility = Visibility.Collapsed;
            AddSpec.Visibility = Visibility.Collapsed;
            DoForPatient();
            password = a.User.Password;
            // LoadUser();
        }
        public Edit(List<Specialization> a,Doctor b)
        {
            InitializeComponent();
            SpecList = a;
            Spec.ItemsSource = a;
            Spec.SelectedIndex = 0;
            actualdoctor =new ValDoc ();
            actualdoctor.SetDoc(b);
            DoForDoctor();
            password = b.User.Password;
           // LoadUser();
        }
        //private void LoadUser()
        //{
        //    if (actualpatient != null)
        //    {
        //        ////Pes.Text = actualpatient.User.PESEL;
        //        ////Imi.Text = actualpatient.User.Name.Name;
        //        ////Nazw.Text = actualpatient.User.Name.Surname;
        //        ////password = actualpatient.User.Password;

        //    }
        //    else
        //    {
        //        Pes.Text = actualdoctor.GetDoc().User.PESEL;
        //        //Imi.Text = actualdoctor.User.Name.Name;
        //        //Nazw.Text = actualdoctor.User.Name.Surname;
        //        //password = actualdoctor.User.Password;
        //        //Spec.SelectedIndex = SpecList.FindIndex(p => p.Name == actualdoctor.Specialization.Name);
        //        //PS.Text = actualdoctor.MondayWorkingTime.Start.ToString();
        //        //PE.Text = actualdoctor.MondayWorkingTime.End.ToString();
        //        //WS.Text = actualdoctor.TuesdayWorkingTime.Start.ToString();
        //        //WE.Text = actualdoctor.TuesdayWorkingTime.End.ToString();
        //        //SS.Text = actualdoctor.WednesdayWorkingTime.Start.ToString();
        //        //SE.Text = actualdoctor.WednesdayWorkingTime.End.ToString();
        //        //CS.Text = actualdoctor.ThursdayWorkingTime.Start.ToString();
        //        //CE.Text = actualdoctor.ThursdayWorkingTime.End.ToString();
        //        //PIS.Text = actualdoctor.FridayWorkingTime.Start.ToString();
        //        //PIE.Text = actualdoctor.FridayWorkingTime.End.ToString();
        //    }
        //}
        private void DoForPatient()
        {
            label4.Visibility = Visibility.Collapsed;
            Spec.Visibility = Visibility.Collapsed;
            GP.Visibility = Visibility.Collapsed;
            AddSpec.Visibility = Visibility.Collapsed;
           
            Bind("Pesel", Pes, true);
            Bind("FirstName", Imi, true);
            Bind("LastName", Nazw, true);
            //BindPassword();



        }
        //private void BindPassword()
        //{
        //    if (actualpatient != null)
        //        DataContext = actualpatient;
        //    else
        //        DataContext = actualdoctor;// created somewhere

        //    // create a binding by code
        //    Binding passwordBinding = new Binding(SecurePasswordProperty.Name);
        //    if (actualpatient != null)
        //        passwordBinding.Source =actualpatient;
        //    else
        //        passwordBinding.Source =actualdoctor;
        //    passwordBinding.ValidatesOnDataErrors = true;
        //    passwordBinding.Mode = BindingMode.TwoWay;
        //    passwordBinding.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
        //    passwordBox.SetBinding(SecurePasswordProperty, passwordBinding);

        //    Binding passwordBinding1 = new Binding(SecurePasswordProperty1.Name);
        //    if (NewPatient != null)
        //        passwordBinding1.Source = NewPatient;
        //    else
        //        passwordBinding1.Source = NewDoctor;
        //    passwordBinding1.ValidatesOnDataErrors = true;
        //    passwordBinding1.Mode = BindingMode.TwoWay;
        //    passwordBinding1.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
        //    passwordBox1.SetBinding(SecurePasswordProperty1, passwordBinding1);

        //}
        private void Bind(string name, DependencyObject con, bool d)
        {
            Binding myBinding = new Binding();
            if (d)
                myBinding.Source = actualpatient;
            else
                myBinding.Source = actualdoctor;
            myBinding.Path = new PropertyPath(name);
            myBinding.Mode = BindingMode.TwoWay;
            myBinding.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            myBinding.ValidatesOnDataErrors = true;
            BindingOperations.SetBinding(con, TextBox.TextProperty, myBinding);
        }
        private void BindValRul(string name, DependencyObject con, bool d)
        {
            Binding myBinding = new Binding();
            myBinding.Source = actualdoctor;
            myBinding.Path = new PropertyPath(name);
            myBinding.Mode = BindingMode.TwoWay;
            myBinding.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            IntegerValidation abc = new IntegerValidation() { MaxValue = 24, MinValue = 0 };
            myBinding.ValidatesOnDataErrors = true;
            myBinding.ValidationRules.Add(abc);
            BindingOperations.SetBinding(con, TextBox.TextProperty, myBinding);


        }
        private void DoForDoctor()
        {
           
            Bind("Pesel", Pes, false);
            Bind("FirstName", Imi, false);
            Bind("LastName", Nazw, false);

            //BindPassword();
            BindValRul("PS", PS, false);
            BindValRul("PE", PE, false);
            BindValRul("WS", WS, false);
            BindValRul("WE", WE, false);
            BindValRul("SS", SS, false);
            BindValRul("SE", SE, false);
            BindValRul("CS", CS, false);
            BindValRul("CE", CE, false);
            BindValRul("PIS", PIS, false);
            BindValRul("PIE", PIE, false);

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
                //if (!label4.IsVisible)
                //{
                //    actualpatient.User.Name.Name = Imi.Text;
                //    actualpatient.User.Name.Surname = Nazw.Text;
                //    actualpatient.User.PESEL = Pes.Text;

                //}
                //else
                //{
                //    actualdoctor.User.Name.Surname = Nazw.Text;
                //    actualdoctor.User.PESEL = Pes.Text;

                //    actualdoctor.User.Name.Name = Imi.Text;
                //    actualdoctor.Specialization = (Specialization)Spec.SelectedItem;
                //    actualdoctor.MondayWorkingTime = new WorkingTime();
                //    actualdoctor.MondayWorkingTime.Start = Int32.Parse(PS.Text);
                //    actualdoctor.MondayWorkingTime.End = Int32.Parse(PE.Text);
                //    actualdoctor.TuesdayWorkingTime = new WorkingTime();
                //    actualdoctor.TuesdayWorkingTime.Start = Int32.Parse(WS.Text);
                //    actualdoctor.TuesdayWorkingTime.End = Int32.Parse(WE.Text);
                //    actualdoctor.WednesdayWorkingTime = new WorkingTime();
                //    actualdoctor.WednesdayWorkingTime.Start = Int32.Parse(SS.Text);
                //    actualdoctor.WednesdayWorkingTime.End = Int32.Parse(SE.Text);
                //    actualdoctor.ThursdayWorkingTime = new WorkingTime();
                //    actualdoctor.ThursdayWorkingTime.Start = Int32.Parse(CS.Text);
                //    actualdoctor.ThursdayWorkingTime.End = Int32.Parse(CE.Text);
                //    actualdoctor.FridayWorkingTime = new WorkingTime();
                //    actualdoctor.FridayWorkingTime.Start = Int32.Parse(PIS.Text);
                //    actualdoctor.FridayWorkingTime.End = Int32.Parse(PIE.Text);

                //}
                this.Close();
            }
            else
            {
                MessageBox.Show("Złe hasło");
            }
           
        }
        public Patient GetPatient()
        {
            if (actualpatient == null)
                return null;
            return actualpatient.GetPat();
        }
        public Doctor GetDoctor()
        {
            if (actualdoctor == null)
                return null;
            return actualdoctor.GetDoc();
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
                    actualdoctor.GetDoc().User.Password = chp.GetPassword();
                else
                    actualpatient.GetPat().User.Password = chp.GetPassword();

                MessageBox.Show("Zmieniono hasło z powodzeniem");//password + "    " + chp.GetPassword());
            }
           
        }
    }
}
