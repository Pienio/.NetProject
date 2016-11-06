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
        DbSet<User> Users { get; set; }
        DbSet<Patient> Patients { get; set; }
        DbSet<Doctor> Doctors { get; set; }
        DbSet<Specialization> Specializations { get; set; }
        DbSet<Visit> Visits { get; set; }
        void Fill();
        void AddPatient(Patient nowy);
        void AddSpecialization(Specialization nowy);
        void AddDoctor(Doctor nowy);
        void UpdateDoctor(Doctor nowy);
        void UpdatePatient(Patient nowy);
    }
}
