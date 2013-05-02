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
        FakeArticleRepository repository = new FakeArticleRepository();
        Article model;

        public ContentModelTest()
        {
            model = repository.Load("main");
        }

        [TestMethod]
        public void ShouldHaveIdProperty()
        {
            Assert.AreEqual("main", model.Title);
        }

        [TestMethod]
        public void ShouldTrimIdSpaces()
        {
            Article model = repository.Load("main");
            Assert.AreEqual("main", model.Title);
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
                var repository = repositoryTable.Locate<IArticleRepository>();
                Assert.Fail("Should not locate Article");
            }
            catch
            {
            }

            repositoryTable.Register<IArticleRepository>(new FakeArticleRepository());
            var repo = repositoryTable.Locate<IArticleRepository>();
            Assert.IsNotNull(repo);
        }

        [TestMethod]
        public void RepositoryConfigShouldRegisterContentModelRepository()
        {
            var repositoryTable = RepositoryTable.Repositories;
            try
            {
                var repository = repositoryTable.Locate<IArticleRepository>();
                Assert.Fail("Should not locate Article");
            }
            catch
            {
            }

            RepositoryConfig.RegisterRepositories(repositoryTable);
            var repo = repositoryTable.Locate<IArticleRepository>();
            Assert.IsNotNull(repo);
            Assert.IsNotNull(repo as ArticleRepository);
        }
    
    }
}
