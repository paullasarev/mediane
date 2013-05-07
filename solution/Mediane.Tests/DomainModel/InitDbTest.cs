using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DbUp;
using System.Data.SqlServerCe;
using System.IO;
using Mediane.DomainModel;
using System.Collections.Generic;

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

    }
}
