﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    public class User : Entity
    {
        public string Username { get; set; }
        /// <summary>
        /// Trzeba będzie zrobić szyfrowanie przy secie
        /// </summary>
        public string Password { get; set; }
        public PersonName Name { get; set; }
    }
}
