using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Mediane.DomainModel
{
    public class InitDb
    {
        const string DataDirectoryProperty = "DataDirectory";

        public static string GetDataDirectory()
        {
            return (string) AppDomain.CurrentDomain.GetData(DataDirectoryProperty);
        }

        public static void SetDataDirectory(string directory)
        {
            AppDomain.CurrentDomain.SetData(DataDirectoryProperty, directory);
        }

        private readonly string dbName;
        private readonly MedianeSql sql;
        private readonly string ConnectionString;
        private readonly string dbFileName;

        public InitDb(string newDbName, MedianeSql newSql)
        {
            dbName = newDbName;
            sql = newSql;
            ConnectionString = "DataSource=|DataDirectory|\\" + dbName;
            dbFileName = Path.Combine(GetDataDirectory(), dbName);
        }

        public InitDb(string newDbName, MedianeSql newSql, string newDbFileName)
        {
            dbName = newDbName;
            sql = newSql;
            dbFileName = newDbFileName;
            ConnectionString = "DataSource=" + dbFileName;
        }

        public void CreateDbIfNotExist()
        {
            string connectionString = GetConnectionString();

            using (SqlCeEngine engine = new SqlCeEngine(connectionString))
            {
                if (File.Exists(dbFileName))
                {
                    if (!engine.Verify())
                    {
                        throw new Exception(String.Format("Invalid database {0}", dbName));
                    }
                }
                else
                {
                    engine.CreateDatabase();
                }
            }

            var upgrader = DbUp.DeployChanges.To
                .SqlCeDatabase(connectionString)
                .WithScripts(sql.InitScripts)
                .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                throw new Exception("Cannot upgrade database", result.Error);
            }
            
        }

        public string GetConnectionString()
        {
            return ConnectionString;
        }

        public string GetProviderName()
        {
            return "System.Data.SqlServerCe.4.0";
        }
    }
}