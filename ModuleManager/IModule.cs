namespace ModuleManager
{
    /// <summary>
    /// Module Interface
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Module Name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Register Module
        /// </summary>
        void Register();
    }
}
