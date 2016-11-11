using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess
{
    public class ContainerInitializer
    {
        public static void Initialize(IUnityContainer container)
        {
            container.RegisterInstance<IApplicationDataFactory>(new ApplicationDataFactory());
        }
    }
}
