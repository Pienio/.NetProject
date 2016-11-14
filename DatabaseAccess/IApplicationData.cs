using DatabaseAccess.Model;
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

        void SaveChangesOn();
    }
    
    public interface ITransactionalApplicationData : IApplicationData
    {
        bool IsTransactionRunning { get; }
        bool CommitUnfinishedTransaction { get; set; }
        bool ToCommit { get; set; }

        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
