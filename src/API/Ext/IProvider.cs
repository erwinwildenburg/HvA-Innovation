using System.Collections.Generic;

namespace API.Ext
{
    public interface IProvider<out TProd> where TProd : IProviderProduct
    {
        ProviderInfo Describe();

        IEnumerable<ProviderParameterInfo> DescribeParameters();

        void SetParameters(IDictionary<string, object> productParams);

        TProd Produce();
    }
}
