namespace ModuleManager
{
    /// <summary>
    /// Abstract Module, configurable by custom IOC/DI container
    /// </summary>
    /// <typeparam name="TContainer">Any IOC/DI container</typeparam>
    public abstract class AbstractModule<TContainer> : IIOCModule
    {
        public abstract string Name { get; }

        protected TContainer Container { get; private set; }

        public abstract void Register();

        public void SetContainer(object container)
        {
            this.Container = (TContainer)container;
        }
    }
}
