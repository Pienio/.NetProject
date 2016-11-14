using DatabaseAccess.Model;
using DeepEqual;
using DeepEqual.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visits;
using Visits.Services;
using Visits.Utils;

namespace DatabaseTest
{
    [TestClass]
    public class UITests : DataTest
    {
        [TestMethod]
        public async Task AuthenticationTest()
        {
            Patient p = new Patient();
            p.User = new User()
            {
                Kind = DocOrPat.Patient,
                Name = new PersonName() { Name = "F", Surname = "M" },
                Password = PasswordHasher.CreateHash("123456"),
                PESEL = "11111111111"
            };
            Patient q = new Patient() { User = p.User };
            Database.Patients.Add(p);
            Database.SaveChangesOn();
            Database.DetachOn();

            ILogUserService log = new LogUserService();
            await log.LogIn("11111111111", PasswordHasher.CreateHash("123456"), Database);
            Assert.IsTrue(q != log.Logged);
            Assert.IsTrue(q.IsDeepEqual(log.Logged));
        }
    }
}
