using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    public class Doctor : Entity, IPerson
    {
        public User User { get; set; } = new User();

        public Specialization Specialization { get; set; }

        public WorkingTime MondayWorkingTime { get; set; }
        public WorkingTime TuesdayWorkingTime { get; set; }
        public WorkingTime WednesdayWorkingTime { get; set; }
        public WorkingTime ThursdayWorkingTime { get; set; }
        public WorkingTime FridayWorkingTime { get; set; }
    }
}
