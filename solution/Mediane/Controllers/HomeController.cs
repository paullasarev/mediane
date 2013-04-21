using Mediane.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mediane.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new ContentModel();
            model.Rendered = "New page template";

            return View(model);
        }

        //public ActionResult Index(ContentModel model)
        //{
        //    return View("Index", model);
        //}

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}
