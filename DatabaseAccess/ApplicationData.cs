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
        public IDbSet<User> Users { get; set; }
        public IDbSet<Patient> Patients { get; set; }
        public IDbSet<Doctor> Doctors { get; set; }
        public IDbSet<Specialization> Specializations { get; set; }
        public IDbSet<Visit> Visits { get; set; }
        
        public ApplicationData(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            
        }

        public ApplicationData() : base() { }

        public List<Specialization>  ShowSpec()
        {
            return Specializations.Select(p => p).ToList();
        }
        public List<Doctor> ShowDoc()
        {
            return Doctors.Select(d => d).ToList();
        }
        public void Fill()
        {
            if (Specializations.Count() == 0)
            {
                Specializations.Add(new Specialization("Reumatolog"));
                Specializations.Add(new Specialization("Kardiolog"));
                Specializations.Add(new Specialization("Neurolog"));
                Specializations.Add(new Specialization("Urolog"));
                Specializations.Add(new Specialization("Okulista"));
                Specializations.Add(new Specialization("Psychiatra"));
                Specializations.Add(new Specialization("Ginekolog"));
            }
            if(Users.Count()==0)
            {
                string[] names = { "Kuba", "Jan", "Łukasz", "Adrian", "Bartosz", "Marek", "Filip", "Bartłomiej" };
                string[] surnames = { "Soczkowski", "Berwid", "Okular", "Michałowski", "Skała", "Mikowski", "Wasiłkowski", "Normowski" };
                string[] pesels = { "09586749381", "19683750923", "94860285691", "58672349682", "38596827364", "58476923857", "88975643287", "29384795618" };
                for (int i = 0; i < 8; i++)
                {
                    Doctor ne = new Doctor();
                    ne.User.Name.Name = names[i];
                    ne.User.Name.Surname = surnames[i];
                    ne.User.PESEL = pesels[i];
                    ne.User.Password = "1111111111";
                    ne.MondayWorkingTime = new WorkingTime();
                    ne.MondayWorkingTime.Start = 8 + i / 2;
                    ne.MondayWorkingTime.End = 12 + i / 2;
                    ne.TuesdayWorkingTime = new WorkingTime();
                    ne.TuesdayWorkingTime.Start = 8 + i / 2;
                    ne.TuesdayWorkingTime.End = 12 + i / 2;
                    ne.WednesdayWorkingTime = new WorkingTime();
                    ne.WednesdayWorkingTime.Start = 8 + i / 2;
                    ne.WednesdayWorkingTime.End = 12 + i / 2;
                    ne.ThursdayWorkingTime = new WorkingTime();
                    ne.ThursdayWorkingTime.Start = 8 + i / 2;
                    ne.ThursdayWorkingTime.End = 12 + i / 2;
                    ne.FridayWorkingTime = new WorkingTime();
                    ne.FridayWorkingTime.Start = 8 + i / 2;
                    ne.FridayWorkingTime.End = 12 + i / 2;
                    Doctors.Add(ne);
                }
            }
                
            this.SaveChanges();
        }

    }
}
