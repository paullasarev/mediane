using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mediane.Controllers;
using System.Web.Mvc;
using Mediane.Models;
using System.IO;
using System.Web.Routing;
using Xipton.Razor;
using System.Web;

namespace Mediane.Tests.Views
{
    //Install-Package RazorMachine

    static class Constants
    {
        public static string Config =
@"<xipton.razor>

  <contentProviders>
    <clear/>
    <add type=""Xipton.Razor.Core.ContentProvider.FileContentProvider"" rootFolder=""../../../Mediane/Views""/> 
  </contentProviders>

  <namespaces>
    <add namespace=""RazorMachineFakes""/>
    <add namespace=""System.Web.Mvc""/>
  </namespaces>

  <references>
    <add reference=""System.Web.dll""/>
  </references>
 
</xipton.razor>";
    }

    [TestClass]
    public class HomeIndexViewTest
    {
        RazorMachine Engine = new RazorMachine(Constants.Config);
        const string content = "Wiki content";
        ContentModel model = new ContentModel("main");
        ITemplate template;

        public HomeIndexViewTest()
        {
            model.Content = content;
            template = Engine.ExecuteUrl("~/Home/Index", model, null);
        }

        [TestMethod]
        public void RenderedHtmlShouldContainModelRenderProperty()
        {
            var rendered = template.Result;

            Assert.IsTrue(rendered.Contains(model.Rendered));
        }

        [TestMethod]
        public void RenderedHtmlShouldContainPartialLogin()
        {
            var rendered = template.Result;

            Assert.IsTrue(rendered.Contains("<partial>_LoginPartial</partial>"));
        }

    }

    [TestClass]
    public class HomeEditViewTest
    {
        RazorMachine Engine = new RazorMachine(Constants.Config);
        const string content = "Wiki content";
        ContentModel model = new ContentModel("main");
        ITemplate template;

        public HomeEditViewTest()
        {
            model.Content = content;
            template = Engine.ExecuteUrl("~/Home/Edit", model, null);
        }

        [TestMethod]
        public void RenderedHtmlShouldContainModelRenderProperty()
        {
            var rendered = template.Result;

            Assert.IsTrue(rendered.Contains(content));
        }

    }

}
