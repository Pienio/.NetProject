using DatabaseAccess;
using DatabaseAccess.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visits.Services
{
    public class LogUserService : ILogUserService
    {
        public Person Logged { get; private set; }

        public event EventHandler LoggedChanged;
        private void OnLoggedChanged()
        {
            if (LoggedChanged != null)
                LoggedChanged(this, EventArgs.Empty);
        }

        public void LogIn(Person user)
        {
            if (Logged != null)
                throw new InvalidOperationException("Nie można zalogować nowego użytkownika, gdy inny jest już zalogowany.");
            Logged = user;
            OnLoggedChanged();
        }

        public async Task LogIn(string PESEL, string password, IApplicationData db)
        {
            var user = await Task.Run(() => (from u in db.Users
                                             where u.PESEL == PESEL && u.Password == password
                                             select u).First());
            Person person;
            if (user.Kind == DocOrPat.Doctor)
                person = await Task.Run(() => Logged = (from d in db.Doctors
                                                        where d.User.Key == user.Key
                                                        select d).First());
            else //Kind == Person
                person = await Task.Run(() => Logged = (from p in db.Patients
                                                        where p.User.Key == user.Key
                                                        select p).First());
            Logged = person;
            OnLoggedChanged();
        }

        public void LogOut()
        {
            if (Logged == null)
                throw new InvalidOperationException("Brak użytkownika do wylogowania");
            Logged = null;
            OnLoggedChanged();
        }
    }
}
