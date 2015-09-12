USE VotingSystem;

CREATE TABLE [dbo].[Questions]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Content] NVARCHAR(250) NOT NULL, 
    [UrlId] CHAR(10) NOT NULL
)

GO

ALTER Table [dbo].[Questions]
ADD  RequireNames BIT NOT NULL
GO

CREATE UNIQUE INDEX [UIX_Questions_UrlId] ON [dbo].[Questions] ([UrlId])
