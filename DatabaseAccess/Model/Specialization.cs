﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Model
{
    public class Specialization : Entity
    {
        public virtual string Name { get; set; }
         
        public Specialization() : base() { }
        public Specialization(string name) : base()
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}