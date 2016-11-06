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
    public class PersonName
    {
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }

        public override string ToString()
        {
            return Name + ' ' + Surname;
        }
    }
}
