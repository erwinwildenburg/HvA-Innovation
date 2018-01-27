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
        public string ConfigurationBucketName { get; set; }
        public string ReportStatusDataBucketName { get; set; }
        public string BuildQueueUrl { get; set; }

        private DynamoDBContext _dDbContext;
        private IAmazonS3 _s3Client;

        public void Init()
        {
            // Initialize connection to Amazon Storage
            Amazon.RegionEndpoint region = Amazon.RegionEndpoint.GetBySystemName(AmazonRegion);
            _s3Client = new AmazonS3Client(AmazonAccessKey, AmazonSecretAccessKey, region);
            _dDbContext = new DynamoDBContext(new AmazonDynamoDBClient(AmazonAccessKey, AmazonSecretAccessKey, region));
        }

        #region -- Amazon AWS Objects --

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
