using System;
using System.Collections.Generic;
using System.Composition.Hosting.Core;

namespace API.Ext.Util
{
    /// <summary>
    /// Custom MEF provider to resolve dependencies against the native DI framework.
    /// </summary>
    /// <remarks>
    /// This class acts as an adapter that bridges the dependency resolution mechanism
    /// of MEF to be able to resolve against the services provided by the .NET Core
    /// native dependency injection (DI) facility.
    /// </remarks>
    public class ServiceProviderExportDescriptorProvider : ExportDescriptorProvider
    {
        private const string OriginName = "DI-ServiceProvider";

        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderExportDescriptorProvider(IServiceProvider sp)
        {
            _serviceProvider = sp;
        }

        public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors(
                CompositionContract contract, DependencyAccessor descriptorAccessor)
        {

            object svc = _serviceProvider.GetService(contract.ContractType);
            if (svc == null)
            {
                yield break;
            }

            object Ca(LifetimeContext ctx, CompositionOperation op) => svc;
            yield return new ExportDescriptorPromise(contract, OriginName, true,
                    NoDependencies, deps => ExportDescriptor.Create(Ca, NoMetadata));
        }
    }
}
