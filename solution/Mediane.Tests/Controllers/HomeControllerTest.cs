using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mediane;
using Mediane.Controllers;
using Mediane.DomainModel;
using MvcRouteTester;
using System.Web.Routing;
using System.Collections.Specialized;
using Mediane.Tests.Models;

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

        [TestMethod]
        public void EditUrlShouldRoute()
        {
            var route = new { controller = "Home", action = "Edit", id = "Main_Page" };
            RouteAssert.HasRoute(Routes, "/home/edit/Main_Page", route);
        }

        [TestMethod]
        public void SaveUrlShouldRoute()
        {
            var route = new { controller = "Home", action = "Save", id = "Main_Page" };
            RouteAssert.HasRoute(Routes, "/home/save/Main_Page", route);
        }
    }


    [TestClass]
    public class HomeControllerTest
    {
        public HomeControllerTest()
        {
            RepositoryTable.Repositories.Register<IContentModelRepository>(new FakeContentModelRepository());
            var repo = RepositoryTable.Repositories.Locate<IContentModelRepository>();
            var model = repo.Create("main");
            repo.Save(model);
        }

        [TestMethod]
        public void IndexShouldMakeContentModel()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.Index("main") as ViewResult;

            ContentModel model = result.Model as ContentModel;
            Assert.IsNotNull(model);

            Assert.IsTrue(model.Rendered.Contains("New page template"));
        }

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

        [TestMethod]
        public void EditShouldMakeEditView()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.Edit("main") as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Edit", result.ViewName);
            Assert.AreEqual("main", (result.Model as ContentModel).Id);
        }

        [TestMethod]
        public void SaveShouldRedirectToIndex()
        {
            HomeController controller = new HomeController();

            var result = controller.Save(" main", "new content", "Save") as RedirectToRouteResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("main", result.RouteValues["Id"]);
        }

        [TestMethod]
        public void SaveShouldStoreNewContent()
        {
            HomeController controller = new HomeController();

            var result = controller.Save(" main", "new content", "Save") as RedirectToRouteResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("main", result.RouteValues["Id"]);
        }

        //[TestMethod]
        //public void ShouldBindSaveController()
        //{
        //    // Arrange
        //    var formCollection = new NameValueCollection { 
        //        { "foo.month", "2" },
        //        { "foo.day", "12" },
        //        { "foo.year", "1964" }
        //    };

        //    var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
        //    var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(ContentModel));

        //    var bindingContext = new ModelBindingContext
        //    {
        //        ModelName = "foo",
        //        ValueProvider = valueProvider,
        //        ModelMetadata = modelMetadata
        //    };

        //    var b = new DefaultModelBinder();
        //    ControllerContext controllerContext = new ControllerContext();

        //    // Act
        //    DateTime result = (DateTime)b.BindModel(controllerContext, bindingContext);

        //    // Assert
        //    Assert.AreEqual(DateTime.Parse("1964-02-12 12:00:00 am"), result);
        //}
    }
}
