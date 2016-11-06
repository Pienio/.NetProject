using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    /// <summary>
    /// Zakładamy, że godziny dotyczą tego samego dnia, tj Start < End
    /// </summary>
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
