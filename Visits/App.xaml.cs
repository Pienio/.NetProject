using DatabaseAccess;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Visits.Services;

namespace Visits
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string Name => ResourceAssembly.GetName().Name;
        public static IUnityContainer Container { get; private set; }
        private IApplicationDataFactory _factory;

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            _factory?.Dispose();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IUnityContainer container = new UnityContainer();
            ContainerInitializer.Initialize(container);
            container.RegisterInstance<ILogUserService>(new LogUserService());

            _factory = container.Resolve<IApplicationDataFactory>();
            _factory.CreateApplicationData().Fill();

            MainWindow = container.Resolve<MainWindow>();
            Container = container;
            MainWindow.Show();
        }
    } 
}
