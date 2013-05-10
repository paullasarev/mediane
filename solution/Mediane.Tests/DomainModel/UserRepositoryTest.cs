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
            m.UserName = username;

            Db.Insert(m);

            UserDb m2 = Db.SingleOrDefault<UserDb>(m.UserId);

            Assert.AreEqual(m.UserName, m2.UserName);
        }

        [TestMethod]
        public void CreateShouldAddToUserAndMembershipTable()
        {
            var repo = new UserRepository(ConnectionString, ProviderName);

            string username = "user2";
            string password = "asdf2";

            int userId = repo.CreateLocal(username, password);

            UserDb m2 = Db.SingleOrDefault<UserDb>(userId);
            Assert.AreEqual(username, m2.UserName);

            MembershipDb m3 = Db.SingleOrDefault<MembershipDb>(userId);
            Assert.AreEqual(password, m3.Password);
            Assert.IsTrue(m3.IsConfirmed);
        }

        [TestMethod]
        public void CreateShouldFailForExistantUser()
        {
            var repo = new UserRepository(ConnectionString, ProviderName);

            string username = "user3";
            string password = "asdf3";

            repo.CreateLocal(username, password);

            try
            {
                repo.CreateLocal(username, password);
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

            repo.CreateLocal(username, password);

            Assert.IsTrue(repo.Validate(username, password));
        }

        [TestMethod]
        public void ValidateShouldFalseForNonExistantUser()
        {
            var repo = new UserRepository(ConnectionString, ProviderName);

            string username = "user5";
            string password = "asdf5";

            repo.CreateLocal(username, password);

            Assert.IsFalse(repo.Validate("user6", password));
        }

        [TestMethod]
        public void ValidateShouldFalseForWrongPassword()
        {
            var repo = new UserRepository(ConnectionString, ProviderName);

            string username = "user7";
            string password = "asdf7";

            repo.CreateLocal(username, password);

            Assert.IsFalse(repo.Validate(username, "asdf8"));
        }

        [TestMethod]
        public void GetUserIdShouldReturnUserId()
        {
            var repo = new UserRepository(ConnectionString, ProviderName);

            string username = "user9";
            string password = "asdf9";

            repo.CreateLocal(username, password);

            int userId = repo.GetUserId(username);
            Assert.IsTrue(userId > 0);
        }

        [TestMethod]
        public void GetUserIdShouldReturnIvalidIdForAbsentUserName()
        {
            var repo = new UserRepository(ConnectionString, ProviderName);

            string username = "baduser9";

            int userId = repo.GetUserId(username);
            Assert.AreEqual(-1, userId);
        }

        [TestMethod]
        public void GetUserByIdShouldReturnUsername()
        {
            var repo = new UserRepository(ConnectionString, ProviderName);

            string username = "user10";
            string password = "asdf10";

            repo.CreateLocal(username, password);

            int userId = repo.GetUserId(username);

            string name = repo.GetUserById(userId);
            Assert.AreEqual(username, name);
        }

        [TestMethod]
        public void CreateUserShouldAddToUserTable()
        {
            var repo = new UserRepository(ConnectionString, ProviderName);

            string username = "user11";

            int userId = repo.CreateUser(username);

            UserDb userDb = Db.SingleOrDefault<UserDb>(userId);
            Assert.AreEqual(username, userDb.UserName);
        }

    }
}
