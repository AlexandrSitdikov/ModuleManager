namespace ModuleManager
{
    using System;

    /// <summary>
    /// After IModule Registered
    /// </summary>
    public delegate void ModuleRegistered(IModule module);

    /// <summary>
    /// On IModule registering Exception. Return False for stop throw
    /// </summary>
    public delegate bool ModuleRegiterFail(IModule module, Exception ex);

    /// <summary>
    /// Before State change
    /// </summary>
    public delegate void StateChange(State state, State old);
}
