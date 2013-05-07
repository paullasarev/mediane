using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Mediane.DomainModel;
using System.IO;

namespace Mediane.Tests.Functional
{
    [TestClass]
    public class HomePageTest
    {
        static IWebDriver Driver;
        const string BaseUrl = "http://localhost:56773/";
        const string MainPageId = "Main_Page";
        const string DbName = "MedianeDb.sdf";

        [ClassInitialize()]
        public static void Initialize(TestContext testContext)
        {
            var solutionFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)));

            string dbFileName = Path.Combine(solutionFolder, "Mediane", "App_Data", DbName);
            File.Delete(dbFileName);
            var initDb = new InitDb(DbName, new MedianeSql(), dbFileName);
            initDb.CreateDbIfNotExist();

            Driver = new FirefoxDriver();
            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
        }

        [ClassCleanup()]
        public static void Cleanup()
        {
            Driver.Quit();
        }

        [TestMethod]
        public void HomePageShouldContainEditButton()
        {
            var page = new HomePage(Driver, BaseUrl, MainPageId);

            IWebElement editButton = page.GetEditButton();

            Assert.IsNotNull(editButton);
        }

        [TestMethod]
        public void ShouldOpenEditMainPage()
        {
            var page = new HomePage(Driver, BaseUrl, MainPageId);

            IWebElement editButton = page.GetEditButton();
            editButton.Click();

            Assert.AreEqual(BaseUrl + "Home/Edit/Main_Page", Driver.Url);
        }
    }
}
