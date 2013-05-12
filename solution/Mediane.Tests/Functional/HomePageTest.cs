using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Mediane.DomainModel;
using System.IO;
using System.Web;

namespace Mediane.Tests.Functional
{
    [TestClass]
    public class HomePageTest
    {
        static string PageId;
        static TestEnvironment environment = TestEnvironment.Environment;
        static IWebDriver Driver;

        [ClassInitialize()]
        public static void Initialize(TestContext testContext)
        {
            Driver = environment.WebDriver;
            PageId = environment.NextPageId();
        }

        [TestMethod]
        public void HomePageShouldContainEditButton()
        {
            var page = new HomePage(Driver, environment.BaseUrl, PageId);

            IWebElement editButton = page.GetEditButton();

            Assert.IsNotNull(editButton);
        }

        [TestMethod]
        public void ShouldOpenEditMainPage()
        {
            var page = new HomePage(Driver, environment.BaseUrl, PageId);

            IWebElement editButton = page.GetEditButton();
            editButton.Click();

            Assert.AreEqual(environment.BaseUrl + "Home/Edit/" + HttpUtility.UrlPathEncode(PageId), Driver.Url);
        }
    }
}
