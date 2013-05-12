using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Web.Mvc;
using System.Web;

namespace Mediane.Tests.Functional
{
    public class HomePage
    {
        private IWebDriver Driver;
        private string BaseUrl;
        private string PageId;

        public HomePage(IWebDriver driver, string baseUrl, string id)
        {
            Driver = driver;
            BaseUrl = baseUrl;
            PageId = id;

            Driver.Navigate().GoToUrl(BaseUrl + "Home/Index/" + PageId);
        }

        public string EncodedId
        {
            get
            {
                return HttpUtility.UrlPathEncode(PageId);
            }
        }

        public IWebElement GetEditButton()
        {
            IWebElement query = Driver.FindElement(By.CssSelector(".toolbox li a[href='/Home/Edit/" + EncodedId + "']"));
            return query;
        }
    }
}
