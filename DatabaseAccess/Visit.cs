using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    public class Visit : Entity
    {
        public User Patient { get; set; }
        public User Doctor { get; set; }
        public DateTime Date { get; set; }
    }
}
