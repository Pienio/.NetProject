using System;
using DeepEqual;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseAccess.Model;
using DeepEqual.Syntax;
using System.Linq;

namespace DatabaseTest
{
    [TestClass]
    public class BasicDbOperationsTest : DataTest
    {
        [TestMethod]
        public void AddTest()
        {
            Database.BeginTransaction();
            //Act:
            Specialization s1 = new Specialization() { Name = "Anastezjolog" };
            this.Database.Specializations.Add(s1);

            this.Database.SaveChangesOn();
            this.Database.DetachOn();

            var s2 = Database.Specializations.Find(s1.Key);

            //Assert:
            Assert.IsTrue(s1 != s2);
            Assert.IsTrue(s1.IsDeepEqual(s2));


        }

        [TestMethod]
        public void UpdateTest()
        {
            Database.BeginTransaction();
            //Act:
            Specialization s1 = new Specialization() { Name = "Anastezjolog" };
            this.Database.Specializations.Add(s1);
            this.Database.SaveChangesOn();
            this.Database.DetachOn();

            s1 = Database.Specializations.Find(s1.Key);
            s1.Name = "abcd";
            this.Database.SaveChangesOn();
            this.Database.DetachOn();

            var s2 = Database.Specializations.Find(s1.Key);

            //Assert:
            Assert.IsTrue(s1 != s2);
            Assert.IsTrue(s1.IsDeepEqual(s2));
        }

        [TestMethod]
        public void DeleteTest()
        {
            Database.BeginTransaction();
            //Act:
            Specialization s1 = new Specialization() { Name = "Anastezjolog" };
            this.Database.Specializations.Add(s1);
            this.Database.SaveChangesOn();
            this.Database.DetachOn();

            s1 = Database.Specializations.Find(s1.Key);
            Database.Specializations.Remove(s1);
            this.Database.SaveChangesOn();
            this.Database.DetachOn();

            Assert.IsTrue(Database.Specializations.Find(s1.Key) == null);
        }

        [TestMethod]
        public void AddJoinTest()
        {
            Database.BeginTransaction();
            var spec = Database.Specializations.Find(1);
            //spec.Name = "Okulista";
            Doctor g = new Doctor() { User = new User() };
            g.Specialization = spec;
            g.User.Name = new PersonName();
            g.User.Name.Name = "Jan";
            g.User.Name.Surname = "Janowski";
            g.User.PESEL = "77777777777";
            g.User.Kind = DocOrPat.Doctor;
            g.User.Password = "96e79218965eb72c92a549dd5a330112";
            g.MondayWorkingTime = new WorkingTime();
            g.MondayWorkingTime.Start = 8;
            g.MondayWorkingTime.End = 12;
            g.TuesdayWorkingTime = new WorkingTime();
            g.TuesdayWorkingTime.Start = 8;
            g.TuesdayWorkingTime.End = 12;
            g.WednesdayWorkingTime = new WorkingTime();
            g.WednesdayWorkingTime.Start = 8;
            g.WednesdayWorkingTime.End = 12;
            g.ThursdayWorkingTime = new WorkingTime();
            g.ThursdayWorkingTime.Start = 8;
            g.ThursdayWorkingTime.End = 12;
            g.FridayWorkingTime = new WorkingTime();
            g.FridayWorkingTime.Start = 8;
            g.FridayWorkingTime.End = 12;
            this.Database.Doctors.Add(g);
            this.Database.SaveChangesOn();
            this.Database.DetachOn();

            var g2 = Database.Doctors.Find(g.Key);
            //g.Specialization = spec;
            //Asset:
            Assert.IsTrue(g != g2);
            //Assert.IsTrue(g.User.IsDeepEqual(g2.User));
            Assert.IsTrue(g.IsDeepEqual(g2));


        }

        [TestMethod]
        public void CheckGetVisitTest()
        {
            
            Database.BeginTransaction();
            Doctor a = this.Database.Doctors.Find(1);
            var b = a.FirstFreeSlot;
            Patient c = this.Database.Patients.Find(1);
            Visit vis = new Visit();
            vis.Patient = c;
            vis.Doctor = a;
            vis.Date = b;

            this.Database.Visits.Add(vis);
            this.Database.SaveChangesOn();
            this.Database.DetachOn();
            Visit vis1 = this.Database.Visits.Find(vis.Key);

            //Check add
            Assert.IsTrue(vis != vis1);
            Assert.IsTrue(vis.IsDeepEqual(vis1));

            Visit vis2 = new Visit();
            vis2.Patient = c;
            vis2.Doctor = a;
            vis2.Date = a.FirstFreeSlot;
            Assert.IsTrue(vis2.Date != vis1.Date);

            this.Database.Visits.Add(vis2);
            this.Database.SaveChangesOn();
            this.Database.DetachOn();
            Visit vis3 = this.Database.Visits.Find(vis2.Key);

            //Check add
            Assert.IsTrue(vis2 != vis3);
            Assert.IsTrue(vis2.IsDeepEqual(vis3));
            Assert.IsTrue(vis2.Date!=vis.Date);
        }


        [TestMethod]
        public void ListTest()
        {
            Database.BeginTransaction();
            Patient p = new Patient();
            p.User = new User()
            {
                Name = new PersonName() { Name = "F", Surname = "M" },
                Kind = DocOrPat.Patient,
                Password = "96e79218965eb72c92a549dd5a330112",
                PESEL = "95122907757"
            };

            var spec = new Specialization();
            spec.Name = "Okulista";
            Doctor g = new Doctor() { User = new User() };
            g.Specialization = spec;
            g.User.Name = new PersonName();
            g.User.Name.Name = "Jan";
            g.User.Name.Surname = "Janowski";
            g.User.PESEL = "77777777777";
            g.User.Kind = DocOrPat.Doctor;
            g.User.Password = "96e79218965eb72c92a549dd5a330112";
            g.MondayWorkingTime = new WorkingTime();
            g.MondayWorkingTime.Start = 8;
            g.MondayWorkingTime.End = 12;
            g.TuesdayWorkingTime = new WorkingTime();
            g.TuesdayWorkingTime.Start = 8;
            g.TuesdayWorkingTime.End = 12;
            g.WednesdayWorkingTime = new WorkingTime();
            g.WednesdayWorkingTime.Start = 8;
            g.WednesdayWorkingTime.End = 12;
            g.ThursdayWorkingTime = new WorkingTime();
            g.ThursdayWorkingTime.Start = 8;
            g.ThursdayWorkingTime.End = 12;
            g.FridayWorkingTime = new WorkingTime();
            g.FridayWorkingTime.Start = 8;
            g.FridayWorkingTime.End = 12;

            Visit v = new Visit(p, g, g.FirstFreeSlot);
            p.Visits.Add(v);
            //Database.Users.Add(p.User);
            //Database.Users.Add(g.User);
            Database.Patients.Add(p);
            Database.Doctors.Add(g);

            Database.SaveChangesOn();
            Database.DetachOn();

            //Assert:
            Assert.IsTrue(Database.Visits.Any());
            var v1 = Database.Visits.OrderByDescending(vi => vi.Key).First();
            bool pr = v1.IsDeepEqual(v);
            Assert.IsTrue(pr);
            Assert.IsTrue(v1.Doctor.IsDeepEqual(g), "v.D != first.D");
            Assert.IsTrue(v1.Patient.IsDeepEqual(p), "v.P != first.P");
        }
    }
}
