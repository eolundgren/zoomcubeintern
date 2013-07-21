namespace PulseMates.Controllers.WebAPI
{
    using Filters;
    using Infrastructure.Azure;
    using Infrastructure.Filters;
    using Infrastructure.Mongo;
    using Infrastructure.Extensions;

    using Microsoft.WindowsAzure.StorageClient;
    using Models;
    using Models.Storage;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Net.Http.Headers;

    //[Authorize]
    //[ValidateHttpAntiForgeryToken]
    public class ItemController : ApiController
    {
        #region Field Members

        private static Dictionary<GroupByFilter, string> _groupByFormats = new Dictionary<GroupByFilter, string>()
        {
            { GroupByFilter.Year, "yyyy" },
            { GroupByFilter.Month, "yyyy-MM" },
            { GroupByFilter.Day, "yyyy-MM-dd" },
            { GroupByFilter.Hour, "yyyy-MM-dd HH" },
            { GroupByFilter.Minute, "yyyy-MM-dd HH:mm" },
            { GroupByFilter.Second, "yyyy-MM-dd HH:mm:ss" },
        };

        private static Dictionary<GroupByFilter, Func<DateTime, DateTime>> _groupBy = new Dictionary<GroupByFilter, Func<DateTime, DateTime>>()
        {
            { GroupByFilter.Year,       x => new DateTime(x.Year, 1,       1,     0,      0,        0) },
            { GroupByFilter.Month,      x => new DateTime(x.Year, x.Month, 1,     0,      0,        0) },
            { GroupByFilter.Day,        x => new DateTime(x.Year, x.Month, x.Day, 0,      0,        0) },
            { GroupByFilter.Hour,       x => new DateTime(x.Year, x.Month, x.Day, x.Hour, 0,        0) },
            { GroupByFilter.Minute,     x => new DateTime(x.Year, x.Month, x.Day, x.Hour, x.Minute, 0) },
            { GroupByFilter.Second,     x => new DateTime(x.Year, x.Month, x.Day, x.Hour, x.Minute, x.Second) },
        };

        private INodeRepository _repository;
        private static readonly CloudBlobContainer _container = BlobHelper.GetWebApiContainer();

        private static Func<string, bool> IsImageType = x =>
        {
            return x.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase) ||
                x.Equals("image/gif", StringComparison.OrdinalIgnoreCase) ||
                x.Equals("image/png", StringComparison.OrdinalIgnoreCase) ||
                x.Equals("image/bmp", StringComparison.OrdinalIgnoreCase);
        };

        private static Func<string, bool> IsModelType = x =>
        {
            return x.Equals("application/json", StringComparison.OrdinalIgnoreCase) ||
                x.Equals("application/xml", StringComparison.OrdinalIgnoreCase) ||
                x.Equals("text/xml", StringComparison.OrdinalIgnoreCase);
        };

        #endregion

        #region Constructor Members

        public ItemController() : this(new NodeRepository()) { }
        public ItemController(INodeRepository repository)
        {
            _repository = repository;
        }

        #endregion

        #region Action Members

        [HttpGet, ActionName("Grid")]
        public Paged<Node> GetAllAsGrid([FromUri]GeoFilter filter)
        {
            return Get(filter).AsPaged(filter.Index, filter.Size);
        }

        [HttpGet, ActionName("Group")]
        public IEnumerable<NodeStack> GetAllGroupByDate([FromUri]GroupByFilter groupBy, [FromUri]GeoFilter filter)
        {
            return Get(filter).ToList()
                .GroupBy(x => _groupBy[groupBy](x.Time))
                .Select(x => new NodeStack { Name = x.Key.ToString(_groupByFormats[groupBy]), Nodes = x })
                .OrderByDescending(x => x.Name);
        }

        [HttpGet, ActionName("Tags")]
        public IQueryable<Tag> GetAllTags()
        {
            return _repository.GetUniqueTags().AsQueryable();
        }

        [HttpGet, ActionName("Image")]
        public HttpResponseMessage GetImage(string id, ImageFilter size = ImageFilter.Full)
        {
            var node = _repository.Find(id);

            if (node == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Node Wasn't Found.");

            return null;
        }

        #endregion

        #region Method Members

        [HttpGet, ActionName("")]
        public IQueryable<Node> Get([FromUri]GeoFilter filter)
        {
            var tags = Request.RequestUri.GetQuerystringParameterValues("tag").ToArray();

            var query = filter.HasLocation ?
                tags.Length > 0 ?
                    _repository.FindAll(tags, filter.Longitude, filter.Latitude, filter.Radius) :
                    _repository.FindAll(filter.Longitude, filter.Latitude, filter.Radius) :
                tags.Length > 0 ?
                    _repository.FindAll(tags) :
                    _repository.FindAll();

            return query; //.AsPaged(filter.Index, filter.Size);
        }

        [HttpGet, ActionName("")]
        public HttpResponseMessage Get(string id)
        {
            var node = _repository.Find(id);

            if (node == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            var negotiator = Configuration.Services.GetContentNegotiator();
            var result = negotiator.Negotiate(typeof(Node), Request, Configuration.Formatters);

            if (result == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "");

            // a little hack to return the correct media type.
            var mediaType = result.MediaType.MediaType.StartsWith("image/") && node.Image != null ?
                node.Image.MediaType : result.MediaType.MediaType;

            var response = new HttpResponseMessage
            {
                Content = new ObjectContent<Node>(node, result.Formatter, mediaType),
            };

            if (mediaType.StartsWith("image/"))
                response.Headers.CacheControl = new CacheControlHeaderValue()
                {
                    Public = true,
                    MaxAge = new TimeSpan(1, 0, 0, 0)
                };

            return response;
        }

        [HttpPost, ActionName("")]
        public HttpResponseMessage Post([FromBody]NodeModel model)
        {
            if (ModelState.IsValid && model == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Body can't be empty.");

            if (ModelState.IsValid)
            {
                var node = model.ToNode();
                var success = _repository.Create(node);

                if (success)
                    return Request.CreateResponse<Node>(HttpStatusCode.Created, node);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [HttpPut, ActionName("")]
        public async Task<HttpResponseMessage> Put(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            var node = _repository.Find(id);

            if (node == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            var contentType = Request.Content.Headers.ContentType.MediaType;

            if (IsModelType(contentType))
            {
                var model = await Request.Content.ReadAsAsync<NodeModel>();
                node = model.ToNode(id, node.Image);
            }
            else if (IsImageType(contentType))
            {
                var fileId = Guid.NewGuid().ToString();
                var imgStream = await Request.Content.ReadAsStreamAsync();

                var blob = _container.GetBlobReference(fileId);
                blob.UploadFromStream(imgStream);

                node.Image = new Image { Id = fileId, MediaType = contentType, Url = blob.Uri.AbsoluteUri };
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Only support json, jpeg, png, gif");

            if (ValidateModel(node))
            {
                var success = _repository.Update(node);

                if (success)
                    return Request.CreateResponse<Node>(HttpStatusCode.Accepted, node);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [HttpDelete, ActionName("")]
        public HttpResponseMessage Delete(string id)
        {
            var success = _repository.Delete(id);

            if (success)
                return Request.CreateResponse(HttpStatusCode.Accepted);

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Couldn't find any nodes with the specific id; \"" + id + "\".");
        }

        [NonAction]
        private bool ValidateModel(object model)
        {
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            return Validator.TryValidateObject(model, context, results, true);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }

    }
}
