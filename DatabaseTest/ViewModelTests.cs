using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseAccess.Model;
using Visits.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DeepEqual.Syntax;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Visits.Services;

namespace DatabaseTest
{
    [TestClass]
    public class ViewModelTests:ViewModelTest
    {
        
        
        [TestMethod]
        public void WizListViewModelCheck()
        {


            Patient pat = CreatePatient();
            Doctor doc = CreateDoctor();
            db.Patients.Add(pat);
            db.Doctors.Add(doc);
            Visit c = new Visit();
            c.Doctor = doc;
            c.Patient = pat;
            c.Date = doc.FirstFreeSlot;
            db.Visits.Add(c);
            db.SaveChangesOn();
            asd.LogIn(pat.User.PESEL, pat.User.Password, db).Wait();
    
            var a = GetViewModel();

            IEnumerable<Visit> ec =  from v in db.Visits.Local
                                   where v.Patient.Key == asd.Logged.Key && (v.Date > DateTime.Now)
                                   select v;
            a.Invoke();
            var gb = a.Visits;
            
            Assert.IsTrue(gb.IsDeepEqual(ec));

            a.DeleteVisitCmd.Execute(a.Visits.First());
            a.Invoke();
            gb = a.Visits;
            ec= from v in db.Visits.Local
                where v.Patient.Key == asd.Logged.Key && (v.Date > DateTime.Now)
                select v;

            Assert.IsTrue(gb.IsDeepEqual(ec));

            asd.LogOut();
            asd.LogIn(doc.User.PESEL, doc.User.Password, db).Wait();
            ec = from v in db.Visits.Local
                                    where v.Doctor.Key == asd.Logged.Key && (v.Date > DateTime.Now)
                                    select v;
            a.Invoke();
            gb = a.Visits;

            Assert.IsTrue(gb.IsDeepEqual(ec));

        }
        [TestMethod]
        public void RegisterCheck()
        {
         //db.Commit();
            var a = GetRegModel();
            Patient b = CreatePatient();
            a.Who = false;
           // a.SetPatientTest(b);
            db.ToCommit = false;
            a.AddPatient(b,db).Wait();

            IEnumerable<Patient> ec = from v in db.Patients.Local
                                    where v.Key == b.Key
                                    select v;
            var f = ec.First();
            Assert.IsTrue(b.IsDeepEqual(f));
           



        }
        [TestMethod]
        public void SearchTest()
        {
            for(int i=0;i<1000;i++)
            {
                Doctor a = CreateDoctor();
                a.User.Name.Name = i.ToString();
                db.Doctors.Add(a);
            }

            var d = GetMWModel();
            db.SaveChangesOn();
            var c = db.Specializations.First();
            var s=DateTime.Now;
            d.SearchByNameText = "Jan";
            d.SearchCmd.Execute(null);
            var p = DateTime.Now - s;
            Assert.IsTrue(p.TotalSeconds <= 3);



        }
        

        private WizListViewModel GetViewModel()
        {


            return new WizListViewModel(asd, Fac);
        }
        private RegisterViewModel GetRegModel()
        {


            return new RegisterViewModel(asd, Fac);
        }
        private MainWindowViewModel GetMWModel()
        {


            return new MainWindowViewModel(asd, Fac);
        }
        private AddSpecViewModel GetAddSpecModel()
        {


            return new AddSpecViewModel(asd, Fac);
        }
    }
}
    


        



