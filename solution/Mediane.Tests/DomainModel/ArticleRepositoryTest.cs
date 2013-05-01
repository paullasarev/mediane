using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Mediane.DomainModel;

namespace Mediane.Tests.DomainModel
{
    [PetaPoco.TableName("articles")]
    [PetaPoco.PrimaryKey("ArticleId")]
    public class ArticleDb
    {
        public long ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
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

        //[TestMethod]
        //public void RepositoryShouldSaveArticle()
        //{
        //    var repo = new ArticleRepository(Db);

        //    var a = repo.Load("Page 2");
        //    Assert.AreEqual("Page 2", a.Id);

        //    a.Content = "Sample content";

        //    Db.Insert(a);
        //    var id = a.ArticleId;

        //    ArticleDb a2 = Db.SingleOrDefault<ArticleDb>(id);

        //    Assert.AreEqual(id, a2.ArticleId);
        //    Assert.AreEqual(a.Title, a2.Title);
        //    Assert.AreEqual(a.Content, a2.Content);
        //}

    }
}
