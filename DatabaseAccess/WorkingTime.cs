using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    [ComplexType]
    public class WorkingTime
    {
        public int Start { get; set; }
        public int End { get; set; }

        public WorkingTime() 
        { 

        }
    }
}
