using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseAccess.Model;


namespace DatabaseTest
{
    [TestClass]
    public class BasicTest: DataTest
    {
        [TestMethod]
        public void Specialization_BasicTest()
        {
            Specialization a = new Specialization() { Name = "Anastezjolog" };
            this.Database.Specializations.Add(a);

            this.Database.SaveChangesOn();            
            this.Database.DetachOn();

            var s2 = Database.Specializations.Find(a.Key);

            //Assert:
            Assert.IsTrue(a != s2);
           // Assert.IsTrue(compare.Compare(s, s2).AreEqual);
        }
    }
}
