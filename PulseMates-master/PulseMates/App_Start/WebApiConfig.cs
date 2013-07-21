namespace PulseMates
{
    using Infrastructure.Formatters;

    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Tracing;
    using System.Net.Http.Formatting;
    using System.Web.Routing;
    using System.Net.Http;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            config.Routes.MapHttpRoute(
                name: "GroupApi",
                routeTemplate: "api/Item/Group/{groupBy}",
                defaults: new { controller = "Item", action = "Group", groupBy = "Day" }
            );

            config.Routes.MapHttpRoute(
                name: "GridApi",
                routeTemplate: "api/Item/Grid",
                defaults: new { controller = "Item", action = "Grid" }
            );

            config.Routes.MapHttpRoute(
                name: "TagRoute",
                routeTemplate: "api/Tags",
                defaults: new { controller = "Item", action = "Tags" }
            );

            config.Routes.MapHttpRoute(
                name: "PageNodeRoute",
                routeTemplate: "api/Page/{id}/Item",
                defaults: new { controller = "Page", action = "Item" }
            );

            config.Routes.MapHttpRoute(
                name: "ImageApiRoute",
                routeTemplate: "api/{controller}/{id}/Image/{size}",
                defaults: new { action = "Image", size = "Full" },
                constraints: new { size = "Tiny|Small|Medium|Large|Full" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional, action = "" }
            );

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}"
            //);

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            config.EnableQuerySupport();

            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;

            config.Formatters.Add(new NodeImageMediaTypeFormatter());
            config.Formatters.XmlFormatter.UseXmlSerializer = true;

            config.Formatters.JsonFormatter.AddQueryStringMapping("format", "json", "application/json");
            config.Formatters.XmlFormatter.AddQueryStringMapping("format", "xml", "application/xml");
            config.Formatters.Last().AddQueryStringMapping("format", "image", "image/*");
        }
    }
}
