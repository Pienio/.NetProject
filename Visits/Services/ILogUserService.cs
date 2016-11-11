using DatabaseAccess;
using DatabaseAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visits.Services
{
    public interface ILogUserService
    {
        Person Logged { get; }

        event EventHandler LoggedChanged;

        void LogIn(Person user);
        Task LogIn(string PESEL, string password, IApplicationData db);
        void LogOut();
    }
}
