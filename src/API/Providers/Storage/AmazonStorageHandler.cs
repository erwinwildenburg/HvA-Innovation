using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.S3;
using Amazon.S3.Model;
using API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Providers.Storage
{
    /// <summary>
    /// Implements a Amazon based Storage Handler.
    /// </summary>
    public sealed class AmazonStorageHandler : IStorageHandler
    {
        public bool IsDisposed { get; private set; }

        public string AmazonAccessKey { get; set; }
        public string AmazonSecretAccessKey { get; set; }
        public string AmazonRegion { get; set; }

        private DynamoDBContext _dDbContext;
        private IAmazonS3 _s3Client;

        public void Init()
        {
            // Initialize connection to Amazon Storage
            Amazon.RegionEndpoint region = Amazon.RegionEndpoint.GetBySystemName(AmazonRegion);
            _s3Client = new AmazonS3Client(AmazonAccessKey, AmazonSecretAccessKey, region);
            _dDbContext = new DynamoDBContext(new AmazonDynamoDBClient(AmazonAccessKey, AmazonSecretAccessKey, region));
        }

        public async Task<List<StoredFileInfo>> GetFileInfo(Guid id)
        {
            List<ScanCondition> conditions;
            // Get results for only deviceId
            if (id != Guid.Empty)
            {
                // Search all results
                conditions = new List<ScanCondition>
                {
                    new ScanCondition("Id", ScanOperator.Equal, id)
                };
                return (await _dDbContext.ScanAsync<DynamoDbStoredFileInfo>(conditions).GetRemainingAsync()).Select(x => x.ToStoredFileInfo()).ToList();
            }

            // Search all results
            conditions = new List<ScanCondition> { };
            return (await _dDbContext.ScanAsync<DynamoDbStoredFileInfo>(conditions).GetRemainingAsync()).Select(x => x.ToStoredFileInfo()).ToList();
        }

        #region -- Amazon AWS Objects --

        [DynamoDBTable("hva-innovation-files")]
        public class DynamoDbStoredFileInfo
        {
            [DynamoDBHashKey] private string Id { get; set; }
            public string Name { get; set; }
            public string Language { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }

            public StoredFileInfo ToStoredFileInfo()
            {
                return new StoredFileInfo
                {
                    Id = Guid.Parse(Id),
                    Name = Name,
                    Language = Language,
                    CreatedAt = CreatedAt,
                    UpdatedAt = UpdatedAt
                };
            }
        }

        #endregion -- Amazon AWS Objects --

        #region -- IDisposable Support --

        // The bulk of the clean-up code is implemented in Dispose(bool)
        private void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            // free managed resources
            if (disposing)
            {
            }

            // free native resources if there are any.

            IsDisposed = true;
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources itself, but leave the other methods
        // exactly as they are.
        //~AmazonDscHandler() {
        // Finalizer calls Dispose(false)
        //Dispose(false);
        //}

        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }

        #endregion -- IDisposable Support --
    }
}
