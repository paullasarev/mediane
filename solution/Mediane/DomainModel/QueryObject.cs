using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediane.DomainModel
{
    public class MedianeSql
    {
        public readonly string ClearAll =
@"
DELETE FROM articles;
GO
DELETE FROM users;
GO
";

        public readonly string SchemaVersions =
            "SELECT * from SchemaVersions AS sv ORDER BY sv.Applied";

        public readonly string SchemaVersionsCount =
            "SELECT COUNT(*) FROM SchemaVersions";

        public DbUp.Engine.SqlScript[] InitScripts
        {
            get
            {
                var scripts = new List<DbUp.Engine.SqlScript>();

                scripts.Add(new DbUp.Engine.SqlScript("Init001_Create_Tables", Init001_Create_Tables));
                scripts.Add(new DbUp.Engine.SqlScript("Init002_Add_User_Table", Init002_Add_User_Table));
                scripts.Add(new DbUp.Engine.SqlScript("Init003_Add_UserId_Column", Init003_Add_UserId_Column));

                return scripts.ToArray();
            }
        }

        public readonly string Init001_Create_Tables =
@"
CREATE TABLE [Articles] (
  [ArticleId] bigint NOT NULL  IDENTITY (1,1)
, [Title] nvarchar(200) NOT NULL
, [Content] ntext NOT NULL
);
GO
ALTER TABLE [Articles] ADD CONSTRAINT [PK__Articles] PRIMARY KEY ([ArticleId]);
GO
CREATE UNIQUE INDEX [ArticleTitles] ON [Articles] ([Title] ASC);
GO
";

        public readonly string Init002_Add_User_Table =
@"
CREATE TABLE [Users] (
  [Username] nvarchar(50) NOT NULL
, [Password] nvarchar(50) NOT NULL
);
GO
ALTER TABLE [Users] ADD CONSTRAINT [PK__Users] PRIMARY KEY ([Username]);
GO
";

        public readonly string Init003_Add_UserId_Column =
@"
ALTER TABLE [Users] ADD [UserId] int IDENTITY (1,1);
GO
";

        public readonly string AddPage1 = 
            "INSERT INTO Articles (Title, Content) VALUES ('Page 1', 'Content of Page 1')";

        public readonly string ArticleByTitle =
            "SELECT * FROM [Articles] WHERE Title=@0";

        public readonly string UserByUsername =
            "SELECT * FROM [Users] WHERE [Username]=@0";
        
        public readonly string UserByUserId =
            "SELECT * FROM [Users] WHERE [UserId]=@0";
    }

}