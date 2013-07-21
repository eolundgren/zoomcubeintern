using PulseMates.Infrastructure.Filters;
using PulseMates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PulseMates.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = ApplicationInfo.Title;
            ViewBag.Message = ApplicationInfo.Description;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Title = "About";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Title = "Contact";
            ViewBag.Message = "Don’t Save Data, Preserve Events";
            return View();
        }
    }
}
