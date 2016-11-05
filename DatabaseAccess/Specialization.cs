using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    public class Specialization : Entity
    {
        public string Name { get; set; }

        public Specialization() : base() { }
        public Specialization(string name) : base()
        {
            Name = name;
        }
    }
}
