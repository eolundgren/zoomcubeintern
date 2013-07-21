using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PulseMates.Models.Storage;
using PulseMates.Infrastructure.Mongo;
using PulseMates.Models;

namespace PulseMates.Controllers
{
    [Authorize]
    public class ParticipateController : Controller
    {
        #region Field Members

        private IDataSourceRepository _repository;

        #endregion

        #region Constructor Members

        public ParticipateController()
            : this(new DataSourceRepository()) { }

        public ParticipateController(IDataSourceRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _repository = repository;
        }

        #endregion

        //
        // GET: /Participate/
        public ActionResult Index(string id = "")
        {
            var model = string.IsNullOrEmpty(id) ? 
                _repository.FindAll().FirstOrDefault() :
                _repository.Find(id);

            ViewBag.Title = model.Name;
            ViewBag.Message = model.Description;
            return View(model);
        }

        public ActionResult Upload(string id)
        {
            var model = _repository.Find(id);

            ViewBag.Id = model.Id;
            ViewBag.Title = model.Name;
            ViewBag.Message = "Contribute";  
            return View(model.CreateValue());
        }

        public ActionResult Facebook(string id)
        {
            var model = _repository.Find(id);

            ViewBag.Id = model.Id;
            ViewBag.Title = model.Name;
            ViewBag.Message = "Contribute";

            return View(model.CreateValue());
        }

        //public ActionResult Create(string id)
        //{
        //    var model = new DatasetModel();

        //    ViewBag.Title = "Participate";
        //    ViewBag.Message = "Create Dataset";
        //    ViewBag.DatasetId = id;
        //    return View(model);
        //}

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var model = _repository.Find(id);

            ViewBag.Id = id;
            ViewBag.Title = model.Name;
            ViewBag.Message = "Manage Dataset";
            ViewBag.DefinitionTypes = ReflectionHelper.GetNames();
            return View(new DatasetModel(model));
        }

        [HttpPost]
        public ActionResult Edit(string id, DatasetModel model)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(model.ToDataset(id));
                return RedirectToAction("Index", new { Id = id });
            }

            return null;
        }

    }
}