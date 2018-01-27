using System.Collections.Generic;
using System.Reflection;
using API.Ext;

namespace API.Providers.Storage
{
    public class AmazonStorageHandlerProvider : IStorageHandlerProvider
    {
        private static readonly ProviderInfo Info = new ProviderInfo("Amazon");
        private static readonly IEnumerable<ProviderParameterInfo> Params = new[]
        {
            new ProviderParameterInfo(nameof(AmazonStorageHandler.AmazonAccessKey), true),
            new ProviderParameterInfo(nameof(AmazonStorageHandler.AmazonSecretAccessKey), true),
            new ProviderParameterInfo(nameof(AmazonStorageHandler.AmazonRegion), true)
        };

        private IDictionary<string, object> _productParams;

        private AmazonStorageHandler _handler;

        public ProviderInfo Describe() => Info;

        public IEnumerable<ProviderParameterInfo> DescribeParameters() => Params;

        public void SetParameters(IDictionary<string, object> productParams)
        {
            _productParams = productParams;
        }

        public IStorageHandler Produce()
        {
            if (_handler == null)
            {
                lock (this)
                {
                    if (_handler == null)
                    {
                        _handler = new AmazonStorageHandler();

                        if (_productParams != null)
                        {
                            foreach (ProviderParameterInfo param in Params)
                            {
                                if (_productParams.ContainsKey(param.Name))
                                {
                                    typeof(AmazonStorageHandler)
                                        .GetTypeInfo()
                                        .GetProperty(param.Name, BindingFlags.Public | BindingFlags.Instance)
                                        .SetValue(_handler, _productParams[param.Name]);
                                }
                            }
                        }

                        _handler.Init();
                    }
                }
            }

            return _handler;
        }
    }
}
