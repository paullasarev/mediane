using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mediane;
using Mediane.Controllers;
using Mediane.Models;
using MvcRouteTester;
using System.Web.Routing;

namespace Mediane.Tests.Controllers
{
    [TestClass]
    public class HomeIndexController
    {
        [TestInitialize]
        public void SetUp()
        {
            Routes = new RouteCollection();
            RouteConfig.RegisterRoutes(Routes);
        }

        RouteCollection Routes;

        [TestMethod]
        public void IndexShouldMakeContentModel()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.Index() as ViewResult;

            ContentModel model = result.Model as ContentModel;
            Assert.IsNotNull(model);

            Assert.AreEqual("New page template", model.Rendered);
        }

        //Install-Package MvcRouteUnitTester

        [TestMethod]
        public void IndexUrlShouldCallMainPage()
        {
            RouteAssert.HasRoute(Routes, "/home/index");
            
            var mainPageRoute = new { controller = "Home", action = "Index", id = "Main_Page" };
            RouteAssert.HasRoute(Routes, "/home/index/Main_Page", mainPageRoute);

            var pageRoute = new { controller = "Home", action = "Index", id = "Other_Page" };
            RouteAssert.HasRoute(Routes, "/home/index/Other_Page", pageRoute);
        }

        [TestMethod]
        public void EmptyUrShouldRedirectToMainPage()
        {
            var mainPageRoute = new { controller = "RootRedirector", action = "Redirect"};
            RouteAssert.HasRoute(Routes, "/", mainPageRoute);
        }

        [TestMethod]
        public void WrongHomeActionShouldRedirectToMainPage()
        {
            var mainPageRoute = new { controller = "RootRedirector", action = "Redirect" };
            RouteAssert.HasRoute(Routes, "/Home/asdf", mainPageRoute);
        }

        [TestMethod]
        public void WrongControllerShouldRedirectToMainPage()
        {
            var mainPageRoute = new { controller = "RootRedirector", action = "Redirect" };
            RouteAssert.HasRoute(Routes, "/Hom", mainPageRoute);
        }

        //[TestMethod]
        //public void WrongUrlShouldNoRoutes()
        //{
        //    RouteAssert.NoRoute(Routes, "/home/index/asf/asdf");
        //    RouteAssert.NoRoute(Routes, "/home/indexa");
        //    RouteAssert.NoRoute(Routes, "/home1/index");
        //}
    }


    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void RootRedirectShouldRedirectToHomeIndex()
        {
            var controller = new RootRedirectorController();

            var result = controller.Redirect() as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Permanent);
            Assert.AreEqual("Home", result.RouteName);
        }

        [TestMethod]
        public void About()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.About() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Contact()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.Contact() as ViewResult;

            Assert.IsNotNull(result);
        }
    }
}
