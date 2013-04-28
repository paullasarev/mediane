using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mediane.Infrastructure;
using Mediane.DomainModel;
using System.Web.Mvc;
using XmlUnit;
using System.Web;
using Mediane.Tests.Models;

namespace Mediane.Tests.Views
{
    [TestClass]
    public class InfrastructureTest
    {
        HtmlHelper helper = new HtmlHelper(new ViewContext(), new Utils.FakeViewDataContainer());

        FakeContentModelRepository repository = new FakeContentModelRepository();
        ContentModel model;

        public InfrastructureTest()
        {
            model = repository.Create("main");
        }

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
