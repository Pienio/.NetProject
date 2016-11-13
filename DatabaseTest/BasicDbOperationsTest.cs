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
    }
}
