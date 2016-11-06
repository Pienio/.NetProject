using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    public class Patient : Entity, IPerson
    {
        public virtual User User { get; set; }
    }
}
