using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Mediane.Tests.Functional
{
    public class EditPage
    {
        private IWebDriver Driver;
        private string BaseUrl;
        private string PageId;

        public EditPage(IWebDriver driver, string baseUrl, string pageId)
        {
            Driver = driver;
            BaseUrl = baseUrl;
            PageId = pageId;

            Driver.Navigate().GoToUrl(BaseUrl + "Home/Edit/" + PageId);
        }

        public string EncodedId
        {
            get
            {
                return HttpUtility.UrlPathEncode(PageId);
            }
        }

        public IWebElement GetSaveButton()
        {
            IWebElement element = Driver.FindElement(By.CssSelector("form#ContentEdit input#ok"));
            return element;
        }

        internal IWebElement GetTypeField()
        {
            IWebElement element = Driver.FindElement(By.CssSelector("form#PageProperty select#Type"));
            return element;
        }

        internal IWebElement GetTypeButton()
        {
            IWebElement element = Driver.FindElement(By.CssSelector("form#PageProperty input#typeOk"));
            return element;
        }
    }
}
