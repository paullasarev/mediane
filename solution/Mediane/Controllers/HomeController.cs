using Mediane.DomainModel;
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
            return RedirectToRoute("Home", new { Controller = "Home", Action = "Index", Id = "Main_Page"});
        }
    }

    public class HomeController : Controller
    {
        private IArticleRepository repository;

        public HomeController(IArticleRepository newRepository)
        {
            repository = newRepository;
        }

        public HomeController()
        {
            repository = RepositoryTable.Repositories.Locate<IArticleRepository>();
        }

        public ActionResult Index(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home", new { Id = "Main_Page" });
            }

            var model = repository.Load(id);

            return View("Index", model);
        }

        public ActionResult Edit(string id)
        {
            var model = repository.Load(id);

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

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(string id, string Content, string action)
        {
            var model = repository.Load(id);
            if (action == "Save")
            {
                model.Content = Content;
                repository.Save(model);
            }

            return RedirectToAction("Index", "Home", new { Id = model.Title });
        }
    }
}
