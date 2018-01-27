using API.Configuration;
using API.Ext;
using API.Ext.Util;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace API
{
    public interface IStorageHandlerProvider : IProvider<IStorageHandler> { }

    public class StorageHandlerManager : ProviderManagerBase<IStorageHandlerProvider, IStorageHandler>
    {
        public StorageHandlerManager(IOptions<StorageHandlerSettings> settings, IServiceProvider sp) : base(new ServiceProviderExportDescriptorProvider(sp))
        {
            string[] extAssms = settings.Value?.Ext?.SearchAssemblies;
            string[] extPaths = settings.Value?.Ext?.SearchPaths;

            // Add assemblies to search context
            if ((settings.Value?.Ext?.ReplaceExtAssemblies).GetValueOrDefault())
                ClearSearchAssemblies();
            if (extAssms?.Length > 0)
            {
                AddSearchAssemblies(
                    extAssms.Select(x =>
                    {
                        AssemblyName an = GetAssemblyName(x);
                        if (an == null) throw new ArgumentException("Invalid assembly name.");
                        return an;
                    }).Select(x =>
                    {
                        Assembly asm = GetAssembly(x);
                        if (asm == null) throw new InvalidOperationException("Unable to resolve assembly from name.");
                        return asm;
                    }));
            }

            // Add directory paths to search context
            if ((settings.Value?.Ext?.ReplaceExtPaths).GetValueOrDefault())
                ClearSearchPaths();
            if (extPaths?.Length > 0)
            {
                AddSearchPath(extPaths.Select(x =>
                {
                    string y = Path.GetFullPath(x);
                    return y;
                }));
            }

            base.Init();
        }

        protected override void Init()
        {
            // Skipping the initialization till after construction parameters are applied
        }
    }
}
