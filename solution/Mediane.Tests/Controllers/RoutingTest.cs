using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Routing;
using MvcRouteTester;

namespace Mediane.Tests.Controllers
{
    [TestClass]
    public class RoutingTest
    {
        [TestInitialize]
        public void SetUp()
        {
            Routes = new RouteCollection();
            RouteConfig.RegisterRoutes(Routes);
        }

        RouteCollection Routes;

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
            var mainPageRoute = new { controller = "RootRedirector", action = "Redirect" };
            RouteAssert.HasRoute(Routes, "/", mainPageRoute);
        }

        [TestMethod]
        public void WrongHomeActionShouldRedirectToMainPage()
        {
            var mainPageRoute = new { controller = "RootRedirector", action = "Redirect" };
            RouteAssert.HasRoute(Routes, "/Home/asdf", mainPageRoute);
        }

        [TestMethod]
        public void WrongHomeAction2ShouldRedirectToMainPage()
        {
            var mainPageRoute = new { controller = "RootRedirector", action = "Redirect" };
            RouteAssert.HasRoute(Routes, "/Home", mainPageRoute);
        }

        [TestMethod]
        public void WrongHomeAction3ShouldRedirectToMainPage()
        {
            var mainPageRoute = new { controller = "RootRedirector", action = "Redirect" };
            RouteAssert.HasRoute(Routes, "/Home/Index", mainPageRoute);
        }

        [TestMethod]
        public void WrongHomeAction4ShouldRedirectToMainPage()
        {
            var mainPageRoute = new { controller = "RootRedirector", action = "Redirect" };
            RouteAssert.HasRoute(Routes, "/Home/Edit", mainPageRoute);
        }

        [TestMethod]
        public void WrongControllerShouldRedirectToMainPage()
        {
            var mainPageRoute = new { controller = "RootRedirector", action = "Redirect" };
            RouteAssert.HasRoute(Routes, "/Hom", mainPageRoute);
        }

        [TestMethod]
        public void EditUrlShouldRoute()
        {
            var route = new { controller = "Home", action = "Edit", id = "Main_Page" };
            RouteAssert.HasRoute(Routes, "/home/edit/Main_Page", route);
        }
        
        //[TestMethod]
        //public void EditWithoutIdShouldNoRoute()
        //{
        //    RouteAssert.IsIgnoredRoute(Routes, "/home/edit");
        //}

        [TestMethod]
        public void EditUrlShouldRouteToAnyId()
        {
            var route = new { controller = "Home", action = "Edit", id = "Main2_Page" };
            RouteAssert.HasRoute(Routes, "/home/edit/Main2_Page", route);
        }

        [TestMethod]
        public void SaveUrlShouldRoute()
        {
            var route = new { controller = "Home", action = "Save", id = "Main_Page" };
            RouteAssert.HasRoute(Routes, "/home/save/Main_Page", route);
        }

        [TestMethod]
        public void SaveUrlShouldRouteToAnyId()
        {
            var route2 = new { controller = "Home", action = "Save", id = "Main2_Page" };
            RouteAssert.HasRoute(Routes, "/home/save/Main2_Page", route2);
        }
    }
}
