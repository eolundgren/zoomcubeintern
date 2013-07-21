using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PulseMates.Controllers
{
    public class DemoController : Controller
    {
        //
        // GET: /Demo/
        public ActionResult Index()
        {

            ViewBag.Title = "Demos";
            ViewBag.Message = "Cool stuff is inbound";
            return View();
        }

        public ActionResult Grid()
        {
            ViewBag.Title = "Demos";
            ViewBag.Message = "A simple Grid display of data";
            return View();
        }

        public ActionResult Map()
        {
            ViewBag.Title = "Demos";
            ViewBag.Message = "A simple map display of data";
            return View();
        }

        public ActionResult Timeslider()
        {
            ViewBag.Title = "Demos";
            ViewBag.Message = "A simple timeslider";
            return View();
        }

        public ActionResult Page()
        {
            ViewBag.Title = "Demos";
            ViewBag.Message = "A simple page";
            return View();
        }

        public ActionResult Event()
        {
            ViewBag.Title = "Demos";
            ViewBag.Message = "A simple event";
            return View();
        }

    }
}
