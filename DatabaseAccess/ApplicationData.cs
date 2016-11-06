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
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Visit> Visits { get; set; }

        public ApplicationData(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public ApplicationData() : base()
        {
        }

        public void Fill()
        {
            Specialization[] specs = {
            new Specialization("Reumatolog"),
            new Specialization("Kardiolog"),
            new Specialization("Neurolog"),
            new Specialization("Urolog"),
            new Specialization("Okulista"),
            new Specialization("Psychiatra"),
            new Specialization("Ginekolog"),
            new Specialization("Pediatra")};

            if (Specializations.Count() == 0)
            {
                Specializations.AddRange(specs);
                this.SaveChanges();
            }
            if (Users.Count() == 0)
            {
                string[] names = { "Kuba", "Jan", "Łukasz", "Adrian", "Bartosz", "Marek", "Filip", "Bartłomiej" };
                string[] surnames = { "Soczkowski", "Berwid", "Okular", "Michałowski", "Skała", "Mikowski", "Wasiłkowski", "Normowski" };
                string[] pesels = { "09586749381", "19683750923", "94860285691", "58672349682", "38596827364", "58476923857", "88975643287", "29384795618" };
                for (int i = 0; i < 8; i++)
                {
                    Doctor ne = new Doctor();

                    ne.User = new User() { Name = new PersonName() };
                    ne.User.Name.Name = names[i];
                    ne.User.Name.Surname = surnames[i];
                    ne.User.PESEL = pesels[i];
                    ne.User.Password = "96e79218965eb72c92a549dd5a330112";
                    ne.User.Kind = DocOrPat.Doctor;
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
                    ne.Specialization = specs[i];
                    Doctors.Add(ne);
                }
                this.SaveChanges();
            }
          
        }
        public void AddDoctor(Doctor nowy)
        {
            Doctors.Add(nowy);
            this.SaveChanges();
        }
        public void AddPatient(Patient nowy)
        {
            Patients.Add(nowy);
            this.SaveChanges();
        }
        public void AddSpecialization(Specialization nowy)
        {
            Specializations.Add(nowy);
            this.SaveChanges();
        }
        public void AddVisit(Visit nowy)
        {
            Visits.Add(nowy);
            this.SaveChanges();
        }
        public void UpdateDoctor(Doctor nowy)
        {
            this.Users.Attach(nowy.User);
            var entry = this.Entry(nowy.User);
            entry.State= EntityState.Modified;
            this.Doctors.Attach(nowy);
            var entry1 = this.Entry(nowy);
            entry1.State = EntityState.Modified;
            this.SaveChanges();
            
        }
        public void UpdatePatient(Patient nowy)
        {
            this.Users.Attach(nowy.User);
            var entry = this.Entry(nowy.User);
            entry.State = EntityState.Modified;
            this.Patients.Attach(nowy);
            var entry1 = this.Entry(nowy);
            entry1.State = EntityState.Modified;
            this.SaveChanges();
        }
    }
}
