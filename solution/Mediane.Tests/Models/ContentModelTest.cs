using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mediane.DomainModel;
using XmlUnit.Xunit;

//Install-Package XmlUnit.Xunit

namespace Mediane.Tests.Models
{
    /// <summary>
    /// Summary description for ContentModelTest
    /// </summary>
    [TestClass]
    public class ContentModelTest
    {
        FakeContentModelRepository repository = new FakeContentModelRepository();
        ContentModel model;

        public ContentModelTest()
        {
            model = repository.Create("main");
        }

        [TestMethod]
        public void ShouldHaveIdProperty()
        {
            Assert.AreEqual("main", model.Id);
        }

        [TestMethod]
        public void ShouldTrimIdSpaces()
        {
            ContentModel model = repository.Create("main");
            Assert.AreEqual("main", model.Id);
        }

        [TestMethod]
        public void ShouldGetSetContent()
        {
            string content = "The wiki markup";

            model.Content = content;
            Assert.AreEqual(content, model.Content);
        }

        [TestMethod]
        public void NullContentAssignDoesNotAffectState()
        {
            string content = "The wiki markup";
            model.Content = content;
            model.Content = null;
            Assert.AreEqual(content, model.Content);
        }

        [TestMethod]
        public void RenderedShouldProduceParagraphTag()
        {
            model.Content = "one paragraph";
            Utils.XmlAssert.IsEqual(" <p>one paragraph</p>\n ", model.Rendered);
        }

        [TestMethod]
        public void RepositoryShouldRegisterComponentModelRepository()
        {
            var repositoryTable = new RepositoryTable();
            try
            {
                var repository = repositoryTable.Locate<IContentModelRepository>();
                Assert.Fail("Should not locate ContentModel");
            }
            catch
            {
            }

            repositoryTable.Register<IContentModelRepository>(new FakeContentModelRepository());
            var repo = repositoryTable.Locate<IContentModelRepository>();
            Assert.IsNotNull(repo);
        }

        [TestMethod]
        public void RepositoryConfigShouldRegisterContentModelRepository()
        {
            var repositoryTable = RepositoryTable.Repositories;
            try
            {
                var repository = repositoryTable.Locate<IContentModelRepository>();
                Assert.Fail("Should not locate ContentModel");
            }
            catch
            {
            }

            RepositoryConfig.RegisterRepositories(repositoryTable);
            var repo = repositoryTable.Locate<IContentModelRepository>();
            Assert.IsNotNull(repo);
            Assert.IsNotNull(repo as ContentModelRepository);
        }
    
    }
}
