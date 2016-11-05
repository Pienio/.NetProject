using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    class Doctor : User
    {
        public WorkingTime MondayWorkingTime { get; set; }
        public WorkingTime TuesdayWorkingTime { get; set; }
        public WorkingTime WednesdayWorkingTime { get; set; }
        public WorkingTime ThursdayWorkingTime { get; set; }
        public WorkingTime FridayWorkingTime { get; set; }
        public WorkingTime SaturdayWorkingTime { get; set; }
        public WorkingTime SundayWorkingTime { get; set; }
    }
}
