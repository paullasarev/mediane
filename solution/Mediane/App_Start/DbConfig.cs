using Mediane.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediane
{
    public static class DbConfig
    {
        const string DbName = "MedianeDb.sdf";

        public static void InitOrUpgrade()
        {
            var sql = new MedianeSql();
            var initDb = new InitDb(DbName, sql);
            initDb.CreateDbIfNotExist();
        }
    }
}