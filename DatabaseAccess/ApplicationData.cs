using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    class ApplicationData : DbContext, IApplicationData
    {
        public IDbSet<Doctor> Doctors { get; set; }
        public IDbSet<Doctor> Patients { get; set; }
        public IDbSet<Doctor> Specializations { get; set; }
        public IDbSet<Doctor> Visits { get; set; }

        public ApplicationData(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            
        }

        public ApplicationData() : base() { }

    }
}
