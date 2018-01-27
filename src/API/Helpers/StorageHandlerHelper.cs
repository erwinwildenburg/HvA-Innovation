using API.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace API.Helpers
{
    public class StorageHandlerHelper
    {
        private readonly StorageHandlerSettings _settings;
        private readonly StorageHandlerManager _storageManager;

        private IStorageHandlerProvider _defaultStorageProvider;

        public StorageHandlerHelper(IOptions<StorageHandlerSettings> settings, StorageHandlerManager storageManager)
        {
            _settings = settings.Value;
            _storageManager = storageManager;

            Init();
        }

        public IStorageHandler DefaultHandler { get; private set; }

        private void Init()
        {
            _defaultStorageProvider = _storageManager.GetProvider(_settings.Provider);
            if (_defaultStorageProvider == null)
                throw new ArgumentException("Invalid, missing or unresolved provider name.");

            if (_settings.Params?.Count > 0)
                _defaultStorageProvider.SetParameters(_settings.Params);

            DefaultHandler = _defaultStorageProvider.Produce();
            if (DefaultHandler == null)
                throw new InvalidOperationException("Failed to construct Storage handler.");
        }
    }
}
