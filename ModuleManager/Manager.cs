namespace ModuleManager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Module Manager
    /// </summary>
    public class Manager
    {
        private List<LoadingModule> moduleList = new List<LoadingModule>();
        private State state;

        private string modulePath;
        private Func<FileInfo, bool> moduleFilter;

        /// <summary>
        /// Module Manager
        /// </summary>
        /// <param name="modulePath">.dll path</param>
        /// <param name="moduleFilter">.dll filter</param>
        public Manager(string modulePath = null, Func<FileInfo, bool> moduleFilter = null)
        {
            this.modulePath = modulePath;
            this.moduleFilter = moduleFilter;

            this.state = State.Ready;
        }

        /// <summary>
        /// After IModule Registered
        /// </summary>
        public event ModuleRegistered ModuleRegistered;

        /// <summary>
        /// On IModule registering Exception. Return False for stop throw
        /// </summary>
        public event ModuleRegiterFail ModuleRegiterFail;

        /// <summary>
        /// Before State change
        /// </summary>
        public event StateChange StateChange;

        /// <summary>
        /// State
        /// </summary>
        public State State
        {
            get
            {
                return this.state;
            }

            private set
            {
                this.StateChange?.Invoke(value, this.state);
                this.state = value;
            }
        }

        /// <summary>
        /// Load Modules from AppDomain & module path dlls
        /// </summary>
        /// <param name="getModuleInstance">Get instance from IOC</param>
        /// <param name="registerModuleType">Register type in IOC</param>
        public void LoadModules(Func<Type, IModule> getModuleInstance, Action<Type> registerModuleType)
        {
            this.State = State.LoadAssemblies;

            var assemplyList = AppDomain.CurrentDomain.GetAssemblies()
                .Where(s => s.GetTypes().Any(p => typeof(IModule).IsAssignableFrom(p)))
                .ToList();

            if (this.modulePath != null)
            {
                var bins = new DirectoryInfo(this.modulePath).GetFiles("*.dll");
                foreach (var bin in bins)
                {
                    if (assemplyList.Any(x => x.CodeBase.ToLower().EndsWith('/' + bin.Name.ToLower())) || (this.moduleFilter != null && !this.moduleFilter(bin)))
                    {
                        continue;
                    }

                    var ass = Assembly.LoadFrom(bin.FullName);
                    if (ass.GetTypes().Any(p => typeof(IModule).IsAssignableFrom(p)))
                    {
                        assemplyList.Add(ass);
                    }
                }
            }

            this.LoadModules(getModuleInstance, registerModuleType, assemplyList.ToArray());
        }

        /// <summary>
        /// Load Modules from Assemblies. Use this method for Tests
        /// </summary>
        /// <param name="getModuleInstance">Get instance from IOC</param>
        /// <param name="registerModuleType">Register type in IOC</param>
        /// <param name="assemplyList">Module assembly list</param>
        public void LoadModules(Func<Type, IModule> getModuleInstance, Action<Type> registerModuleType, params Assembly[] assemplyList)
        {
            this.State = State.LoadModules;
            Init(getModuleInstance, registerModuleType, assemplyList);
            while (moduleList.Any(x => !x.IsLoaded))
            {
                foreach (var module in moduleList.Where(ml => !ml.IsLoaded && !ml.Assembly.GetReferencedAssemblies().Any(x => moduleList.Any(l => !l.IsLoaded && l.Assembly.GetName().Name.Equals(x.Name)))).ToList())
                {
                    try
                    {
                        module.Module.Register();
                        module.IsLoaded = true;
                        this.ModuleRegistered?.Invoke(module.Module);
                    }
                    catch (Exception ex)
                    {
                        if (this.ModuleRegiterFail == null || this.ModuleRegiterFail.Invoke(module.Module, ex) == false)
                        {
                            this.State = State.Fail;
                            throw;
                        }
                    }
                }
            }

            this.State = State.Success;
        }

        /// <summary>
        /// Get all loaded modules
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IModule> GetModules()
        {
            return moduleList.Where(x => x.IsLoaded).Select(x => x.Module);
        }

        /// <summary>
        /// Get Module from Assembly
        /// </summary>
        /// <param name="assembly">Assembly</param>
        /// <returns>IModule instance</returns>
        public IModule GetModuleByAssembly(Assembly assembly)
        {
            return moduleList.Where(x => x.Module.GetType().Assembly == assembly).Select(x => x.Module).FirstOrDefault();
        }

        private void Init(Func<Type, IModule> getModuleInstance, Action<Type> registerModuleType, params Assembly[] assemplyList)
        {
            moduleList.Clear();
            if (assemplyList != null)
            {
                foreach (var assembly in assemplyList.OrderBy(x => x.GetName().Name))
                {
                    var moduleType = assembly.GetTypes().FirstOrDefault(x => x.GetInterfaces().Any(i => i == typeof(IModule)));
                    if (moduleType != null)
                    {
                        registerModuleType(moduleType);
                        var module = getModuleInstance(moduleType);
                        moduleList.Add(new LoadingModule(module));
                    }
                }
            }
        }

        private class LoadingModule
        {
            public LoadingModule(IModule module)
            {
                this.Assembly = module.GetType().Assembly;
                this.Module = module;
            }

            public IModule Module { get; set; }

            public bool IsLoaded { get; set; }

            public Assembly Assembly { get; set; }

            public override string ToString()
            {
                return string.Format("Name: {0}, IsLoaded: {1}", this.Assembly.FullName, this.IsLoaded);
            }
        }
    }
}