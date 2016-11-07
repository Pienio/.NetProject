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
using Visits.Validations;
using System.Security;

namespace Visits
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        private bool result=false;
        private ValPac NewPatient = null;
        private ValDoc NewDoctor=null;
        private Specialization NewSpec = null;
        private List<Specialization> SpecList;
        public static readonly DependencyProperty SecurePasswordProperty =
        DependencyProperty.RegisterAttached("Pas", typeof(string), typeof(Register));
        public static readonly DependencyProperty SecurePasswordProperty1 =
        DependencyProperty.RegisterAttached("Pasp", typeof(string), typeof(Register));

        public Register()
        {
            InitializeComponent();
            DoForPatient();
            

        }
        public Register(List<Specialization> a)
        {
            InitializeComponent();
            //this.Loaded += delegate { AddTriggers(); };
            SpecList = a;
            Spec.ItemsSource = a;
            Spec.SelectedIndex = 0;
            DoForDoctor();



        }
        private void DoForPatient()
        {
            label4.Visibility = Visibility.Collapsed;
            Spec.Visibility = Visibility.Collapsed;
            GP.Visibility = Visibility.Collapsed;
            AddSpec.Visibility = Visibility.Collapsed;
            NewPatient = new ValPac();
            Bind("Pesel", Pes,true);
            Bind("FirstName", Imi, true);
            Bind("LastName", Nazw, true);
             BindPassword();



        }
        private void BindPassword()
        {
            if (NewPatient != null)
                DataContext = NewPatient;
            else
                DataContext = NewDoctor;// created somewhere

            // create a binding by code
            Binding passwordBinding = new Binding(SecurePasswordProperty.Name);
            if (NewPatient != null)
                passwordBinding.Source = NewPatient;
            else
                passwordBinding.Source = NewDoctor;      
            passwordBinding.ValidatesOnDataErrors = true;
            passwordBinding.Mode = BindingMode.TwoWay;
            passwordBinding.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            passwordBox.SetBinding(SecurePasswordProperty, passwordBinding);
            
            Binding passwordBinding1 = new Binding(SecurePasswordProperty1.Name);
            if (NewPatient != null)
                passwordBinding1.Source = NewPatient;
            else
                passwordBinding1.Source = NewDoctor;
            passwordBinding1.ValidatesOnDataErrors = true;
            passwordBinding1.Mode = BindingMode.TwoWay;
            passwordBinding1.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            passwordBox1.SetBinding(SecurePasswordProperty1, passwordBinding1);
            
        }
        private void Bind(string name,DependencyObject con,bool d)
        {
            Binding myBinding = new Binding();
            if (d)
                myBinding.Source = NewPatient;
            else
                myBinding.Source = NewDoctor;
            myBinding.Path = new PropertyPath(name);
            myBinding.Mode = BindingMode.TwoWay;
            myBinding.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            myBinding.ValidatesOnDataErrors = true;
            BindingOperations.SetBinding(con, TextBox.TextProperty, myBinding);
        }
        private void BindValRul(string name, DependencyObject con, bool d)
        {
            Binding myBinding = new Binding();
            if (d)
                myBinding.Source = NewPatient;
            else
                myBinding.Source = NewDoctor;
            myBinding.Path = new PropertyPath(name);
            myBinding.Mode = BindingMode.TwoWay;
            myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            IntegerValidation abc = new IntegerValidation() { MaxValue = 24, MinValue = 0 };
            myBinding.ValidationRules.Add(abc);
            BindingOperations.SetBinding(con, TextBox.TextProperty, myBinding);
            

        }
        private void DoForDoctor()
        {
            NewDoctor = new ValDoc();
            Bind("Pesel", Pes, false);
            Bind("FirstName", Imi, false);
            Bind("LastName", Nazw, false);
            
            BindPassword();
            //AddTriggers();
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
        private void AddTriggers()
        {
           
            DataTrigger trigger = new DataTrigger();
            trigger.Value = false;
            trigger.Binding = new Binding() { ElementName = "PS", Path = new PropertyPath(Validation.HasErrorProperty) };
            Setter setter = new Setter();
            setter.Property = Button.IsEnabledProperty;
            setter.Value = true;
            trigger.Setters.Add(setter);
            Reg.Style.Triggers.Add(trigger);
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
            
            this.Close();
        }
        public Patient GetPatient()
        {
            return NewPatient.GetPat();
        }
        public Doctor GetDoctor()
        { 
            return NewDoctor.GetDoc();
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
        private void MyPassword_Changed(object sender, RoutedEventArgs e)
        {
            // this should trigger binding and therefore validation
            if(NewPatient!=null)
            {
                ((ValPac)DataContext).Pas = passwordBox.Password;
                RoutedEventArgs newEventArgs = new RoutedEventArgs(PasswordBox.PasswordChangedEvent);

                passwordBox1.RaiseEvent(newEventArgs);
            } 
            else
            {
                ((ValDoc)DataContext).Pas = passwordBox.Password;
                RoutedEventArgs newEventArgs = new RoutedEventArgs(PasswordBox.PasswordChangedEvent);
                passwordBox1.RaiseEvent(newEventArgs);
            }
                
        }
        private void MyPassword_Changed1(object sender, RoutedEventArgs e)
        {
            if (NewPatient != null)
            {
                ((ValPac)DataContext).Pasp = passwordBox1.Password;
            }

            else
            {
                ((ValDoc)DataContext).Pasp = passwordBox1.Password;
            }
        }
    }
}
