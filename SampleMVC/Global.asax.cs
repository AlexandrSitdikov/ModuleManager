namespace SampleMVC
{
    using System;
    using System.Diagnostics;
    using System.Web.Mvc;
    using System.Web.Routing;
    using LightInject;

    public class Global : System.Web.HttpApplication
    {
        internal static IServiceContainer Container { get; private set; }

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                  new { controller = "sample", action = "modulelist", id = UrlParameter.Optional });


            Container = new ServiceContainer();
            var moduleManager = new ModuleManager.Manager();

            Container.RegisterInstance<IServiceContainer>(Container);
            Container.RegisterInstance(moduleManager);
            Container.RegisterInstance<ILogger>(new Logger());

            moduleManager.ModuleRegistered += module => Debug.WriteLine($"Module from assembly {module.GetType().Assembly.GetName().Name} loaded");
            moduleManager.LoadModules(type => (ModuleManager.IModule)Container.GetInstance(type), type => Container.Register(type));
        }
    }
}