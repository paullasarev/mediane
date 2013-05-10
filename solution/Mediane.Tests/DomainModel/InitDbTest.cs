using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DbUp;
using System.Data.SqlServerCe;
using System.IO;
using Mediane.DomainModel;
using System.Collections.Generic;
using System.Web.Helpers;

namespace Mediane.Tests.DomainModel
{
    [TestClass]
    public class InitDbTest
    {
        static string CurrentDataDirectory;
        static string CurrentDirectory = Directory.GetCurrentDirectory();
        const string DbName = "TestDb.sdf";

        string DbFullFileName;
        static MedianeSql Sql;
        InitDb InitDbInctance;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            CurrentDataDirectory = InitDb.GetDataDirectory();
            InitDb.SetDataDirectory(CurrentDirectory);

            Sql = new MedianeSql();
        }

        [ClassCleanup]
        public static void Done()
        {
            InitDb.SetDataDirectory(CurrentDataDirectory);
        }

        [TestInitialize]
        public void SetUp()
        {
            DbFullFileName = Path.Combine(CurrentDirectory, DbName);
            File.Delete(DbFullFileName);

            InitDbInctance = new InitDb(DbName, Sql);
        }

        [TestMethod]
        public void ShouldCreateDatabaseIfItNotExists()
        {
            InitDbInctance.CreateDbIfNotExist();

            Assert.IsTrue(File.Exists(DbName));
        }

        [TestMethod]
        public void ShouldNotCreateDatabaseIfItExists()
        {
            InitDbInctance.CreateDbIfNotExist();
            using (var db = new PetaPoco.Database(InitDbInctance.GetConnectionString(), InitDbInctance.GetProviderName()))
            {
                db.Execute(Sql.AddPage1);
            }

            var fi1 = new FileInfo(DbFullFileName);

            InitDbInctance.CreateDbIfNotExist();

            var fi2 = new FileInfo(DbFullFileName);
            Assert.AreEqual(fi1.Length, fi2.Length);
        }

        [TestMethod]
        public void ShouldThrowOnWrongDbFile()
        {
            File.Create(DbFullFileName).Close();

            try
            {
                InitDbInctance.CreateDbIfNotExist();
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod]
        public void ShouldApplyDbUpScripts()
        {
            InitDbInctance.CreateDbIfNotExist();

            using (var db = new PetaPoco.Database(InitDbInctance.GetConnectionString(), InitDbInctance.GetProviderName()))
            {
                int count = db.ExecuteScalar<int>(Sql.SchemaVersionsCount);
                Assert.IsTrue(count > 0);
            }
        }

        [TestMethod]
        public void ShouldCreateSimpleMembersipProviderTables()
        {
            InitDbInctance.CreateDbIfNotExist();

            using (var db = new PetaPoco.Database(InitDbInctance.GetConnectionString(), InitDbInctance.GetProviderName()))
            {
                Assert.IsFalse(InitDbInctance.ColumnExist(db, "Users", "Password"));
                Assert.IsTrue(InitDbInctance.ColumnExist(db, "Users", "UserId"));
                Assert.IsTrue(InitDbInctance.ColumnExist(db, "Users", "UserName"));

                Assert.IsTrue(InitDbInctance.TableExist(db, "webpages_Membership"));
                Assert.IsTrue(InitDbInctance.TableExist(db, "webpages_OAuthMembership"));
                Assert.IsTrue(InitDbInctance.TableExist(db, "webpages_OAuthToken"));
            }
        }

        [TestMethod]
        public void ShouldCreateSimpleRoleProviderTables()
        {
            InitDbInctance.CreateDbIfNotExist();

            using (var db = new PetaPoco.Database(InitDbInctance.GetConnectionString(), InitDbInctance.GetProviderName()))
            {
                Assert.IsTrue(InitDbInctance.TableExist(db, "webpages_Roles"));
                Assert.IsTrue(InitDbInctance.TableExist(db, "webpages_UsersInRoles"));
            }
        }

        [TestMethod]
        public void ShouldCreateAdministratorLocalAccount()
        {
            InitDbInctance.CreateDbIfNotExist();

            using (var db = new PetaPoco.Database(InitDbInctance.GetConnectionString(), InitDbInctance.GetProviderName()))
            {
                string username = "administrator";
                string password = "root";
                
                UserDb userDb = db.SingleOrDefault<UserDb>(Sql.UserByUsername, username);
                Assert.AreEqual(username, userDb.UserName);
                var userId = userDb.UserId;

                MembershipDb membershipDb = db.SingleOrDefault<MembershipDb>(userId);
                Assert.IsTrue(Crypto.VerifyHashedPassword(membershipDb.Password, password));
                Assert.IsTrue(membershipDb.IsConfirmed);
                Assert.IsTrue(membershipDb.CreateDate.HasValue);
                Assert.AreEqual(DateTime.Now.Date, membershipDb.CreateDate.Value.Date);
            }
        }
    }
}
