
# ModuleManager
Very simple module manager for build loosely coupled .NET applications based on IOC/DI

## NuGet
[https://www.nuget.org/packages/ModuleManager](https://www.nuget.org/packages/ModuleManager)

# Example
MyCusctomModule.dll

    public class Module : IModule
    {
	    private IIocContainer container;

	    public Module(IIocContainer container)
	    {
	        this.container = container;
	    }

	    public string Name
	    {
	        get { return "My Custom Module 1"; }
	    }

	    public void Register()
	    {
	        // do something
	    }
    }

MyCusctomModule2.dll

    public class Module : IModule
    {
	    private IIocContainer container;

	    public Module(IIocContainer container)
	    {
	        this.container = container;
	    }
	    
	    public override string Name
	    {
		    get { return "My Custom Module 2"; }
	    }

	    public override void Register()
	    {
	        // do something with IIocContainer
	        // this.Container.Register(...)
	    }
    }

Application.dll
    
	var container = new ServiceContainer();
    	var moduleManager = new ModuleManager.Manager();

    	container.RegisterInstance<IServiceContainer>(container);
    	container.RegisterInstance(moduleManager);

    	moduleManager.LoadModules(type => (ModuleManager.IModule)container.GetInstance(type), type => container.Register(type));
