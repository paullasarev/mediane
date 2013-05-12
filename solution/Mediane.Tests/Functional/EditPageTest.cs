using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Mediane.Tests.Functional
{
    [TestClass]
    public class EditPageTest
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
        public void ShouldContainSaveButton()
        {
            var page = new EditPage(Driver, environment.BaseUrl, PageId);

            IWebElement button = page.GetSaveButton();

            Assert.IsNotNull(button);
        }

        [TestMethod]
        public void ShouldContainTypeFieldAndButton()
        {
            var page = new EditPage(Driver, environment.BaseUrl, PageId);

            IWebElement field = page.GetTypeField();
            Assert.IsNotNull(field);

            IWebElement button = page.GetTypeButton();
            Assert.IsNotNull(button);
        }
    }
}
