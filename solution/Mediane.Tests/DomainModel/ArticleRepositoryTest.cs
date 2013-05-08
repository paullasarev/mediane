﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Mediane.DomainModel;
using Mediane.Tests.Models;

namespace Mediane.Tests.DomainModel
{
    [TestClass]
    public class RepositoryRegistrationTest
    {
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

    [TestClass]
    public class ArticleRepositoryTest
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get {return testContextInstance;}
            set {testContextInstance = value;}
        }

        private const string DbName = "Temp.sdf";
        private static string ConnectionString;
        private static string ProviderName ;
        private static PetaPoco.Database Db;

        [ClassInitialize]
        public static void FixtureSetUp(TestContext context)
        {
            File.Delete(DbName);
            InitDb initDb = new InitDb(DbName, new MedianeSql(), DbName);
            ConnectionString = initDb.GetConnectionString();
            ProviderName = initDb.GetProviderName();
            initDb.CreateDbIfNotExist();

            Db = new PetaPoco.Database(ConnectionString, ProviderName);
        }

        [ClassCleanup]
        public static void FixtureTearDown()
        {
            Db.Dispose();
        }

        [TestInitialize]
        public void SetUP()
        {
        }

        [TestCleanup]
        public void TearDown()
        {
        }

        [TestMethod]
        public void DbShouldBeOpened()
        {
            Assert.IsNotNull(Db);
        }

        [TestMethod]
        public void ArticleDbShouldStoreInBase()
        {
            var a = new ArticleDb();
            a.Title = "My new article";
            a.Content = "Sample content";

            Db.Insert(a);
            var id = a.ArticleId;

            ArticleDb a2 = Db.SingleOrDefault<ArticleDb>(id);

            Assert.AreEqual(id, a2.ArticleId);
            Assert.AreEqual(a.Title, a2.Title);
            Assert.AreEqual(a.Content, a2.Content);
        }

        [TestMethod]
        public void ShouldNotBeAbleSaveTwoSameTitles()
        {
            var a = new ArticleDb();
            a.Title = "Page 1";
            a.Content = "Sample content";
            Db.Insert(a);
            try
            {
                Db.Insert(a);
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod]
        public void RepositoryShouldSaveArticle()
        {
            var repo = new ArticleRepository(ConnectionString, ProviderName);

            var title = "Page 2";
            Article a = repo.Load(title);
            Assert.AreEqual(title, a.Title);
            a.Content = "Sample content";

            repo.Save(a);

            ArticleDb aDb = Db.SingleOrDefault<ArticleDb>("SELECT * FROM ARTICLES WHERE Title=@0", title);

            Assert.AreEqual(a.Title, aDb.Title);
            Assert.AreEqual(a.Content, aDb.Content);
        }

        [TestMethod]
        public void RepositoryShouldLoadArticle()
        {
            var repo = new ArticleRepository(ConnectionString, ProviderName);

            var title = "Page 3";
            Article a = repo.Create(title);
            a.Content = "Sample content";
            repo.Save(a);

            Article a2 = repo.Load(title);

            Assert.AreEqual(a.Title, a2.Title);
            Assert.AreEqual(a.Content, a2.Content);
        }

        [TestMethod]
        public void LoadShouldIgnoreTitleHeadTailSpaces()
        {
            var repo = new ArticleRepository(ConnectionString, ProviderName);

            var title = "Page 4";
            Article a = repo.Create(title);
            a.Content = "Sample content";
            repo.Save(a);

            Article a2 = repo.Load(" Page 4 ");

            Assert.AreEqual(a.Title, a2.Title);
            Assert.AreEqual(a.Content, a2.Content);
        }

        [TestMethod]
        public void NotExistantTitleShouldSetFlagIsNew()
        {
            var repo = new ArticleRepository(ConnectionString, ProviderName);

            var title = "Page 5";

            Article a = repo.Load(title);
            Assert.IsTrue(a.IsNew);

            repo.Save(a);

            Article a2 = repo.Load(title);
            Assert.IsFalse(a2.IsNew);
        }

        [TestMethod]
        public void SaveShouldBeAbleTwice()
        {
            var repo = new ArticleRepository(ConnectionString, ProviderName);

            var title = "Page 6";
            Article a = repo.Create(title);
            a.Content = "Sample content";
            repo.Save(a);

            a.Content = "Sample content 2";
            repo.Save(a);

            Article a2 = repo.Load(title);
            Assert.AreEqual(a.Title, a2.Title);
            Assert.AreEqual(a.Content, a2.Content);
        }
    }
}
