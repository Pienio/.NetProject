using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseAccess.Model;


namespace DatabaseAcessTest
{
    [TestClass]
    public class BasicTest: DataTest
    {
        [TestMethod]
        public void Specialization_BasicTest()
        {
            Specialization a = new Specialization() { Name = "Anastezjolog" };
            this.DataBase.Specializations.Add(a);

            this.DataBase.SaveChangesOn();            
            this.DataBase.DetachOn();

            var s2 = this.DataBase.Specializations.Find(a.Key);

            //Assert:
            Assert.IsTrue(a != s2);
           // Assert.IsTrue(compare.Compare(s, s2).AreEqual);
        }
    }
}
