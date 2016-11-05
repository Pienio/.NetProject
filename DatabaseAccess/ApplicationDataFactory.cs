using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    public class ApplicationDataFactory : IApplicationDataFactory
    { 
        public IApplicationData CreateApplicationData()
        {
            return new ApplicationData();
        }
    }
}
