using System;
using System.Collections.Generic;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;
using Microsoft.DotNet.PlatformAbstractions;

namespace API.Ext.Util
{
    /// <summary>
    /// Defines a base implementation of IProviderManager
    /// that supports dynamic extension provider discovery and loading from
    /// built-in assemblies and file paths using the Managed Extensibility Framework
    /// (MEF) version 2, or otherwise known as the light edition of MEF.
    /// </summary>
    public abstract class ProviderManagerBase<TProv, TProd> : IProviderManager<TProv, TProd>
        where TProv : IProvider<TProd>
        where TProd : IProviderProduct
    {

        private readonly ServiceProviderExportDescriptorProvider _adapter;
        private readonly List<Assembly> _builtInAssemblies = new List<Assembly>();
        private readonly List<Assembly> _searchAssemblies = new List<Assembly>();
        private readonly List<string> _searchPaths = new List<string>();
        private TProv[] _foundProviders;

        /// <summary>
        /// Constructs a base Provider Manager with default configuration settings. 
        /// </summary>
        /// <param name="adapter">an optional adapter to allow MEF dependencies
        ///    to be resolved by external DI sources</param>
        /// <remarks>
        ///    The default configuration of the base Manager adds the assemblies
        ///    containing the generic typic parameters (TProv, TProd) to be added
        ///    as <i>built-in</i> assemblies, and to include all other loaded and
        ///    active assemblies as searchable assemblies (and no search paths).
        ///    Additionally if an adapter is provided it will be added to the internal
        ///    MEF resolution process as a last step in resolving dependencies.
        /// </remarks>
        protected ProviderManagerBase(ServiceProviderExportDescriptorProvider adapter = null)
        {
            _adapter = adapter;

            Init();
        }

        protected virtual void Init()
        {
            // By default we include the assemblies containing the
            // principles a part of the built-ins and every other
            // assembly in context a part of the search scope
            AddBuiltIns(typeof(TProv).GetTypeInfo().Assembly, typeof(TProd).GetTypeInfo().Assembly);

            // Get a list of all assemblies used at runtime
            string runtimeId = RuntimeEnvironment.GetRuntimeIdentifier();
            IEnumerable<AssemblyName> assemblies = DependencyContext.Default.GetRuntimeAssemblyNames(runtimeId);
            List<AssemblyName> asms = new List<AssemblyName>();
            foreach (AssemblyName assembly in assemblies)
            {
                // Check if the assembly has already been processed
                // If it has already been processed it will be skipped
                if (asms.Contains(assembly))
                    continue;

                // Add assemblies to the temporary list
                // This prevents duplicate addition load
                asms.Add(assembly);

                // The assembly has not been added before
                // Add it to the assemblies to load
                AddSearchAssemblies(Assembly.Load(assembly));
            }
        }

        /// <summary>
        /// Lists the built-in assemblies that will be
        /// searched first for matching providers.
        /// </summary>
        public IEnumerable<Assembly> BuiltInAssemblies => _builtInAssemblies;

        /// <summary>
        /// Lists the built-in assemblies that will be
        /// searched first for matching providers.
        /// </summary>
        public IEnumerable<Assembly> SearchAssemblies => _searchAssemblies;

        /// <summary>
        /// Lists the built-in assemblies that will be
        /// searched first for matching providers.
        /// </summary>
        public IEnumerable<string> SearchPaths => _searchPaths;

        /// <summary>
        /// Returns all the matching provider implementations that
        /// have previously been discovered.  If necessary, invokes
        /// the resolution process to find matching providers.
        /// </summary>
        public IEnumerable<string> FoundProvidersNames
        {
            get
            {
                if (_foundProviders == null)
                    FindProviders();
                return _foundProviders.Select(p => p.Describe().Name);
            }
        }

        public TProv GetProvider(string name)
        {
            if (_foundProviders == null)
                FindProviders();
            return _foundProviders.FirstOrDefault(p => name.Equals(p.Describe().Name));
        }

        /// <summary>
        /// Resets the list of built-in assemblies to be searched.
        /// </summary>
        protected ProviderManagerBase<TProv, TProd> ClearBuiltIns()
        {
            _builtInAssemblies.Clear();
            return this;
        }

        /// <summary>
        /// Adds one or more built-in assemblies to be searched for matching provider
        /// implementations.
        /// </summary>
        private ProviderManagerBase<TProv, TProd> AddBuiltIns(params Assembly[] assemblies)
        {
            return AddBuiltIns((IEnumerable<Assembly>)assemblies);
        }

        /// <summary>
        /// Adds one or more built-in assemblies to be searched for matching provider
        /// implementations.
        /// </summary>
        private ProviderManagerBase<TProv, TProd> AddBuiltIns(IEnumerable<Assembly> assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                if (!_builtInAssemblies.Contains(assembly))
                    _builtInAssemblies.Add(assembly);
            }

            return this;
        }

