using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    public interface IApplicationData : IDisposable
    {
        IDbSet<User> Users { get; set; }
        IDbSet<Patient> Patients { get; set; }
        IDbSet<Doctor> Doctors { get; set; }
        IDbSet<Specialization> Specializations { get; set; }
        IDbSet<Visit> Visits { get; set; }
        List<Specialization> ShowSpec();
        List<Doctor> ShowDoc();
        void Fill();
    }
}
