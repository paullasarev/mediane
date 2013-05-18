using Mediane.DomainModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediane.Tests.Functional
{
    class TestEnvironment
    {
        const int iisPort = 2020;
        private readonly string _applicationName = "Mediane";
        private Process _iisProcess;

        public readonly string BaseUrl = "http://localhost:" + iisPort.ToString() + "/";
            // "http://localhost:56773/";

        public readonly string DbName = "MedianeDb.sdf";
        static TestEnvironment TestEnvironmentInstance;
        //string SolutionFolder { get; set; }
        //string DbFileName { get; set; }
        public IWebDriver WebDriver { get; private set; }

        public TestEnvironment()
        {
            string solutionFolder = GetSolutionFolder();
            string dbFileName = Path.Combine(solutionFolder, "Mediane", "App_Data", DbName);
            InitDb(dbFileName);
            
            StartIIS();

            WebDriver = CreateWebDriver();
        }

        ~TestEnvironment()
        {
            WebDriver.Quit();
            StopIIS();
        }


        private void StartIIS() 
        {
            var applicationPath = GetApplicationPath(_applicationName);
            var programFiles = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles);

            _iisProcess = new Process();
            _iisProcess.StartInfo.FileName = programFiles + "\\IIS Express\\iisexpress.exe";
            _iisProcess.StartInfo.Arguments = string.Format("/path:\"{0}\" /port:{1}", applicationPath, iisPort);
            _iisProcess.Start();
        }

        private void StopIIS()
        {
            if (_iisProcess.HasExited == false)
            {
                _iisProcess.Kill();
            }
        }

        private string GetApplicationPath(string applicationName)
        {
            return Path.Combine(GetSolutionFolder(), applicationName);
        }

        private static string GetSolutionFolder()
        {
            var solutionFolder = Path.GetDirectoryName(Path.GetDirectoryName(
                Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)));
            return solutionFolder;
        }

        public void InitDb(string dbFileName)
        {
            File.Delete(dbFileName);
            var initDb = new InitDb(DbName, new MedianeSql(), dbFileName);
            initDb.CreateDbIfNotExist();
        }

        static IWebDriver CreateWebDriver()
        {
            IWebDriver webDriver = new FirefoxDriver();
            webDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            
            return webDriver;
        }

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

        int CurrentId = 1;
        public string NextPageId()
        {
            string pageId = String.Format("Page {0}", CurrentId);
            ++CurrentId;

            return pageId;
        }
    }
}
