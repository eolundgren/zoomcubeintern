namespace PulseMates.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.ModelBinding;

    public static class ApiControllerExtension
    {
        public static HttpResponseException CreateException(this ApiController controller, HttpStatusCode statusCode, string reason, string message, params object[] args)
        {
            var response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(string.Format(message, args)),
                ReasonPhrase = reason
            };

            return new HttpResponseException(response);
        }
    }
}