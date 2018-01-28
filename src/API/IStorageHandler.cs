using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Ext;
using API.Models;

namespace API
{
    public interface IStorageHandler : IProviderProduct
    {
        #region -- Methods --

        Task<List<StoredFileInfo>> GetFileInfo(Guid id);

        #endregion -- Methods --
    }
}
