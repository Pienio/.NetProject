using DatabaseAccess;
using DatabaseAccess.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visits.Services;
using System.Data.Entity;
using System.IO;

namespace DatabaseTest
{
    public abstract class ViewModelTest
    {
        public ITransactionalApplicationData db { get; set; }
        public ApplicationDataFactory Fac {
            get; set; }
        public ILogUserService asd
        {
            get; set;
        } 

        [TestInitialize]
        public void Initialize()
        {
           // File.Delete("Wizyty.sqlite");
            Fac = new ApplicationDataFactory();
            asd = new LogUserService();
            db = Fac.CreateTransactionalApplicationData();

            


        }

        [TestCleanup]
        public void Cleanup()
        {
            db.ToCommit = true;
            db.Rollback();
            db.Dispose();
        }
        protected Doctor CreateDoctor()
        {
            var spec = new Specialization("Okulista");
            Doctor g = new Doctor() { User = new User() };
            g.Specialization = spec;
            g.User.Name.Name = "Jan";
            g.User.Name.Surname = "Janowski";
            g.User.PESEL = "77777777777";
            g.User.Kind = DocOrPat.Doctor;
            g.User.Password = "96e79218965eb72c92a549dd5a330112";
            g.MondayWorkingTime = new WorkingTime() { Start = 8, End = 12 };
            g.TuesdayWorkingTime = new WorkingTime() { Start = 8, End = 12 };
            g.WednesdayWorkingTime = new WorkingTime() { Start = 8, End = 12 };
            g.ThursdayWorkingTime = new WorkingTime() { Start = 8, End = 12 };
            g.FridayWorkingTime = new WorkingTime() { Start = 8, End = 12 };
            return g;
        }

        protected Patient CreatePatient()
        {
            Patient p = new Patient();
            p.User = new User()
            {
                Name = new PersonName() { Name = "a", Surname = "b" },
                Kind = DocOrPat.Patient,
                Password = "96e79218965eb72c92a549dd5a330112",
                PESEL = "12345678901"
            };
            return p;
        }
    }
    }
