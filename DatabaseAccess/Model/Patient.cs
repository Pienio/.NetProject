using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Model
{
    public class Patient : Person
    {
        public override string ToString()
        {
            return User.Name.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Patient)
                return ((Patient)obj).Key == Key && ((Patient)obj).User.Equals(User);
            return false;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode() ^ User.Key.GetHashCode();
        }
    }
}
