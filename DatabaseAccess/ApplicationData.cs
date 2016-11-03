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
            List<Specialization> op = new List<Specialization>();
            if(Specializations.Count()==0)
                 op.Add(new Specialization("SUPA"));

            IQueryable<Specialization> rtn = from temp in Specializations select temp;

            // var list = rtn.ToList();
            //foreach (var a in rtn.ToList())
            //{
            //    //op.Add(new Specialization(a.Name));
            //}
            string a = rtn.First().Name;
               // op = Specializations.Select(p => p).ToList();
            
            
           
         
            return op;
        }
        public void FillSpec()
        {
            if (Specializations.Count() == 0)
                Specializations.Add(new Specialization("Patient"));
            this.SaveChanges();
        }

    }
}
