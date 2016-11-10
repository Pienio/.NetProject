using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Model
{
    public class User : Entity
    {
        public string PESEL { get; set; }
        /// <summary>
        /// Trzeba będzie zrobić szyfrowanie przy secie 
        /// </summary>
        public string Password { get; set; }
        public virtual PersonName Name { get; set; }

        public DocOrPat Kind { get; set; }
    }
}
