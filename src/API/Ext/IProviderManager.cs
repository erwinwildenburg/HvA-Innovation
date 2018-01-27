using System.Collections.Generic;

namespace API.Ext
{
    public interface IProviderManager<out TProv, TProd> where TProv : IProvider<TProd> where TProd : IProviderProduct
    {
        IEnumerable<string> FoundProvidersNames { get; }

        TProv GetProvider(string name);
    }
}
