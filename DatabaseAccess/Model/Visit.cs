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
    }
}
