using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    [ComplexType]
    public class PersonName
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
