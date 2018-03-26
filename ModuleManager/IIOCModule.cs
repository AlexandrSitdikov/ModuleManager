namespace ModuleManager
{
    /// <summary>
    /// Containered Module Interface
    /// </summary>
    public interface IIOCModule : IModule
    {
        /// <summary>
        /// Set IOC Container
        /// </summary>
        void SetContainer(object container);
    }
}