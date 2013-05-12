using Mediane.DomainModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediane.Tests.Functional
{
    class TestEnvironment
    {
        public readonly string BaseUrl = "http://localhost:56773/";
        public readonly string DbName = "MedianeDb.sdf";

        public TestEnvironment()
        {
            SolutionFolder = Path.GetDirectoryName(Path.GetDirectoryName(
                    Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)));
            DbFileName = Path.Combine(SolutionFolder, "Mediane", "App_Data", DbName);

            InitDb();
            WebDriver = CreateWebDriver();
        }

        string SolutionFolder { get; set; }
        string DbFileName { get; set; }

        public void InitDb()
        {
            File.Delete(DbFileName);
            var initDb = new InitDb(DbName, new MedianeSql(), DbFileName);
            initDb.CreateDbIfNotExist();
        }

        static IWebDriver CreateWebDriver()
        {
            IWebDriver webDriver = new FirefoxDriver();
            webDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            
            return webDriver;
        }

        public IWebDriver WebDriver { get; private set; }

        static TestEnvironment TestEnvironmentInstance;
        public static TestEnvironment Environment
        { 
            get
            {
                if (TestEnvironmentInstance == null)
                {
                    TestEnvironmentInstance = new TestEnvironment();
                }

                return TestEnvironmentInstance;
            }
        }

        ~TestEnvironment()
        {
           WebDriver.Quit();
        }

        int CurrentId = 1;
        public string NextPageId()
        {
            string pageId = String.Format("Page {0}", CurrentId);
            ++CurrentId;

            return pageId;
        }
    }
}
