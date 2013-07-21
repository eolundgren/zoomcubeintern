namespace PulseMates.Controllers.WebAPI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Models;
    using Models.Storage;

    using Infrastructure;
    using Infrastructure.Mongo;

    public class PageController : ApiController
    {
        IPageRepository _pageRepo;
        INodeRepository _nodeRepo;

        public PageController() : this(new PageRepository(), new NodeRepository()) { }
        public PageController(IPageRepository pageRepository, INodeRepository nodeRepository)
        {
            _pageRepo = pageRepository;
            _nodeRepo = nodeRepository;
        }

        // GET api/event
        [ActionName("")]
        public IQueryable<Page> Get()
        {
            return _pageRepo.FindAll();
        }

        [ActionName("Item")]
        public IQueryable<Node> GetNodes(string id)
        {
            var page = _pageRepo.Find(id);
            return _nodeRepo.FindAll(page.Tags.ToArray());
        }

        // GET api/event/5
        [ActionName("")]
        public Page Get(string id)
        {
            return _pageRepo.Find(id);
        }

        // POST api/event
        [ActionName("")]
        public HttpResponseMessage Post([FromBody]PageModel model)
        {
            if (ModelState.IsValid)
            {
                var node = model.ToPage();
                var success = _pageRepo.Create(node);

                if (success)
                    return Request.CreateResponse<Page>(HttpStatusCode.Created, node);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        // PUT api/event/5
        [ActionName("")]
        public HttpResponseMessage Put(string id, [FromBody]PageModel model)
        {
            if (ModelState.IsValid)
            {
                var node = model.ToPage(id);
                var success = _pageRepo.Create(node);

                if (success)
                    return Request.CreateResponse<Page>(HttpStatusCode.Created, node);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        // DELETE api/event/5
        [ActionName("")]
        public HttpResponseMessage Delete(string id)
        {
            var success = _pageRepo.Delete(id);

            if (success)
                return Request.CreateResponse(HttpStatusCode.Accepted);

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Couldn't find any nodes with the specific id; \"" + id + "\".");
        }

        protected override void Dispose(bool disposing)
        {
            _nodeRepo.Dispose();
            _pageRepo.Dispose();

            base.Dispose(disposing);
        }
    }
}