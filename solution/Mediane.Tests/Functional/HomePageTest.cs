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
        static QueryObject queryObject = new QueryObject();
        static string ConnectionString;
        private static string ProviderName = "System.Data.SqlServerCe.4.0";

        static HomePageTest()
        {
            var solutionFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)));
            ConnectionString = "DataSource=" + Path.Combine(solutionFolder, "Mediane", "App_Data", "MedianeDb.sdf");
        }

        public static void ClearDb()
        {
            using (var db = new PetaPoco.Database(ConnectionString, ProviderName))
            {
                db.Execute(queryObject.ClearAll);
            }
        }

        [ClassInitialize()]
        public static void Initialize(TestContext testContext)
        {
            Driver = new FirefoxDriver();
            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            ClearDb();
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
