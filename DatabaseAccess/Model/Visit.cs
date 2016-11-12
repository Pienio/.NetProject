using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Model
{
    public class Visit : Entity
    {
        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
        public DateTime Date { get; set; } 

        public Visit() { }

        public Visit(Patient patient, Doctor doctor, DateTime date)
        {
            Patient = patient;
            Doctor = doctor;
            Date = date;
        }

        public override bool Equals(object obj)
        {
            Visit v = obj as Visit;
            if (v != null)
                return Patient.Equals(v.Patient) && Doctor.Equals(Doctor) && Date == v.Date;
            return false;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode() ^ Doctor.GetHashCode() ^ Patient.GetHashCode() ^ Date.GetHashCode();
        }
    }
}
