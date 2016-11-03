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
       // public IDbSet<Doctor> Doctors { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<Specialization> Specializations { get; set; }
        public IDbSet<Visit> Visits { get; set; }
        

        public ApplicationData(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            
        }

        public ApplicationData() : base() { }

        public List<Specialization>  AddSpec()
        {
           
            return Specializations.Select(p => p).ToList();
        }
        public void FillSpec()
        {
            if (Specializations.Count() == 0)
                Specializations.Add(new Specialization("Patient"));
            this.SaveChanges();
        }

    }
}
