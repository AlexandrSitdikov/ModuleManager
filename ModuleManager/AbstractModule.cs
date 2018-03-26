namespace ModuleManager
{
    /// <summary>
    /// Abstract Module, configurable by custom IOC/DI container
    /// </summary>
    /// <typeparam name="TContainer">Any IOC/DI container</typeparam>
    public abstract class AbstractModule<TContainer> : IModule
    {
        public AbstractModule(TContainer container)
        {
            this.Container = container;
        }

        public abstract string Name { get; }

        public abstract void Register();
        
        protected TContainer Container { get; private set; }
    }
}
