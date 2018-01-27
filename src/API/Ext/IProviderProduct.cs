using System;

namespace API.Ext
{
    public interface IProviderProduct : IDisposable
    {
        bool IsDisposed { get; }
    }
}
