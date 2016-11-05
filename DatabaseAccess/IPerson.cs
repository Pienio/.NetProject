using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    /// <summary>
    /// Tutaj będą lądować wspólne metody i właściwości dla Doctor i Patient by wywoływać je z GUI
    /// </summary>
    public interface IPerson
    {
        User User { get; set; }
    }
}
