using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseTest
{
    static class DataExtensions
    {
        public static void DetachOn(this IApplicationData db)
        {
            ObjectContext a = ((IObjectContextAdapter)db).ObjectContext;
            unchecked
            {
                foreach (var entry in a.ObjectStateManager.GetObjectStateEntries(
                    EntityState.Added | EntityState.Deleted | EntityState.Modified | EntityState.Unchanged))
                {
                    if (entry.Entity != null)
                    {
                        a.Detach(entry.Entity);
                    }
                }
            }

        }
    }
}
