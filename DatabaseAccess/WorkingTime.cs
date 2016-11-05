using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    class WorkingTime : Entity
    {
        public int Start { get; set; }
        public int End { get; set; }

        public WorkingTime()
        {

        }
    }
}
