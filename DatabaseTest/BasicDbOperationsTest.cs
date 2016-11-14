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
            Doctor g = CreateDoctor();
            Database.Users.Add(g.User);
            this.Database.Doctors.Add(g);
            this.Database.SaveChangesOn();
            this.Database.DetachOn();

            var g2 = Database.Doctors.Find(g.Key);
            //g.Specialization = spec;
            //Asset:
            //Assert.IsTrue(g != g2);
            //Assert.IsTrue(g.User.IsDeepEqual(g2.User));
            Assert.IsTrue(g.IsDeepEqual(g2));


        }

        [TestMethod]
        public void CheckGetVisitTest()
        {
            Doctor a = CreateDoctor();
            var b = a.FirstFreeSlot;
            Patient c = CreatePatient();
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
            Patient p = CreatePatient();
            Doctor g = CreateDoctor();
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
