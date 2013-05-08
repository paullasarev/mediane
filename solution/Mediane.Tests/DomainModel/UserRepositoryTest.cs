using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mediane.DomainModel;
using System.IO;

namespace Mediane.Tests.DomainModel
{
    [TestClass]
    public class UserRepositoryTest
    {
        private const string DbName = "Temp.sdf";
        private static string ConnectionString;
        private static string ProviderName;
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

        [TestMethod]
        public void RepositoryConfigShouldRegisterUserRepository()
        {
            var repositoryTable = RepositoryTable.Repositories;
            try
            {
                var repository = repositoryTable.Locate<IUserRepository>();
                Assert.Fail("Should not locate IUserRepository");
            }
            catch
            {
            }

            RepositoryConfig.RegisterRepositories(repositoryTable);
            
            var repo = repositoryTable.Locate<IUserRepository>();
            Assert.IsNotNull(repo);
            Assert.IsInstanceOfType(repo, typeof(UserRepository));
        }

        [TestMethod]
        public void UserDbShouldStoreInBase()
        {
            string username = "user1";
            var m = new UserDb();
            m.Username = username;
            m.Password = "asdf";

            Db.Insert(m);

            UserDb m2 = Db.SingleOrDefault<UserDb>((object)username);

            Assert.AreEqual(m.Username, m2.Username);
            Assert.AreEqual(m.Password, m2.Password);
        }

        [TestMethod]
        public void CreateShouldAddToUserTable()
        {
            var repo = new UserRepository(ConnectionString, ProviderName);

            string username = "user2";
            string password = "asdf2";

            repo.Create(username, password);

            UserDb m2 = Db.SingleOrDefault<UserDb>((object)username);

            Assert.AreEqual(username, m2.Username);
            Assert.AreEqual(password, m2.Password);
        }

        [TestMethod]
        public void CreateShouldFailForExistantUser()
        {
            var repo = new UserRepository(ConnectionString, ProviderName);

            string username = "user3";
            string password = "asdf3";

            repo.Create(username, password);

            try
            {
                repo.Create(username, password);
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod]
        public void ValidateShouldTrueForExistantUser()
        {
            var repo = new UserRepository(ConnectionString, ProviderName);

            string username = "user4";
            string password = "asdf4";

            repo.Create(username, password);

            Assert.IsTrue(repo.Validate(username, password));
        }

        [TestMethod]
        public void ValidateShouldFalseForNonExistantUser()
        {
            var repo = new UserRepository(ConnectionString, ProviderName);

            string username = "user5";
            string password = "asdf5";

            repo.Create(username, password);

            Assert.IsFalse(repo.Validate("user6", password));
        }

        [TestMethod]
        public void ValidateShouldFalseForWrongPassword()
        {
            var repo = new UserRepository(ConnectionString, ProviderName);

            string username = "user6";
            string password = "asdf6";

            repo.Create(username, password);

            Assert.IsFalse(repo.Validate(username, "asdf7"));
        }
    }
}
