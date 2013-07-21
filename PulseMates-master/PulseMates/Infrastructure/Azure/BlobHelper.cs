namespace PulseMates.Infrastructure.Azure
{
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Threading.Tasks;

    public static class BlobHelper
    {
        private static Dictionary<string, CloudBlobContainer> _container
            = new Dictionary<string, CloudBlobContainer>();
        
        public static CloudBlobContainer GetWebApiContainer(string containerName = "PulseMatesStorage")
        {
            if (string.IsNullOrEmpty(containerName))
                containerName = "PulseMatesStorage";

            if (_container.ContainsKey(containerName))
                return _container[containerName];

            // Retrieve storage account from connection-string
            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting(containerName)
            );

            // Create the blob client 
            var blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container Container name must use lower case
            var c = blobClient.GetContainerReference(containerName.ToLower());
            // Create the container if it doesn't already exist
            c.CreateIfNotExist();

            _container.Add(containerName, c);
            return c;
        }

        public static void GenerateThumbnails(CloudBlob blob)
        {
            var container = blob.Container;

            Action<ThumbnailSize, byte[]> generateAndSaveThumbnail = (size, byteArr) =>
            {
                var thumb = ThumbnailGenerator.Crop(byteArr, size);

                // Retrieve reference to a blob
                var thumbBlob = container.GetBlobReference(size  + "_" + blob.Uri);
                thumbBlob.Properties.ContentType = blob.Properties.ContentType;
                thumbBlob.UploadByteArray(thumb);
            };

            Task.Run(() =>
            {
                var imageArr = blob.DownloadByteArray();

                generateAndSaveThumbnail(ThumbnailSize.Tiny, imageArr);
                generateAndSaveThumbnail(ThumbnailSize.Small, imageArr);
                generateAndSaveThumbnail(ThumbnailSize.Medium, imageArr);
                generateAndSaveThumbnail(ThumbnailSize.Medium, imageArr);
            });
        }



    }
}