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
        public void IndexUrlShouldRedirectToMainPage()
        {
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            RouteAssert.HasRoute(routes, "/home/index");
            
            var mainPageRoute = new { controller = "Home", action = "Index", id = "Main_Page" };
            RouteAssert.HasRoute(routes, "/home/index/Main_Page", mainPageRoute);
            RouteAssert.HasRoute(routes, "/home/index", mainPageRoute);
        }

        [TestMethod]
        public void WrongUrlShouldNoRoutes()
        {
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            RouteAssert.NoRoute(routes, "/home/index/asf/asdf");
            RouteAssert.NoRoute(routes, "/home/indexa");
            RouteAssert.NoRoute(routes, "/home1/index");
        }
    }


    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
