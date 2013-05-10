using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

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
                scripts.Add(new DbUp.Engine.SqlScript("Init003_CreateSimpleMembershipTables", Init003_CreateSimpleMembershipTables));

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

        public readonly string Init003_CreateSimpleMembershipTables =
@"
DROP TABLE [Users];
GO
CREATE TABLE [Users] (
  [UserId] int NOT NULL PRIMARY KEY IDENTITY,
  [UserName] nvarchar(56) NOT NULL UNIQUE
);
GO
CREATE TABLE [webpages_Membership] (
  [UserId] int NOT NULL PRIMARY KEY,
  [CreateDate] datetime,
  [ConfirmationToken] nvarchar(128),
  [IsConfirmed] bit DEFAULT 0,
  [LastPasswordFailureDate] datetime,
  [PasswordFailuresSinceLastSuccess] int NOT NULL DEFAULT 0,
  [Password] nvarchar(128) NOT NULL,
  [PasswordChangedDate] datetime,
  [PasswordSalt] nvarchar(128) NOT NULL,
  [PasswordVerificationToken] nvarchar(128),
  [PasswordVerificationTokenExpirationDate] datetime,
  CONSTRAINT fk_UserId FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId])
);
GO
CREATE TABLE [webpages_OAuthMembership] (
  [Provider] nvarchar(30) NOT NULL,
  [ProviderUserId] nvarchar(100) NOT NULL,
  [UserId] int NOT NULL,
  PRIMARY KEY ([Provider], [ProviderUserId])
);
GO
CREATE TABLE [webpages_OAuthToken] (
  [Token] nvarchar(100) NOT NULL,
  [Secret] nvarchar(100) NOT NULL,
  PRIMARY KEY ([Token])
);
GO
CREATE TABLE [webpages_Roles] (
  [RoleId] int NOT NULL PRIMARY KEY IDENTITY,
  [RoleName] nvarchar(256) NOT NULL UNIQUE
);
GO
CREATE TABLE [webpages_UsersInRoles] (
  [UserId] int NOT NULL,
  [RoleId] int NOT NULL,
  PRIMARY KEY ([UserId], [RoleId]),
  CONSTRAINT fk_UserId FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId]),
  CONSTRAINT fk_RoleId FOREIGN KEY ([RoleId]) REFERENCES [webpages_Roles]([RoleId])
);
GO
INSERT INTO [Users] ([UserName]) VALUES ('" + adminUsername + @"');
GO
INSERT INTO [webpages_Membership] ([UserId], [Password], [PasswordSalt], [IsConfirmed], CreateDate) VALUES (@@IDENTITY, '" + hashedAdminPassword + @"', '', 1, GETDATE());
GO
";

        private const string adminUsername = "administrator";
        private const string adminPassword = "root";
        private static string hashedAdminPassword = Crypto.HashPassword(adminPassword);

        public readonly string AddPage1 = 
            "INSERT INTO Articles (Title, Content) VALUES ('Page 1', 'Content of Page 1')";

        public readonly string ArticleByTitle =
            "SELECT * FROM [Articles] WHERE Title=@0";

        public readonly string UserByUsername =
            "SELECT * FROM [Users] WHERE [UserName]=@0";
        
        public readonly string UserByUserId =
            "SELECT * FROM [Users] WHERE [UserId]=@0";

        public readonly string TableExist =
            "SELECT COUNT(*) from INFORMATION_SCHEMA.TABLES where TABLE_NAME = @0";
        
        public readonly string ColumnExist =
            "SELECT COUNT(*) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @0 AND COLUMN_NAME = @1";
    }

}