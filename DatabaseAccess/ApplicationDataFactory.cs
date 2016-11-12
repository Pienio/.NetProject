using DatabaseAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    public class ApplicationDataFactory : IApplicationDataFactory
    {
        private static ITransactionalApplicationData instance;

        public IApplicationData CreateApplicationData()
        {
            return CreateDbContext(false);
        }

        public ITransactionalApplicationData CreateTransactionalApplicationData()
        {
            return CreateDbContext(true);
        }

        public void Dispose()
        {
            if (instance != null)
                instance.Dispose();
        }

        private ITransactionalApplicationData CreateDbContext(bool beginTransaction)
        {
            if (instance == null)
                instance = new ApplicationData(beginTransaction);
            else if (beginTransaction && !instance.IsTransactionRunning)
                instance.BeginTransaction();
            else if (instance.IsTransactionRunning)
            {
                if (instance.CommitUnfinishedTransaction)
                    instance.Commit();
                else
                    instance.Rollback();
                if (beginTransaction)
                    instance.BeginTransaction();
            }
            return instance;
        }
    }
}
