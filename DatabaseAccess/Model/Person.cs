using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Model
{
    /// <summary>
    /// Tutaj będą lądować wspólne metody i właściwości dla Doctor i Patient by wywoływać je z GUI
    /// </summary>
    public abstract class Person : Entity
    {
        [Required]
        public virtual User User { get; set; }
        public virtual IList<Visit> Visits { get; set; } = new List<Visit>();
    }
}
