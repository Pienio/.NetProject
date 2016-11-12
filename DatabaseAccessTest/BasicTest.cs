using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseAccess.Model;

namespace DatabaseAccessTest
{
    [TestClass]
    public class BasicTest: DataTest
    {
        [TestMethod]
        public void Specialization_BasicTest()
        {
            Specialization a = new Specialization { Name = "Anastezjolog" };
          

        }
    }
        
    
}
