using DatabaseAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DatabaseTest
{
    public abstract class DataTest
    {
        public ITransactionalApplicationData Database { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            var factory = new ApplicationDataFactory();
            Database = factory.CreateTransactionalApplicationData();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Database.Rollback();
            Database.Dispose();
        }
    }
}
