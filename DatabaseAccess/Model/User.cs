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
        [Required]
        public string PESEL { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public virtual PersonName Name { get; set; } = new PersonName();

        [Required]
        public DocOrPat Kind { get; set; }
    }
}
