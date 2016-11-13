using System;
using DeepEqual;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseAccess.Model;
using DeepEqual.Syntax;

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
            var spec = new Specialization();
            spec.Name = "Okulista";
            Doctor g = new Doctor() { User = new User()};
            g.Specialization = spec;
            g.User.Name = new PersonName();
            g.User.Name.Name = "Jan";
            g.User.Name.Surname = "Janowski";
            g.User.PESEL = "77777777777";
            g.User.Kind = DocOrPat.Doctor;
            g.User.Password = "96e79218965eb72c92a549dd5a330112";
            g.MondayWorkingTime = new WorkingTime();
            g.MondayWorkingTime.Start = 8 ;
            g.MondayWorkingTime.End = 12;
            g.TuesdayWorkingTime = new WorkingTime();
            g.TuesdayWorkingTime.Start = 8 ;
            g.TuesdayWorkingTime.End = 12 ;
            g.WednesdayWorkingTime = new WorkingTime();
            g.WednesdayWorkingTime.Start = 8 ;
            g.WednesdayWorkingTime.End = 12 ;
            g.ThursdayWorkingTime = new WorkingTime();
            g.ThursdayWorkingTime.Start = 8 ;
            g.ThursdayWorkingTime.End = 12 ;
            g.FridayWorkingTime = new WorkingTime();
            g.FridayWorkingTime.Start = 8 ;
            g.FridayWorkingTime.End = 12 ;
            this.Database.Doctors.Add(g);
            this.Database.SaveChangesOn();
            this.Database.DetachOn();
            
            var g2 = Database.Doctors.Find(g.Key);
            
            //Asset:
            Assert.IsTrue(g != g2);
            //Assert.IsTrue(g.User.IsDeepEqual(g2.User));
            Assert.IsTrue(g.IsDeepEqual(g2));


        }
    }
}
