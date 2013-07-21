namespace PulseMates.Infrastructure.Formatters
{
    using Azure;
    using Models;
    using Extensions;

    using System;
    using System.IO;
    using System.Net.Http;
    using System.Web;
    using System.Linq;
    using Microsoft.WindowsAzure.StorageClient;

    public class NodeImageMediaTypeFormatter : AzureImageMediaFormatter<Node>
    {
        protected override void OnWriteToStream(Node value, Stream writeStream, HttpContent content)
        {
            var request = HttpContext.Current.Request;
            var size = request.Url.GetQuerystringParameterValues("size").FirstOrDefault();

            if (value.Image == null)
                return;

            CloudBlob thumbRef = Container.GetBlobReference(size + "_" + value.Image.Id)
                    , imageRef = null;

            ThumbnailSize thumbnailSize;
            
            if (!string.IsNullOrEmpty(size) && Enum.TryParse(size, true, out thumbnailSize))
            {
                byte[] byteArr;

                try
                {
                    byteArr = thumbRef.DownloadByteArray();
                }
                catch (StorageClientException)
                {
                    imageRef = Container.GetBlobReference(value.Image.Id);

                    byteArr = ThumbnailGenerator.Crop(imageRef.DownloadByteArray(), thumbnailSize);

                    // update connection it will close with the download attempt.
                    thumbRef = Container.GetBlobReference(size + "_" + value.Image.Id);
                    thumbRef.Properties.ContentType = imageRef.Properties.ContentType;
                    thumbRef.UploadByteArray(byteArr);
                }
                
                writeStream.Write(byteArr, 0, byteArr.Length);
            }
            else
            {
                imageRef = Container.GetBlobReference(value.Image.Id);
                imageRef.DownloadToStream(writeStream);
            }

            content.Headers.Expires = new DateTimeOffset(DateTime.Now.AddHours(24));
        }
    }
}