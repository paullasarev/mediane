using Mediane.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mediane.Controllers
{
    public class RootRedirectorController : Controller
    {
        public ActionResult Redirect()
        {
            return RedirectToRoutePermanent("Home", new { Controller = "Home", Action = "Index", Id = "Main_Page"});
        }
    }

    public class HomeController : Controller
    {
        public ActionResult Index(string id = "")
        {
            var model = new ContentModel(id);
            model.Content = "New page template";

            return View(model);
        }

        public ActionResult Edit(string id)
        {
            var model = new ContentModel(id);
            model.Content = "[" + id + "] content";
            return View("Edit", model);
        }

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


        public ActionResult Save(string id, string Content, string action)
        {
            var model = new ContentModel(id);
            model.Content = Content;
            return RedirectToAction("Index", "Home", new { Id = model.Id });
        }
    }
}
