using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mediane.Infrastructure;
using Mediane.Models;
using System.Web.Mvc;
using XmlUnit;
using System.Web;

namespace Mediane.Tests.Views
{
    [TestClass]
    public class InfrastructureTest
    {
        HtmlHelper helper = new HtmlHelper(new ViewContext(), new Utils.FakeViewDataContainer());
        ContentModel model = new ContentModel("main");

        [TestMethod]
        public void ViewMenuShouldContainItems()
        {
            string view = ViewMenuHelper.ViewMenu(helper, model, "Index").ToHtmlString();
            
            Utils.XmlAssert.IsEqual(@"
            <li><a href=""#"" class=""here"">Document</a></li>
            <li><a href=""#"">Comment</a></li>
            <li><a href=""#"">Property</a></li>
            <li><a href=""#"">Attach</a></li>
            ", view);
        }
    }
}
