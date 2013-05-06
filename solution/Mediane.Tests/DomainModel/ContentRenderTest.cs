using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mediane.DomainModel;
using Mediane.Tests.Models;

namespace Mediane.Tests.DomainModel
{
    [TestClass]
    public class ContentRenderTest
    {
        FakeArticleRepository repository = new FakeArticleRepository();
        Article model;

        public ContentRenderTest()
        {
            model = repository.Load("main");
        }

        [TestMethod]
        public void RenderedShouldProduceParagraphTag()
        {

            model.Content = "one paragraph";
            Utils.XmlAssert.IsEqual(" <p>one paragraph</p>\n ", model.Rendered);
        }

        [TestMethod]
        public void InternalLinkShouldRenderToLink()
        {
            model.Content = "[[Internal_page]]";
            Utils.XmlAssert.IsEqual("<p><a href=\"/Home/Index/Internal_page\">Internal_page</a></p>", model.Rendered);
        }

        [TestMethod]
        public void TextAndInternalLinkShouldRender()
        {
            model.Content = "name [[Internal_page]]";
            Utils.XmlAssert.IsEqual("<p>name <a href=\"/Home/Index/Internal_page\">Internal_page</a></p>", model.Rendered);
        }

        [TestMethod]
        public void TextWithBracketAndInternalLinkShouldRender()
        {
            model.Content = "na[me [[Internal_page]]";
            Utils.XmlAssert.IsEqual("<p>na[me <a href=\"/Home/Index/Internal_page\">Internal_page</a></p>", model.Rendered);
        }

        [TestMethod]
        public void UnclosedInternalLinkShouldRenderToText()
        {
            model.Content = "name [[Internal_page";
            Utils.XmlAssert.IsEqual("<p>name Internal_page</p>", model.Rendered);
        }

        [TestMethod]
        public void LinkWithSpaceShouldRender()
        {
            model.Content = "name [[Internal page]]";
            Utils.XmlAssert.IsEqual("<p>name <a href=\"/Home/Index/Internal page\">Internal page</a></p>", model.Rendered);
        }

        [TestMethod]
        public void TextAfterLinkShouldRender()
        {
            model.Content = "name [[Internal page]] link";
            Utils.XmlAssert.IsEqual("<p>name <a href=\"/Home/Index/Internal page\">Internal page</a> link</p>", model.Rendered);
        }

        [TestMethod]
        public void EmptyLineShouldRenderParagraph()
        {
            model.Content = "name\n\nlink";
            Utils.XmlAssert.IsEqual("<p>name</p><p>link</p>", model.Rendered);
        }

        [TestMethod]
        public void NewLineShouldNotRenderParagraph()
        {
            model.Content = "name\nlink";
            Utils.XmlAssert.IsEqual("<p>name\nlink</p>", model.Rendered);
        }

        [TestMethod]
        public void RetLineShouldNotPassToRender()
        {
            model.Content = "name\r\nlink";
            Utils.XmlAssert.IsEqual("<p>name\nlink</p>", model.Rendered);
        }

        [TestMethod]
        public void AloneRetLineShouldNotPassToRender()
        {
            model.Content = "name\rlink";
            Utils.XmlAssert.IsEqual("<p>name link</p>", model.Rendered);
        }

        [TestMethod]
        public void RetLineShouldRenderParagraph()
        {
            model.Content = "name\r\n\r\nlink";
            Utils.XmlAssert.IsEqual("<p>name</p><p>link</p>", model.Rendered);
        }

        [TestMethod]
        public void TwoEmptyLineShouldRenderOneParagraph()
        {
            model.Content = "name\n\n\nlink";
            Utils.XmlAssert.IsEqual("<p>name</p><p>link</p>", model.Rendered);
        }
    }
}
