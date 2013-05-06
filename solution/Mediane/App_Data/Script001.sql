-- Script Date: 06.05.2013 19:18  - ErikEJ.SqlCeScripting version 3.5.2.26
-- Database information:
-- Locale Identifier: 1033
-- Encryption Mode: 
-- Case Sensitive: False
-- Database: D:\dotnet\mediane\solution\Mediane\App_Data\MedianeDb.sdf
-- ServerVersion: 4.0.8876.1
-- DatabaseSize: 86016
-- Created: 29.04.2013 21:50

-- User Table information:
-- Number of tables: 1
-- Articles: 2 row(s)

CREATE TABLE [Articles] (
  [Title] nvarchar(200) NOT NULL
, [Content] ntext NOT NULL
, [ArticleId] bigint NOT NULL  IDENTITY (1,1)
);
GO
ALTER TABLE [Articles] ADD CONSTRAINT [PK__Articles__0000000000000013] PRIMARY KEY ([ArticleId]);
GO
CREATE UNIQUE INDEX [ArticleTitles] ON [Articles] ([Title] ASC);
GO

