using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    public class Entity
    {
        [Key]
        public long Key { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }
    }
}
