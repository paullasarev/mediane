using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Mediane.DomainModel;

namespace Mediane.Tests.DomainModel
{
    [TestClass]
    public class ArticleRepositoryTest
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get {return testContextInstance;}
            set {testContextInstance = value;}
        }

        private static string ConnectionString = "DataSource=Temp.sdf";
        private static string ProviderName = "System.Data.SqlServerCe.4.0";
        private static PetaPoco.Database Db;

        [ClassInitialize]
        public static void FixtureSetUp(TestContext context)
        {
            File.Copy("..\\..\\..\\Mediane\\App_Data\\MedianeDb.sdf", "Temp.sdf", true);
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
            catch(Exception e)
            {
            }
        }

        [TestMethod]
        public void RepositoryShouldSaveArticle()
        {
            var repo = new ArticleRepository(ConnectionString, ProviderName);

            Article a = repo.Load("Page 2");
            Assert.AreEqual("Page 2", a.Title);
            a.Content = "Sample content";

            repo.Save(a);
            var title = a.Title;

            ArticleDb aDb = Db.SingleOrDefault<ArticleDb>("SELECT * FROM ARTICLES WHERE Title=@0", title);

            Assert.AreEqual(a.Title, aDb.Title);
            Assert.AreEqual(a.Content, aDb.Content);
        }

    }
}
