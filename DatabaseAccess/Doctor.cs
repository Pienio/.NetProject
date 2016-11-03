using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    public class Doctor : User
    {
        public Specialization Specialization { get; set; }
    }
}
