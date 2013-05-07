using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediane.DomainModel
{
    public class MedianeSql
    {
        public string ArticleByTitle
        {
            get { return "SELECT * FROM ARTICLES WHERE Title=@0"; }
        }

        public string ClearAll
        {
            get
            {
                return
@"
DELETE FROM articles;
";
            }
        }

        public string SchemaVersions
        {
            get { return "SELECT * from SchemaVersions AS sv ORDER BY sv.Applied"; }
        }

        public string SchemaVersionsCount
        {
            get { return "SELECT COUNT(*) FROM SchemaVersions"; }
        }

        public DbUp.Engine.SqlScript[] InitScripts
        {
            get
            {
                var scripts = new List<DbUp.Engine.SqlScript>();

                scripts.Add(new DbUp.Engine.SqlScript("Init001_Create_Tables", Init001_Create_Tables));

                return scripts.ToArray();
            }
        }

        public string Init001_Create_Tables
        {
            get
            {
                return
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
            }
        }

        public string AddPage1
        {
            get
            {
                return "INSERT INTO Articles (Title, Content) VALUES ('Page 1', 'Content of Page 1')";
            }
        }
    }

}