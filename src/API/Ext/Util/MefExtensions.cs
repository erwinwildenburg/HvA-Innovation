using System;
using System.Collections.Generic;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace API.Ext.Util
{
    public static class MefExtensions
    {
        private static readonly string[] DefaultPatterns = { "*.dll" };

        public static ContainerConfiguration WithAssembliesInPath(
            this ContainerConfiguration configuration,
            IEnumerable<string> paths,
            AttributedModelProvider conventions = null,
            SearchOption searchOption = SearchOption.TopDirectoryOnly,
            string[] patterns = null)
        {
            if (patterns == null) patterns = DefaultPatterns;

            foreach (string path in paths)
            {
                foreach (string pattern in patterns)
                {
                    // Load assemblies from path
                    List<Assembly> assemblies = Directory
                        .GetFiles(path, pattern, searchOption)
                        .Select(LoadFromAssembly)
                        .Where(x => x != null)
                        .ToList();

                    configuration.WithAssemblies(assemblies, conventions);
                }
            }

            return configuration;
        }

        private static Assembly LoadFromAssembly(string path)
        {
            try
            {
                return AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
            }
            catch (FileLoadException)
            {
                return null;
            }
        }

        internal static bool IsDescendentOf(Type type, Type baseType)
        {
            if (type == baseType || type == typeof(object) || type == null) return false;

            TypeInfo typeInfo1 = type.GetTypeInfo();
            TypeInfo typeInfo2 = baseType.GetTypeInfo();
            return typeInfo1.IsGenericTypeDefinition ? IsGenericDescendentOf(typeInfo1, typeInfo2) : typeInfo2.IsAssignableFrom(typeInfo1);
        }

        private static bool IsGenericDescendentOf(TypeInfo openType, TypeInfo baseType)
        {
            if (openType.BaseType == null) return false;
            if (openType.BaseType == baseType.AsType()) return true;

            foreach (Type type in openType.ImplementedInterfaces)
            {
                if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == baseType.AsType())
                    return true;
            }
            return IsGenericDescendentOf(openType.BaseType.GetTypeInfo(), baseType);
        }
    }
}
