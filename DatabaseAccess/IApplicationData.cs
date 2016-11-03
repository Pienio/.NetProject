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
        IDbSet<Doctor> Doctors { get; set; }
        IDbSet<Patient> Patients { get; set; }
        IDbSet<Specialization> Specializations { get; set; }
        IDbSet<Visit> Visits { get; set; }
        List<Doctor> AddSpec();
    }
}
