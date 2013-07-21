namespace PulseMates.Infrastructure.Azure
{
    using Microsoft.WindowsAzure.StorageClient;

    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;

    public abstract class AzureImageMediaFormatter<TType> : BufferedMediaTypeFormatter
    {
        #region Field Members

        private static CloudBlobContainer _container;

        #endregion

        #region Constructor Members

        protected AzureImageMediaFormatter() 
        {
            //SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/jpeg"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/jpg"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/png"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/gif"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/bmp"));
        }        

        #endregion

        #region Property Members

        protected CloudBlobContainer Container
        {
            get
            {
                if (_container == null)
                    _container = BlobHelper.GetWebApiContainer();

                return _container;
            }
        }

        #endregion

        #region Override Members

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof(TType);
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            OnWriteToStream((TType)value, writeStream, content);
        }

        protected abstract void OnWriteToStream(TType value, Stream writeStream, HttpContent content);

        #endregion
    }
}