using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Model
{
    public class Visit : Entity
    {
        [Required]
        public virtual Patient Patient { get; set; }
        [Required]
        public virtual Doctor Doctor { get; set; }
        [Required]
        public DateTime Date { get; set; } 

        public Visit() { }

        public Visit(Patient patient, Doctor doctor, DateTime date)
        {
            Patient = patient;
            Doctor = doctor;
            Date = date;
        }
    }
}
