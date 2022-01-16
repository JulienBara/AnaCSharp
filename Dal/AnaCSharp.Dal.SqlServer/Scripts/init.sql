USE master;
GO

IF DB_ID (N'ana') IS NULL
CREATE DATABASE ana;
GO

USE ana;
GO

IF NOT EXISTS (SELECT * FROM sysobjects where name='Words')
CREATE TABLE Words (
    WordId INT PRIMARY KEY IDENTITY(1,1),
    Label NVARCHAR(450) NOT NULL
)
GO
CREATE INDEX Words_Label ON Words (Label)
GO

IF NOT EXISTS (SELECT * FROM sysobjects where name='DeterminingStates')
CREATE TABLE DeterminingStates (
    DeterminingStateId INT PRIMARY KEY IDENTITY(1,1)
)
GO

IF NOT EXISTS (SELECT * FROM sysobjects where name='DeterminingWords')
CREATE TABLE DeterminingWords (
    DeterminingWordId INT PRIMARY KEY IDENTITY(1,1),
    WordId INT NOT NULL,
    DeterminingStateId INT NOT NULL,
    [Order] INT NOT NULL,
    FOREIGN KEY (WordId) REFERENCES dbo.Words (WordId),
    FOREIGN KEY (DeterminingStateId) REFERENCES dbo.DeterminingStates (DeterminingStateId)
)
GO
CREATE INDEX DeterminingWords_WordId_Order ON DeterminingWords (WordId, [Order])
GO

IF NOT EXISTS (SELECT * FROM sysobjects where name='DeterminedWords')
CREATE TABLE DeterminedWords (
    DeterminedWordId INT PRIMARY KEY IDENTITY(1,1),
    DeterminingStateId INT NOT NULL,
    WordId INT NOT NULL,
    Number INT NOT NULL,
    FOREIGN KEY (DeterminingStateId) REFERENCES dbo.DeterminingStates (DeterminingStateId),
    FOREIGN KEY (WordId) REFERENCES dbo.Words (WordId)
)
GO
CREATE INDEX DeterminingState_DeterminingStateId ON DeterminedWords (DeterminingStateId)
GO
