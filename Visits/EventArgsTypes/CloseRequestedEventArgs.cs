using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visits.EventArgsTypes
{
    public class CloseRequestedEventArgs : EventArgs
    {
        public bool DialogResult { get; }
        public CloseRequestedEventArgs(bool dialogResult)
        {
            DialogResult = dialogResult;
        }
    }
}
