using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Model
{
    [ComplexType]
    public class PersonName
    {
        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual string Surname { get; set; }

        public override string ToString()
        {
            return Name + ' ' + Surname;
        }
    }
}
