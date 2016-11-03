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
        IDbSet<Doctor> Patients { get; set; }
        IDbSet<Doctor> Specializations { get; set; }
        IDbSet<Doctor> Visits { get; set; }
    }
}
