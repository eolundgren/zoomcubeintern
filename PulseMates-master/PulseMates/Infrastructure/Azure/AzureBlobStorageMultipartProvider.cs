namespace PulseMates.Infrastructure.Azure
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using ioFile = System.IO.File;
    using Path = System.IO.Path;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;

    using Microsoft.WindowsAzure.StorageClient;

    using Extensions;

    public class AzureBlobStorageMultipartProvider : MultipartFormDataStreamProvider
    {
        #region Field Members

        private CloudBlobContainer _container;
        private Dictionary<string, BlobFile> _files =
            new Dictionary<string, BlobFile>();

        #endregion

        #region Constructor Members

        public AzureBlobStorageMultipartProvider(CloudBlobContainer container) 
            : base(Path.GetTempPath())
        {
            if (container == null)
                throw new ArgumentNullException("container");

            _container = container;
        }

        #endregion

        #region Property Members

        public IEnumerable<KeyValuePair<string, BlobFile>> Files
        {
            get { return _files; }
        }

        #endregion

        #region Override Members

        public override Task ExecutePostProcessingAsync()
        {
            // Upload the files to azure blob storage and remove them from local disk
            foreach (var fileData in FileData)
            {
                string fileName = System.IO.Path.GetFileName(fileData.Headers.ContentDisposition.FileName.Trim('"'))
                     , contentType = fileData.Headers.ContentDisposition.DispositionType.Trim('"')
                     , slug = fileName.Slugify()
                     , key = fileData.Headers.ContentDisposition.Name.Trim('"')
                     , id = Guid.NewGuid().ToString();

                if (string.IsNullOrEmpty(fileName))
                    continue;

                // Retrieve reference to a blob
                var blob = _container.GetBlobReference(id);
                blob.Properties.ContentType = contentType;
                blob.UploadFile(fileData.LocalFileName);

                ioFile.Delete(fileData.LocalFileName);

                _files.Add(key, new BlobFile { Id = id, MediaType = contentType, Url = blob.Uri.AbsoluteUri });
            }

            return base.ExecutePostProcessingAsync();
        }

        #endregion
    }
}