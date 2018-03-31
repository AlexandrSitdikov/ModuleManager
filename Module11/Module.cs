namespace Module1
{
    using LightInject;
    using ModuleManager;
    using SampleMVC;

    public class Module : IModule
    {
        private IServiceContainer container;

        public string Name => "Awesome module";

        public Module(IServiceContainer container)
        {
            this.container = container;
            var module2 = typeof(Module2.Module); // for activate project reference
        }

        public void Register()
        {
            this.container.GetInstance<ILogger>().Log("module 1 registerd");
        }
    }
}