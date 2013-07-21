using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PulseMates.Infrastructure.Azure
{
    public class BlobFile
    {
        public string Id { get; set; }
        public string MediaType { get; set; }
        public string Url { get; set; }
    }
}