        /// <summary>
        /// Resets the list of external assemblies to be searched.
        /// </summary>
        protected ProviderManagerBase<TProv, TProd> ClearSearchAssemblies()
        {
            _searchAssemblies.Clear();
            return this;
        }

        /// <summary>
        /// Adds one or more exteranl assemblies to be searched for matching provider
        /// implementations.
        /// </summary>
        private ProviderManagerBase<TProv, TProd> AddSearchAssemblies(params Assembly[] assemblies)
        {
            return AddSearchAssemblies((IEnumerable<Assembly>)assemblies);
        }

        /// <summary>
        /// Adds one or more exteranl assemblies to be searched for matching provider
        /// implementations.
        /// </summary>
        protected ProviderManagerBase<TProv, TProd> AddSearchAssemblies(IEnumerable<Assembly> assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                if (!_builtInAssemblies.Contains(assembly) && !_searchAssemblies.Contains(assembly))
                    _searchAssemblies.Add(assembly);
            }

            return this;
        }

        protected static AssemblyName GetAssemblyName(string assemblyName)
        {
            return AssemblyLoadContext.GetAssemblyName(assemblyName);
        }

        protected static Assembly GetAssembly(AssemblyName assemblyName)
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName);
        }

        /// <summary>
        /// Resets the list of directory paths to be searched.
        /// </summary>
        protected ProviderManagerBase<TProv, TProd> ClearSearchPaths()
        {
            _searchPaths.Clear();
            return this;
        }

        /// <summary>
        /// Adds one or more directory paths to be searched for matching provider
        /// implementations.
        /// </summary>
        protected ProviderManagerBase<TProv, TProd> AddSearchPath(params string[] paths)
        {
            return AddSearchPath((IEnumerable<string>)paths);
        }

        /// <summary>
        /// Adds one or more directory paths to be searched for matching provider
        /// implementations.
        /// </summary>
        protected ProviderManagerBase<TProv, TProd> AddSearchPath(IEnumerable<string> paths)
        {
            foreach (string path in paths)
            {
                if (!_searchPaths.Contains(path))
                    _searchPaths.Add(path);
            }

            return this;
        }

        /// <summary>
        /// Evaluates whether a candidate type is a provider type. 
        /// </summary>
        /// <remarks>
        /// The default implementation simply tests if the candidate type
        /// is a qualified descendent of the TProv provider type.
        /// <para>
        /// Subclasses may add, or replace with, other conditions such as testing
        /// for the presence of a particular class-level custom attribute or
        /// testing for the presence of other features of the class definition
        /// such as a qualifying constructor signature.
        /// </para>
        /// </remarks>
        private bool MatchProviderType(Type candidate)
        {
            return MefExtensions.IsDescendentOf(candidate, typeof(TProv));
        }

        /// <summary>
        /// Each time this is invoked, the search paths and patterns
        /// (built-in assemblies and directory paths + patterns) are
        /// searched to resolve matching components.  The results are
        /// cached and available in FoundProvider. 
        /// </summary>
        private IEnumerable<TProv> FindProviders()
        {
            try
            {
                ConventionBuilder conventions = new ConventionBuilder();
                conventions.ForTypesMatching<TProv>(MatchProviderType)
                    .Export<TProv>()
                    .Shared();

                IEnumerable<string> existingPaths = _searchPaths.Where(Directory.Exists);
                ContainerConfiguration configuration = new ContainerConfiguration()
                    .WithAssemblies(_builtInAssemblies, conventions)
                    .WithAssemblies(_searchAssemblies, conventions)
                    .WithAssembliesInPath(existingPaths.ToArray(), conventions);

                if (_adapter != null)
                    configuration.WithProvider(_adapter);

                using (CompositionHost container = configuration.CreateContainer())
                {
                    _foundProviders = container.GetExports<TProv>().ToArray();
                }

                return _foundProviders;
            }
            catch (ReflectionTypeLoadException e)
            {
                Console.Error.WriteLine(e);
                throw;
            }
        }
    }
}
