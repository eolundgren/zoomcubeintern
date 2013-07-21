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

    using Filters;

    public class EventController : ApiController
    {
        #region Field Members

        IEventRepository _repository;

        #endregion

        #region Constructor Members

        public EventController() : this(new Infrastructure.Mongo.EventRepository()) { }
        public EventController(IEventRepository repository)
        {
            _repository = repository;
        }

        #endregion

        // GET api/event

        [HttpGet, ActionName("")]
        public IQueryable<Event> GetAll()
        {
            return _repository.FindAll();
        }

        [HttpGet, ActionName("")]
        public Event GetById(string id)
        {
            return _repository.Find(id);
        }

        [HttpGet, ActionName("Image")]
        public HttpResponseMessage GetImage(string id, ImageFilter size)
        {
            var e = _repository.Find(id);

            if (e == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Event Not Found.");

            return null;
        }

        [HttpPost, ActionName("")]
        public HttpResponseMessage CreateEvent([FromBody]EventModel model)
        {
            if (ModelState.IsValid)
            {
                var node = model.ToEvent();
                var success = _repository.Create(node);

                if (success)
                    return Request.CreateResponse<Event>(HttpStatusCode.Created, node);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [HttpPut, ActionName("")]
        public HttpResponseMessage UpdateEvent(string id, [FromBody]EventModel model)
        {
            if (ModelState.IsValid)
            {
                var node = model.ToEvent(id);
                var success = _repository.Create(node);

                if (success)
                    return Request.CreateResponse<Event>(HttpStatusCode.Created, node);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        // DELETE api/event/5
        [HttpDelete, ActionName("")]
        public HttpResponseMessage DeleteEvent(string id)
        {
            var success = _repository.Delete(id);

            if (success)
                return Request.CreateResponse(HttpStatusCode.Accepted);

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Couldn't find any nodes with the specific id; \"" + id + "\".");
        }

        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }
    }
}
