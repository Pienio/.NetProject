using DatabaseAccess;
using DatabaseAccess.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DatabaseAcessTest
{
    

  
        public abstract class DataTest
        {
            public IApplicationDataFactory DbFactory { get; set; }
            public ITransactionalApplicationData DataBase { get; set; }


            [TestInitialize]
            public void Initialize()
            {
                DataBase = new ApplicationData(true);

            }



            [TestCleanup]
            public void Cleanup()
            {
                this.DbFactory.Dispose();
            }


        


    }

}
