namespace ModuleManager
{
    /// <summary>
    /// Manager state
    /// </summary>
    public enum State
    {
        /// <summary>
        /// Ready for launch
        /// </summary>
        Ready = 0,

        /// <summary>
        /// Loading assemblies from ApDomain & dlls
        /// </summary>
        LoadAssemblies = 1,

        /// <summary>
        /// Module loading processing
        /// </summary>
        LoadModules = 2,

        /// <summary>
        /// Loading failed
        /// </summary>
        Fail = 3,

        /// <summary>
        /// Loading success
        /// </summary>
        Success = 4
    }
}