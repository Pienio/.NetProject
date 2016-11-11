using System;
using System.Collections.Generic;
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
        public User User { get; set; }
    }
}
